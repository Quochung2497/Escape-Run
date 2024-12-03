
using UnityEngine;

namespace Watermelon
{
    public class DummyHandler : AdvertisingHandler
    {
        private AdsDummyController dummyController;

        private bool isInterstitialLoaded = false;
        private bool isRewardVideoLoaded = false;

        public DummyHandler(AdvertisingModules moduleType) : base(moduleType) { }

        public override void Init(AdsData adsSettings)
        {
            this.adsSettings = adsSettings;

            isInitialized = true;

            if (adsSettings.SystemLogs)
                Debug.Log("[AdsManager]: Module " + ModuleType.ToString() + " has initialized!");

            if (AdsManager.OnAdsModuleInitializedEvent != null)
                AdsManager.OnAdsModuleInitializedEvent.Invoke(ModuleType);

            
        }

        public void SetDummyController(AdsDummyController dummyController)
        {
            this.dummyController = dummyController;
        }

        public override void ShowBanner()
        {
            dummyController.ShowBanner();
        }

        public override void HideBanner()
        {
            dummyController.HideBanner();
        }

        public override void DestroyBanner()
        {
            dummyController.HideBanner();
        }

        public override void RequestInterstitial()
        {
            isInterstitialLoaded = true;

            if (AdsManager.OnInterstitialLoadedEvent != null)
                AdsManager.OnInterstitialLoadedEvent.Invoke();
        }

        public override bool IsInterstitialLoaded()
        {
            return isInterstitialLoaded;
        }

        public override void ShowInterstitial(InterstitialCallback callback)
        {
            dummyController.ShowInterstitial();

            if (AdsManager.OnInterstitialDisplayedEvent != null)
                AdsManager.OnInterstitialDisplayedEvent.Invoke();
        }

        public override void RequestRewardedVideo()
        {
            isRewardVideoLoaded = true;

            if (AdsManager.OnRewardedAdLoadedEvent != null)
                AdsManager.OnRewardedAdLoadedEvent.Invoke();
        }

        public override bool IsRewardedVideoLoaded()
        {
            return isRewardVideoLoaded;
        }

        public override void ShowRewardedVideo(RewardedVideoCallback callback)
        {
            dummyController.ShowRewardedVideo();

            if (AdsManager.OnRewardedAdDisplayedEvent != null)
                AdsManager.OnRewardedAdDisplayedEvent.Invoke();
        }

        public override void SetGDPR(bool state)
        {

        }
    }
}

// -----------------
// Advertisement v 1.3
// -----------------