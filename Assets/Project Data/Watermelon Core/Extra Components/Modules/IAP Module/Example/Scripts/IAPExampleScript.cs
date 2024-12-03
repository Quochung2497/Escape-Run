#pragma warning disable 0649

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class IAPExampleScript : MonoBehaviour
    {
        [SerializeField] Text logText;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] RectTransform contentParent;

        private void Start()
        {
            logText.text = string.Empty;

            GameLoading.MarkAsReadyToHide();

            InitItems();
        }

        private void InitItems()
        {
#if MODULE_IAP
            ProductKeyType[] values = (ProductKeyType[])Enum.GetValues(typeof(ProductKeyType));
            ItemPanelScript itemPanelScript;
            UnityEngine.Purchasing.Product product;
            GameObject itemGameObject;

            for (int i = 0; i < values.Length; i++)
            {
                product = IAPManager.GetProduct(values[i]);

                if (product == null)
                {
                    Debug.Log("Null product :" + values[i]);
                    continue;
                }

                itemGameObject = Instantiate(itemPrefab, contentParent);
                itemGameObject.transform.position = new Vector3(0, -200 * i);
                itemPanelScript = itemGameObject.GetComponent<ItemPanelScript>();
                itemPanelScript.Item = values[i];
                itemPanelScript.Type = product.definition.type.ToString();
                itemPanelScript.Name =  values[i].ToString();
                //itemPanelScript.Price = string.Format("{0} {1}", product.metadata.isoCurrencyCode, product.metadata.localizedPrice);
                itemPanelScript.Price = string.Format("({0} {1})", product.metadata.localizedPrice, product.metadata.isoCurrencyCode);

                itemPanelScript.SetPurchasedTextActive(product.hasReceipt);
                
                if((product.definition.type == UnityEngine.Purchasing.ProductType.Subscription) && (IAPManager.IsSubscribed(values[i])))
                {
                    itemPanelScript.Purchased = "(subscribed)";
                }

            }
            contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x, values.Length * 200);
#else
            Log("IAP Define is disabled!");
#endif
        }

        public void RestoreButton()
        {
            IAPManager.RestorePurchases();
        }

        #region Handle logs
        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Log(string condition, string stackTrace, LogType type)
        {
            logText.text = logText.text.Insert(0, condition + "\n");
        }

        private void Log(string condition)
        {
            logText.text = logText.text.Insert(0, condition + "\n");
        }
        #endregion
    }
}
