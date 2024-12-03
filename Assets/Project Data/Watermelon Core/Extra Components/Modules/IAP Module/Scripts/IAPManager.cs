using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if MODULE_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
#endif

namespace Watermelon
{
    [Define("MODULE_IAP")]
    [HelpURL("https://docs.google.com/document/d/1GlS55aF4z4Ddn4a1QCu5h0152PoOb29Iy4y9RKZ9Y9Y")]
    public class IAPManager
    {
        private static IAPManager instance;

        private bool isInititalized = false;

#if MODULE_IAP
        private IAPStoreListener storeListener;
#endif

        private static OnPurchaseModuleInittedCallback OnPurchaseModuleInitted;
        public static OnPurchaseCompleteCallback OnPurchaseComplete;
        public static OnPurchaseFaildedCallback OnPurchaseFailded;

        public async void Init(GameObject initObject, IAPSettings settings)
        {
            if(isInititalized)
            {
                Debug.Log("[IAP Manager]: Module is already initialized!");
                return;
            }

            instance = this;

#if MODULE_IAP
            storeListener = IAPStoreListener.Create(initObject, this, settings.storeItems);

            try
            {
                var options = new InitializationOptions().SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                storeListener.Initialise();
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
            }
#else
            await Task.Run(() => Debug.Log("[IAP Manager]: Define is disabled!"));
#endif
        }

        public static void RestorePurchases()
        {
#if MODULE_IAP
            instance.storeListener.Restore();
#endif
        }


        public static void SubscribeOnPurchaseModuleInitted(OnPurchaseModuleInittedCallback callback)
        {
            if (instance != null && instance.isInititalized)
                callback?.Invoke();
            else
                OnPurchaseModuleInitted += callback;
        }

        public static void BuyProduct(ProductKeyType productKeyType)
        {
#if MODULE_IAP
            instance.storeListener.OnPurchaseClicked(productKeyType);
#else
            FloatingMessage.ShowMessage("Network error.");
#endif
        }

#if MODULE_IAP
        public static Product GetProduct(ProductKeyType productKeyType)
        {
            return instance.storeListener.GetProduct(productKeyType);
        }
#endif

#if MODULE_IAP
        public static bool IsSubscribed(ProductKeyType productKeyType)
        {
            Product product = instance.storeListener.GetProduct(productKeyType);
            if(product != null)
            {
                // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
                if (product.receipt == null)
                    return false;

                //The intro_json parameter is optional and is only used for the App Store to get introductory information.
                SubscriptionManager subscriptionManager = new SubscriptionManager(product, null);

                // The SubscriptionInfo contains all of the information about the subscription.
                // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
                SubscriptionInfo info = subscriptionManager.getSubscriptionInfo();

                return info.isSubscribed() == Result.True;
            }

            return false;
        }
#endif

#if MODULE_IAP
        public static string GetProductLocalPriceString(ProductKeyType productKeyType)
        {
            Product product = instance.storeListener.GetProduct(productKeyType);

            if (product == null)
                return string.Empty;

            return string.Format("{0} {1}", product.metadata.isoCurrencyCode, product.metadata.localizedPrice);
        }
#endif

#if MODULE_IAP
        public class IAPStoreListener : MonoBehaviour, IDetailedStoreListener
        {
            private Dictionary<ProductKeyType, IAPItem> productsTypeToProductLink = new Dictionary<ProductKeyType, IAPItem>();
            private Dictionary<string, IAPItem> productsKeyToProductLink = new Dictionary<string, IAPItem>();

            private IStoreController controller;
            private IExtensionProvider extensions;

            private IAPManager manager;
            private IAPItem[] items;

            public static IAPStoreListener Create(GameObject parentObject, IAPManager manager, IAPItem[] items)
            {
                IAPStoreListener storeListener = parentObject.AddComponent<IAPStoreListener>();

                storeListener.manager = manager;
                storeListener.items = items;

                return storeListener;
            }

            public void Initialise()
            {
                // Init products
                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                for (int i = 0; i < items.Length; i++)
                {
                    builder.AddProduct(items[i].ID, (UnityEngine.Purchasing.ProductType)items[i].ProductType);

                    productsTypeToProductLink.Add(items[i].ProductKeyType, items[i]);
                    productsKeyToProductLink.Add(items[i].ID, items[i]);
                }

                UnityPurchasing.Initialize(this, builder);
            }

            /// <summary>
            /// Called when Unity IAP is ready to make purchases.
            /// </summary>
            public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
            {
                manager.isInititalized = true;

                this.controller = controller;
                this.extensions = extensions;

                foreach (var item in controller.products.all)
                {
                    if (item.availableToPurchase)
                    {
                        if(!string.IsNullOrEmpty(item.definition.id))
                        {
                            if (productsKeyToProductLink.ContainsKey(item.definition.id))
                            {
                                productsKeyToProductLink[item.definition.id].SetProduct(item);
                            }
                        }
                    }
                }

                if (OnPurchaseModuleInitted != null)
                    OnPurchaseModuleInitted.Invoke();

                Debug.Log("[IAPManager]: Module is initialized!");
            }

            /// <summary>
            /// Called when Unity IAP encounters an unrecoverable initialization error.
            ///
            /// Note that this will not be called if Internet is unavailable; Unity IAP
            /// will attempt initialization until it becomes available.
            /// </summary>
            public void OnInitializeFailed(InitializationFailureReason error)
            {
                Debug.Log("[IAPManager]: Module initialization is failed!");
            }

            public void OnInitializeFailed(InitializationFailureReason error, string message)
            {
                Debug.Log(string.Format("[IAPManager]: Module initialization is failed! {0}", message));
            }

