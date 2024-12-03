#pragma warning disable 0649
#pragma warning disable 0162

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Watermelon
{
    [Define("MODULE_ADMOB")]
    [Define("MODULE_UNITYADS")]
    public static class AdsManager
    {
        private const int INIT_ATTEMPTS_AMOUNT = 30;

        public const ProductKeyType NO_ADS_PRODUCT_KEY = ProductKeyType.NoAds;

        private const string NO_ADS_PREF_NAME = "ADS_STATE";
        private const string NO_ADS_ACTIVE_HASH = "809d08040da0182f4fffa4702095e69e";

        private const string GDPR_PREF_NAME = "GDPR_STATE";

        private static bool isForcedAdEnabled;

        private static bool isModuleInitialized;

        private static GameObject dummyCanvasPrefab;

        private static GameObject gdprPrefab;
        public static GameObject GDPRPrefab => gdprPrefab;

        private static AdsData settings;
        public static AdsData Settings => settings;

        // Fired when module is initialized
        public static OnAdsModuleInitializedCallback OnAdsModuleInitializedEvent;

        // Fired when a banner is loaded
        public static OnAdsCallback OnBannerAdLoadedEvent;
        // Fired when a banner has failed to load
        public static OnAdsCallback OnBannerAdLoadFailedEvent;
        // Fired when a banner ad is clicked
        public static OnAdsCallback OnBannerAdClickedEvent;

        // Fired when an interstitial ad is loaded and ready to be shown
        public static OnAdsCallback OnInterstitialLoadedEvent;
        // Fired when an interstitial ad fails to load
        public static OnAdsCallback OnInterstitialLoadFailedEvent;
        // Fired when an interstitial ad is dismissed
        public static OnAdsCallback OnInterstitialHiddenEvent;
        // Fired when an interstitial ad is displayed (may not be received by Unity until the interstitial closes)
        public static OnAdsCallback OnInterstitialDisplayedEvent;
        // Fired when an interstitial ad is clicked (may not be received by Unity until the interstitial closes)
        public static OnAdsCallback OnInterstitialClickedEvent;

        // Fired when a rewarded ad finishes loading and is ready to be displayed
        public static OnAdsCallback OnRewardedAdLoadedEvent;
        // Fired when a rewarded ad fails to load. Includes the error message.
        public static OnAdsCallback OnRewardedAdLoadFailedEvent;
        // Fired when an rewarded ad is displayed (may not be received by Unity until the rewarded ad closes)
        public static OnAdsCallback OnRewardedAdDisplayedEvent;
        // Fired when an rewarded ad is hidden
        public static OnAdsCallback OnRewardedAdHiddenEvent;
        // Fired when an rewarded video is clicked (may not be received by Unity until the rewarded ad closes)
        public static OnAdsCallback OnRewardedAdClickedEvent;
        // Fired when a rewarded video completes. Includes information about the reward
        public static OnAdsCallback OnRewardedAdReceivedRewardEvent;

        public static IsAdsReadyCallback ExtraInterstitialReadyConditions;
        public static IsAdsReadyCallback ExtraBannerReadyConditions;

        private static double lastInterstitialTime;

        private static AdvertisingHandler.RewardedVideoCallback rewardedVideoCallback;
        private static AdvertisingHandler.InterstitialCallback interstitalCallback;

        private static List<MainThreadEvent> mainThreadEvents = new List<MainThreadEvent>();
        private static int mainThreadEventsCount;

        private static bool isFirstAdLoaded = false;
        private static int initAttemps = 0;
        private static bool waitingForRewardVideoCallback;

        private static Coroutine loadingCoroutine;

        private static AdvertisingHandler[] advertisingModules = new AdvertisingHandler[]
        {
            new DummyHandler(AdvertisingModules.Dummy), // Dummy

#if MODULE_ADMOB
            new AdMobHandler(AdvertisingModules.AdMob), // AdMob module
#endif

#if MODULE_UNITYADS
            new UnityAdsLegacyHandler(AdvertisingModules.UnityAdsLegacy), // Unity Ads Legacy module
#endif
        };

        private static Dictionary<AdvertisingModules, AdvertisingHandler> advertisingActiveModules = new Dictionary<AdvertisingModules, AdvertisingHandler>();

#region Init
        public static void Init(AdsManagerInitModule adsManagerInitModule, bool loadOnStart)
        {
            if (isModuleInitialized)
            {
                Debug.LogWarning("[AdsManager]: Module already exists!");

                return;
            }

            isModuleInitialized = true;

            isFirstAdLoaded = false;
            initAttemps = 0;

            settings = adsManagerInitModule.settings;
            dummyCanvasPrefab = adsManagerInitModule.dummyCanvasPrefab;
            gdprPrefab = adsManagerInitModule.gdprPrefab;

            isForcedAdEnabled = IsForcedAdEnabled(false);

            if (settings == null)
            {
                Debug.LogError("[AdsManager]: Settings don't exist!");

                return;
            }

            Initialiser.InitialiserGameObject.AddComponent<AdsManager.AdEventExecutor>();

            advertisingActiveModules = new Dictionary<AdvertisingModules, AdvertisingHandler>();
            for (int i = 0; i < advertisingModules.Length; i++)
            {
                if (IsModuleEnabled(advertisingModules[i].ModuleType))
                {
                    advertisingActiveModules.Add(advertisingModules[i].ModuleType, advertisingModules[i]);
                }
            }

            if (settings.SystemLogs)
            {
                if (settings.BannerType != AdvertisingModules.Disable && !advertisingActiveModules.ContainsKey(settings.BannerType))
                    Debug.LogWarning("[AdsManager]: Banner type (" + settings.BannerType + ") is selected, but isn't active!");

                if (settings.InterstitialType != AdvertisingModules.Disable && !advertisingActiveModules.ContainsKey(settings.InterstitialType))
                    Debug.LogWarning("[AdsManager]: Interstitial type (" + settings.InterstitialType + ") is selected, but isn't active!");

                if (settings.RewardedVideoType != AdvertisingModules.Disable && !advertisingActiveModules.ContainsKey(settings.RewardedVideoType))
                    Debug.LogWarning("[AdsManager]: Rewarded Video type (" + settings.RewardedVideoType + ") is selected, but isn't active!");
            }

            if (IsModuleEnabled(AdvertisingModules.Dummy))
                InitDummy();

            lastInterstitialTime = TimeUtils.GetCurrentUnixTimestamp() + settings.InsterstitialFirstDelay;

            // Callbacks
            OnInterstitialDisplayedEvent += OnInterstitialDisplayed;
            OnInterstitialHiddenEvent += OnInterstitialHidden;
            OnRewardedAdHiddenEvent += OnRewardedAdHidden;
            OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedReward;

            // Validate
            ExtraInterstitialReadyConditions += CheckInterstitialTime;

#if MODULE_IAP
            IAPManager.OnPurchaseComplete += OnPurchaseComplete;
#endif

            // Add loading task if GDPR isn't created
            if (settings.IsGDPREnabled && !IsGDPRStateExist())
            {
                GDPRLoadingTask gdprLoadingTask = new GDPRLoadingTask();
                gdprLoadingTask.OnTaskCompleted += () =>
                {
                    InitModules();

                    if (loadOnStart)
                        TryToLoadFirstAds();
                };

                GameLoading.AddTask(gdprLoadingTask);

                return;
            }

            InitModules();

            if (loadOnStart)
            {
                TryToLoadFirstAds();
            }
        }

        private static void InitModules()
        {
            foreach (var advertisingModule in advertisingActiveModules.Keys)
            {
                InitModule(advertisingModule);
            }
        }

        private static void InitModule(AdvertisingModules advertisingModule)
        {
            if (advertisingActiveModules.ContainsKey(advertisingModule))
            {
                if(!advertisingActiveModules[advertisingModule].IsInitialized())
                {
                    if (settings.SystemLogs)
                        Debug.Log("[AdsManager]: Module " + advertisingModule.ToString() + " trying to initialize!");

                    advertisingActiveModules[advertisingModule].Init(settings);
                }
                else
                {
                    if (settings.SystemLogs)
                        Debug.Log("[AdsManager]: Module " + advertisingModule.ToString() + " is already initialized!");
                }
            }
            else
            {
                if (settings.SystemLogs)
                    Debug.LogWarning("[AdsManager]: Module " + advertisingModule.ToString() + " is disabled!");
            }
        }

        private static void InitDummy()
        {
            // Inititalize dummy controller
            if (settings.IsDummyEnabled())
            {
                if (dummyCanvasPrefab != null)
                {
                    GameObject dummyCanvas = GameObject.Instantiate(dummyCanvasPrefab);
                    dummyCanvas.transform.position = Vector3.zero;
                    dummyCanvas.transform.localScale = Vector3.one;
                    dummyCanvas.transform.rotation = Quaternion.identity;

                    AdsDummyController adsDummyController = dummyCanvas.GetComponent<AdsDummyController>();
                    adsDummyController.Init(settings);

                    DummyHandler dummyHandler = (DummyHandler)System.Array.Find(advertisingModules, x => x.ModuleType == AdvertisingModules.Dummy);
                    if (dummyHandler != null)
                        dummyHandler.SetDummyController(adsDummyController);
                }
                else
                {
                    Debug.LogError("[AdsManager]: Dummy controller can't be null!");
                }
            }
        }
#endregion

        private static void Update()
        {
            if (mainThreadEventsCount > 0)
            {
                for (int i = 0; i < mainThreadEventsCount; i++)
                {
                    mainThreadEvents[i].Callback?.Invoke();
                }

                mainThreadEvents.Clear();
                mainThreadEventsCount = 0;
            }
        }

        public static void TryToLoadFirstAds()
        {
            if(loadingCoroutine == null)
                loadingCoroutine = Tween.InvokeCoroutine(TryToLoadAdsCoroutine());
        }

        private static IEnumerator TryToLoadAdsCoroutine()
        {
            yield return new WaitForSeconds(1.0f);

            while (!isFirstAdLoaded || initAttemps > INIT_ATTEMPTS_AMOUNT)
            {
                if (LoadFirstAds())
                    break;

                yield return new WaitForSeconds(1.0f * (initAttemps + 1));

                initAttemps++;
            }

            if (settings.SystemLogs)
                Debug.Log("[AdsManager]: First ads have loaded!");
        }

        private static bool LoadFirstAds()
        {
            if (isFirstAdLoaded)
                return true;

            if (settings.IsGDPREnabled && !AdsManager.IsGDPRStateExist())
                return false;

            if (settings.IsIDFAEnabled && !AdsManager.IsIDFADetermined())
                return false;

            bool isRewardedVideoModuleInititalized = AdsManager.IsModuleInititalized(AdsManager.Settings.RewardedVideoType);
            bool isInterstitialModuleInitialized = AdsManager.IsModuleInititalized(AdsManager.Settings.InterstitialType);
            bool isBannerModuleInitialized = AdsManager.IsModuleInititalized(AdsManager.Settings.BannerType);

            bool isRewardedVideoActive = AdsManager.Settings.RewardedVideoType != AdvertisingModules.Disable;
            bool isInterstitialActive = AdsManager.Settings.InterstitialType != AdvertisingModules.Disable;
            bool isBannerActive = AdsManager.Settings.BannerType != AdvertisingModules.Disable;

            if ((!isRewardedVideoActive || isRewardedVideoModuleInititalized) && (!isInterstitialActive || isInterstitialModuleInitialized) && (!isBannerActive || isBannerModuleInitialized))
            {
                if (isRewardedVideoActive)
                    AdsManager.RequestRewardBasedVideo();

                bool isForcedAdEnabled = AdsManager.IsForcedAdEnabled(false);
                if (isInterstitialActive && isForcedAdEnabled)
                    AdsManager.RequestInterstitial();

                if (isBannerActive && isForcedAdEnabled)
                    AdsManager.ShowBanner();

                isFirstAdLoaded = true;

                return true;
            }

            return false;
        }

        public static void CallEventInMainThread(SimpleCallback callback)
        {
            if (callback != null)
            {
                mainThreadEvents.Add(new MainThreadEvent(callback));
                mainThreadEventsCount++;
            }
        }

        public static void ShowErrorMessage()
        {
            FloatingMessage.ShowMessage("Network error. Please try again later");
        }

        public static bool IsModuleEnabled(AdvertisingModules advertisingModule)
        {
            if (advertisingModule == AdvertisingModules.Disable)
                return false;

            return (Settings.BannerType == advertisingModule || Settings.InterstitialType == advertisingModule || Settings.RewardedVideoType == advertisingModule);
        }

        public static bool IsModuleActive(AdvertisingModules advertisingModule)
        {
            return advertisingActiveModules.ContainsKey(advertisingModule);
        }

        public static bool IsModuleInititalized(AdvertisingModules advertisingModule)
        {
            if (advertisingActiveModules.ContainsKey(advertisingModule))
            {
                return advertisingActiveModules[advertisingModule].IsInitialized();
            }

            return false;
        }

#region Interstitial
        public static bool IsInterstitialLoaded()
        {
            return IsInterstitialLoaded(settings.InterstitialType);
        }

        public static bool IsInterstitialLoaded(AdvertisingModules advertisingModules)
        {
            if (!isForcedAdEnabled)
                return false;

            if (!IsModuleActive(advertisingModules))
                return false;

            return advertisingActiveModules[advertisingModules].IsInterstitialLoaded();
        }

        public static void RequestInterstitial()
        {
            RequestInterstitial(settings.InterstitialType);
        }

        public static void RequestInterstitial(AdvertisingModules advertisingModules)
        {
            if (!isForcedAdEnabled)
                return;

            if (!IsModuleActive(advertisingModules))
                return;

            if (advertisingActiveModules[advertisingModules].IsInterstitialLoaded())
                return;

            advertisingActiveModules[advertisingModules].RequestInterstitial();
        }

        public static void ShowInterstitial(AdvertisingHandler.InterstitialCallback callback)
        {
            ShowInterstitial(settings.InterstitialType, callback);
        }

        public static void ShowInterstitial(AdvertisingModules advertisingModules, AdvertisingHandler.InterstitialCallback callback)
        {
            interstitalCallback = callback;

            if (!isForcedAdEnabled)
            {
                ExecuteInterstitialCallback(false);

                return;
            }

            if (!IsModuleActive(advertisingModules))
            {
                ExecuteInterstitialCallback(false);

                return;
            }

            if (!CheckExtraInterstitialCondition())
            {
                ExecuteInterstitialCallback(false);

                return;
            }

            if (!advertisingActiveModules[advertisingModules].IsInterstitialLoaded())
            {
                ExecuteInterstitialCallback(false);

                return;
            }

            advertisingActiveModules[advertisingModules].ShowInterstitial(callback);
        }

        public static void ExecuteInterstitialCallback(bool result)
        {
            if (interstitalCallback != null)
            {
                CallEventInMainThread(() => interstitalCallback.Invoke(result));
            }
        }

        public static void SetInterstitialDelayTime(float time)
        {
            lastInterstitialTime = TimeUtils.GetCurrentUnixTimestamp() + time;
        }

        public static void ResetInterstitialDelayTime()
        {
            lastInterstitialTime = TimeUtils.GetCurrentUnixTimestamp() + settings.InterstitialShowingDelay;
        }

        private static bool CheckInterstitialTime()
        {
            if (settings.SystemLogs)
                Debug.Log("[AdsManager]: Interstitial Time: " + lastInterstitialTime + "; Time: " + TimeUtils.GetCurrentUnixTimestamp());

            return lastInterstitialTime < TimeUtils.GetCurrentUnixTimestamp();
        }

        public static bool CheckExtraInterstitialCondition()
        {
            if (ExtraInterstitialReadyConditions != null)
            {
                bool state = true;

                System.Delegate[] listDelegates = ExtraInterstitialReadyConditions.GetInvocationList();
                for (int i = 0; i < listDelegates.Length; i++)
                {
                    if (!(bool)listDelegates[i].DynamicInvoke())
                    {
                        state = false;

                        break;
                    }
                }

                if (settings.SystemLogs)
                    Debug.Log("[AdsManager]: Extra condition interstitial state: " + state);

                return state;
            }

            return true;
        }
#endregion

#region Rewarded Video
        public static bool IsRewardBasedVideoLoaded()
        {
            return IsRewardBasedVideoLoaded(settings.RewardedVideoType);
        }

        public static bool IsRewardBasedVideoLoaded(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return false;

            return advertisingActiveModules[advertisingModules].IsRewardedVideoLoaded();
        }

        public static void RequestRewardBasedVideo()
        {
            RequestRewardBasedVideo(settings.RewardedVideoType);
        }

        public static void RequestRewardBasedVideo(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            if (advertisingActiveModules[advertisingModules].IsRewardedVideoLoaded())
                return;

            advertisingActiveModules[advertisingModules].RequestRewardedVideo();
        }

        public static void ShowRewardBasedVideo(AdvertisingHandler.RewardedVideoCallback callback, bool showErrorMessage = true)
        {
            ShowRewardBasedVideo(settings.RewardedVideoType, callback, showErrorMessage);
        }

        public static void ExecuteRewardVideoCallback(bool result)
        {
            if (rewardedVideoCallback != null && waitingForRewardVideoCallback)
            {
                CallEventInMainThread(() => rewardedVideoCallback.Invoke(result));

                waitingForRewardVideoCallback = false;

                if (settings.SystemLogs)
                {
                    Debug.Log("[AdsManager]: Reward received: " + result);
                }
            }
        }

        public static void ShowRewardBasedVideo(AdvertisingModules advertisingModules, AdvertisingHandler.RewardedVideoCallback callback, bool showErrorMessage = true)
        {
            rewardedVideoCallback = callback;
            waitingForRewardVideoCallback = true;

            if (!IsModuleActive(advertisingModules))
            {
                ExecuteRewardVideoCallback(false);

                if (showErrorMessage)
                    ShowErrorMessage();

                return;
            }

            if (!advertisingActiveModules[advertisingModules].IsRewardedVideoLoaded())
            {
                ExecuteRewardVideoCallback(false);

                if (showErrorMessage)
                    ShowErrorMessage();

                return;
            }

            advertisingActiveModules[advertisingModules].ShowRewardedVideo(callback);
        }
#endregion

#region Banner
        public static void ShowBanner()
        {
            ShowBanner(settings.BannerType);
        }

        public static void ShowBanner(AdvertisingModules advertisingModules)
        {
            if (!isForcedAdEnabled)
                return;

            if (!IsModuleActive(advertisingModules))
                return;

            if (!CheckExtraBannerCondition())
                return;

            advertisingActiveModules[advertisingModules].ShowBanner();
        }

        public static void DestroyBanner()
        {
            DestroyBanner(settings.BannerType);
        }

        public static void DestroyBanner(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            advertisingActiveModules[advertisingModules].DestroyBanner();
        }

        public static void HideBanner()
        {
            HideBanner(settings.BannerType);
        }

        public static void HideBanner(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            advertisingActiveModules[advertisingModules].HideBanner();
        }

        public static bool CheckExtraBannerCondition()
        {
            if (ExtraBannerReadyConditions != null)
            {
                bool state = true;

                System.Delegate[] listDelegates = ExtraBannerReadyConditions.GetInvocationList();
                for (int i = 0; i < listDelegates.Length; i++)
                {
                    if (!(bool)listDelegates[i].DynamicInvoke())
                    {
                        state = false;

                        break;
                    }
                }

                if (settings.SystemLogs)
                    Debug.Log("[AdsManager]: Extra condition banner state: " + state);

                return state;
            }

            return true;
        }
#endregion

#region Callbacks
        private static void OnInterstitialDisplayed()
        {
            ResetInterstitialDelayTime();
        }

        private static void OnRewardedAdReceivedReward()
        {
            ResetInterstitialDelayTime();
        }

        private static void OnRewardedAdHidden()
        {
            ResetInterstitialDelayTime();
        }

        private static void OnInterstitialHidden()
        {
            ResetInterstitialDelayTime();
        }
#endregion

#region IAP
        private static void OnPurchaseComplete(ProductKeyType productKeyType)
        {
            if (productKeyType == NO_ADS_PRODUCT_KEY)
            {
                DisableForcedAd();
            }
        }

        public static bool IsForcedAdEnabled(bool useCachedValue = true)
        {
            if (useCachedValue)
                return isForcedAdEnabled;

            return !PlayerPrefs.GetString(NO_ADS_PREF_NAME, "").Equals(NO_ADS_ACTIVE_HASH);
        }

        public static void DisableForcedAd()
        {
            Debug.Log("[IAP Manager]: Banners and interstitials are disabled!");

            PlayerPrefs.SetString(NO_ADS_PREF_NAME, NO_ADS_ACTIVE_HASH);

            isForcedAdEnabled = false;

            HideBanner();
        }
#endregion

#region GDPR
        public static void SetGDPR(bool state)
        {
            PlayerPrefs.SetInt(GDPR_PREF_NAME, state ? 1 : 0);

            foreach (AdvertisingModules activeModule in advertisingActiveModules.Keys)
            {
                if (advertisingActiveModules[activeModule].IsInitialized())
                {
                    advertisingActiveModules[activeModule].SetGDPR(state);
                }
                else
                {
                    InitModule(activeModule);
                }
            }
        }

        public static bool GetGDPRState()
        {
            return PlayerPrefs.GetInt(GDPR_PREF_NAME, 0) == 1 ? true : false;
        }

        public static bool IsGDPRStateExist()
        {
            return PlayerPrefs.HasKey(GDPR_PREF_NAME);
        }
#endregion

#region IDFA
        public static bool IsIDFADetermined()
        {
#if UNITY_IOS
            if(settings.IsIDFAEnabled)
            {
                return Unity.Advertisement.IosSupport.ATTrackingStatusBinding.GetAuthorizationTrackingStatus() != Unity.Advertisement.IosSupport.ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED;
            }
#endif

            return true;
        }
#endregion

        public delegate void OnAdsCallback();
        public delegate void OnAdsModuleInitializedCallback(AdvertisingModules advertisingModules);
        public delegate bool IsAdsReadyCallback();

        private class AdEventExecutor : MonoBehaviour
        {
            private void Update()
            {
                AdsManager.Update();
            }
        }

        private class MainThreadEvent
        {
            private SimpleCallback callback;
            public SimpleCallback Callback => callback;

            public MainThreadEvent(SimpleCallback callback)
            {
                this.callback = callback;
            }
        }
    }
}

// -----------------
// Advertisement v 1.3
// -----------------

// Changelog
// v 1.3
// • Admob v8.1.0 support
// • Removed IronSource provider
// v 1.2.1
// • Some fixes in IronSourse provider
// • Some fixes in Admob provider
// • New interface in Admob provider
// • Added Build Preprocessing for Admob 
// v 1.2
// • Added IronSource provider
// v 1.1f3
// • GDPR style rework
// • Rewarded video error message
// • Removed GDPR check in AdMob module
// v 1.1f2
// • GDPR init bug fixed
// v 1.1
// • Added first ad loader
// • Moved IAP check to AdsManager script
// v 1.0
// • Added documentation
// v 0.3
// • Unity Ads fixed
// v 0.2
// • Bug fix
// v 0.1
// • Added basic version