            /// <summary>
            /// Called when a purchase completes.
            ///
            /// May be called at any time after OnInitialized().
            /// </summary>
            public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
            {
                Debug.Log("[IAPManager]: Purchasing - " + e.purchasedProduct.definition.id + " is completed!");

                if (productsKeyToProductLink.ContainsKey(e.purchasedProduct.definition.id))
                {
                    IAPItem tempStoreItem = productsKeyToProductLink[e.purchasedProduct.definition.id];
                    if (tempStoreItem != null)
                    {
                        if (OnPurchaseComplete != null)
                            OnPurchaseComplete.Invoke(tempStoreItem.ProductKeyType);
                    }
                }
                else
                {
                    Debug.Log("[IAPManager]: Product - " + e.purchasedProduct.definition.id + " can't be found!");
                }

                IAPCanvas.ChangeLoadingMessage("Payment complete!");
                IAPCanvas.HideLoadingPanel();

                return PurchaseProcessingResult.Complete;
            }

            /// <summary>
            /// Called when a purchase fails.
            /// </summary>
            public void OnPurchaseFailed(Product product, UnityEngine.Purchasing.PurchaseFailureReason failureReason)
            {
                Debug.Log("[IAPManager]: Purchasing - " + product.definition.id + " is failed!");
                Debug.Log("[IAPManager]: Fail reason - " + failureReason.ToString());

                if (productsKeyToProductLink.ContainsKey(product.definition.id))
                {
                    IAPItem tempShopItem = productsKeyToProductLink[product.definition.id];
                    if (tempShopItem != null)
                    {
                        if (OnPurchaseFailded != null)
                            OnPurchaseFailded.Invoke(tempShopItem.ProductKeyType, (Watermelon.PurchaseFailureReason)failureReason);
                    }
                }
                else
                {
                    Debug.Log("[IAPManager]: Product - " + product.definition.id + " can't be found!");
                }

                IAPCanvas.ChangeLoadingMessage("Payment failed!");
                IAPCanvas.HideLoadingPanel();
            }

            public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
            {
                Debug.Log("[IAPManager]: Purchasing - " + product.definition.id + " is failed!");
                Debug.Log("[IAPManager]: Fail reason - " + failureDescription.message);

                if (productsKeyToProductLink.ContainsKey(product.definition.id))
                {
                    IAPItem tempShopItem = productsKeyToProductLink[product.definition.id];
                    if (tempShopItem != null)
                    {
                        if (OnPurchaseFailded != null)
                            OnPurchaseFailded.Invoke(tempShopItem.ProductKeyType, (Watermelon.PurchaseFailureReason)failureDescription.reason);
                    }
                }
                else
                {
                    Debug.Log("[IAPManager]: Product - " + product.definition.id + " can't be found!");
                }

                IAPCanvas.ChangeLoadingMessage("Payment failed!");
                IAPCanvas.HideLoadingPanel();
            }

            public void OnPurchaseClicked(ProductKeyType productKeyType)
            {
                if (!manager.isInititalized)
                {
                    IAPCanvas.ShowMessage("Network error. Please try again later");

                    return;
                }

                IAPCanvas.ShowLoadingPanel();
                IAPCanvas.ChangeLoadingMessage("Payment in progress..");

                controller.InitiatePurchase(productsTypeToProductLink[productKeyType].ID);
            }

            public void Restore()
            {
                if (!manager.isInititalized)
                {
                    IAPCanvas.ShowMessage("Network error. Please try again later");

                    return;
                }

                IAPCanvas.ShowLoadingPanel();
                IAPCanvas.ChangeLoadingMessage("Restoring purchased products..");

                extensions.GetExtension<IAppleExtensions>().RestoreTransactions((result, message) =>
                {
                    if (result)
                    {
                        // This does not mean anything was restored,
                        // merely that the restoration process succeeded.
                        IAPCanvas.ChangeLoadingMessage("Restoration completed!");
                    }
                    else
                    {
                        // Restoration failed.
                        IAPCanvas.ChangeLoadingMessage("Restoration failed!");
                    }

                    IAPCanvas.HideLoadingPanel();
                });
            }

            public Product GetProduct(ProductKeyType productKeyType)
            {
                if (!manager.isInititalized)
                    return null;

                return controller.products.WithID(productsTypeToProductLink[productKeyType].ID);
            }
        }
#endif

        public delegate void OnPurchaseModuleInittedCallback();
        public delegate void OnPurchaseCompleteCallback(ProductKeyType productKeyType);
        public delegate void OnPurchaseFaildedCallback(ProductKeyType productKeyType, Watermelon.PurchaseFailureReason failureReason);
    }

    public enum PurchaseFailureReason
    {
        PurchasingUnavailable = 0,
        ExistingPurchasePending = 1,
        ProductUnavailable = 2,
        SignatureInvalid = 3,
        UserCancelled = 4,
        PaymentDeclined = 5,
        DuplicateTransaction = 6,
        Unknown = 7
    }

    public enum ProductType
    {
        Consumable = 0,
        NonConsumable = 1,
        Subscription = 2
    }
}

// -----------------
// IAP Manager v 1.1
// -----------------

// Changelog
// v 1.1
// • Support of IAP version 4.9.3
// v 1.0.3
// • Support of IAP version 4.7.0
// v 1.0.2
// • Added quick access to the local price of IAP via GetProductLocalPriceString method
// v 1.0.1
// • Added restoration status messages
// v 1.0.0
// • Documentation added
// v 0.4
// • IAPStoreListener inheriting from MonoBehaviour
// v 0.3
// • Editor style update
// v 0.2
// • IAPManager structure changed
// • Enums from UnityEditor.Purchasing has duplicated to prevent serialization problems
// v 0.1
// • Added basic version
