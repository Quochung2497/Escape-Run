using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Text.RegularExpressions;
#endif
#if MODULE_ADMOB
using GoogleMobileAds.Api;
using Reward = GoogleMobileAds.Api.Reward;
#endif

namespace Watermelon
{
#if MODULE_ADMOB
    public class AdMobHandler : AdvertisingHandler
    {
        private const int RETRY_ATTEMPT_DEFAULT_VALUE = 1;

        private int interstitialRetryAttempt = RETRY_ATTEMPT_DEFAULT_VALUE;
        private int rewardedRetryAttempt = RETRY_ATTEMPT_DEFAULT_VALUE;

        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardedAd rewardBasedVideo;

#if UNITY_EDITOR
        private readonly string ADMOB_GAMEOBJECT_NAME_REGEX = "[0-9]{3,4}x[0-9]{3,4}\\(Clone\\)";
        private Regex regex;
#endif
        private readonly List<string> TEST_DEVICES = new List<string>()
        {
            "9ED87174BA40D80E",
            "03025839C6157A0A",
            "D3C91C4185B0B88C",
            "3D23082D9FB8C8ABA5FB7D57448E5365"
        };

        public AdMobHandler(AdvertisingModules moduleType) : base(moduleType) 
        {
#if UNITY_EDITOR
            regex = new Regex(ADMOB_GAMEOBJECT_NAME_REGEX);
#endif
        }

        public override void Init(AdsData adsSettings)
        {
            this.adsSettings = adsSettings;

            if (adsSettings.SystemLogs)
                Debug.Log("[AdsManager]: AdMob is trying to initialize!");

            MobileAds.SetiOSAppPauseOnBackground(true);

            RequestConfiguration requestConfiguration = new RequestConfiguration()
            {
                TagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified,
                TestDeviceIds = TEST_DEVICES
            };

            MobileAds.SetRequestConfiguration(requestConfiguration);

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(InitCompleteAction);
        }

        private void InitCompleteAction(InitializationStatus initStatus)
        {
            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                isInitialized = true;

                if (AdsManager.OnAdsModuleInitializedEvent != null)
                    AdsManager.OnAdsModuleInitializedEvent.Invoke(ModuleType);

                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: AdMob is initialized!");
            });
        }

        public override void DestroyBanner()
        {
            if (bannerView != null)
                bannerView.Destroy();
        }

        public override void HideBanner()
        {
            if (bannerView != null)
                bannerView.Hide();
        }

        public override void RequestInterstitial()
        {
            if (!isInitialized)
                return;

            if(adsSettings.InterstitialType != AdvertisingModules.AdMob)
            {
                return;
            }

            if (!AdsManager.IsForcedAdEnabled(false))
            {
                return;
            }

            // Clean up interstitial ad before creating a new one.
            if (interstitial != null)
            {
                interstitial.Destroy();
            }

            InterstitialAd.Load(GetInterstitialID(), GetAdRequest(), (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    if (adsSettings.SystemLogs)
                        Debug.Log("[AdsManager]: Interstitial ad failed to load an ad with error: " + error);

                    if (AdsManager.OnInterstitialLoadFailedEvent != null)
                        AdsManager.OnInterstitialLoadFailedEvent.Invoke();

                    interstitialRetryAttempt++;
                    float retryDelay = Mathf.Pow(2, interstitialRetryAttempt);

                    Tween.DelayedCall(interstitialRetryAttempt, () => AdsManager.RequestInterstitial(), true, TweenType.Update);

                    return;
                }

                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: Interstitial ad loaded with response: " + ad.GetResponseInfo());

                interstitial = ad;

                interstitialRetryAttempt = RETRY_ATTEMPT_DEFAULT_VALUE;

                if (AdsManager.OnInterstitialLoadedEvent != null)
                    AdsManager.OnInterstitialLoadedEvent.Invoke();

                // Register for ad events.
                interstitial.OnAdFullScreenContentOpened += HandleInterstitialOpened;
                interstitial.OnAdFullScreenContentClosed += HandleInterstitialClosed;
                interstitial.OnAdClicked += HandleInterstitialClicked;
            });


        }

        public override void RequestRewardedVideo()
        {
            if (!isInitialized)
                return;
            
            if(adsSettings.RewardedVideoType != AdvertisingModules.AdMob)
            {
                return;
            }

            RewardedAd.Load(GetRewardedVideoID(), GetAdRequest(), (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    AdsManager.ExecuteRewardVideoCallback(false);

                    if (adsSettings.SystemLogs)
                        Debug.Log("[AdsManager]: HandleRewardBasedVideoFailedToLoad event received with message: " + error);

                    rewardedRetryAttempt++;
                    float retryDelay = Mathf.Pow(2, rewardedRetryAttempt);

                    Tween.DelayedCall(rewardedRetryAttempt, () => AdsManager.RequestRewardBasedVideo(), true, TweenType.Update);

                    if (AdsManager.OnRewardedAdLoadFailedEvent != null)
                        AdsManager.OnRewardedAdLoadFailedEvent.Invoke();
                    
                    return;
                }

                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: Rewarded ad loaded with response: " + ad.GetResponseInfo());

                rewardedRetryAttempt = RETRY_ATTEMPT_DEFAULT_VALUE;

                if (AdsManager.OnRewardedAdLoadedEvent != null)
                    AdsManager.OnRewardedAdLoadedEvent.Invoke();

                rewardBasedVideo = ad;
                rewardBasedVideo.OnAdFullScreenContentFailed += HandleRewardBasedVideoFailedToShow;
                rewardBasedVideo.OnAdFullScreenContentOpened += HandleRewardBasedVideoOpened;
                rewardBasedVideo.OnAdFullScreenContentClosed += HandleRewardBasedVideoClosed;
                rewardBasedVideo.OnAdClicked += HandleRewardBasedVideoClicked;
            });
        }

        private void RequestBanner()
        {
            if(adsSettings.BannerType != AdvertisingModules.AdMob)
            {
                return;
            }

            if (!AdsManager.IsForcedAdEnabled(false))
            {
                return;
            }

            // Clean up banner before reusing
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            AdSize adSize = AdSize.Banner;

            switch (adsSettings.AdMobSettings.BannerType)
            {
                case AdMobData.BannerPlacementType.Banner:
                    adSize = AdSize.Banner;
                    break;
                case AdMobData.BannerPlacementType.MediumRectangle:
                    adSize = AdSize.MediumRectangle;
                    break;
                case AdMobData.BannerPlacementType.IABBanner:
                    adSize = AdSize.IABBanner;
                    break;
                case AdMobData.BannerPlacementType.Leaderboard:
                    adSize = AdSize.Leaderboard;
                    break;
            }

            AdPosition adPosition = AdPosition.Bottom;
            switch (adsSettings.AdMobSettings.BannerPosition)
            {
                case BannerPosition.Bottom:
                    adPosition = AdPosition.Bottom;
                    break;
                case BannerPosition.Top:
                    adPosition = AdPosition.Top;
                    break;
            }

            bannerView = new BannerView(GetBannerID(), adSize, adPosition);

            // Register for ad events.
            bannerView.OnBannerAdLoaded += HandleAdLoaded;
            bannerView.OnBannerAdLoadFailed += HandleAdFailedToLoad;
            bannerView.OnAdPaid += HandleAdPaid;
            bannerView.OnAdClicked += HandleAdClicked;
            bannerView.OnAdFullScreenContentClosed += HandleAdClosed;

            // Load a banner ad.
            bannerView.LoadAd(GetAdRequest());
        }

        public override void ShowBanner()
        {
            if (!isInitialized)
                return;

            if (bannerView == null)
                RequestBanner();

            if(bannerView != null)
                bannerView.Show();
        }

        public override void ShowInterstitial(InterstitialCallback callback)
        {
            if (!isInitialized)
            {
                if (callback != null)
                    callback.Invoke(false);

                return;
            }

            interstitial.Show();
        }

        public override void ShowRewardedVideo(RewardedVideoCallback callback)
        {
            if (!isInitialized)
                return;

            rewardBasedVideo.Show((GoogleMobileAds.Api.Reward reward) =>
            {
                AdsManager.CallEventInMainThread(delegate
                {
                    AdsManager.ExecuteRewardVideoCallback(true);

                    if (adsSettings.SystemLogs)
                        Debug.Log("[AdsManager]: HandleRewardBasedVideoRewarded event received");

                    if (AdsManager.OnRewardedAdReceivedRewardEvent != null)
                        AdsManager.OnRewardedAdReceivedRewardEvent.Invoke();

                    AdsManager.ResetInterstitialDelayTime();
                    AdsManager.RequestRewardBasedVideo();
                });
            });
        }

        public override bool IsInterstitialLoaded()
        {
            return interstitial != null && interstitial.CanShowAd();
        }

        public override bool IsRewardedVideoLoaded()
        {
            return rewardBasedVideo != null && rewardBasedVideo.CanShowAd();
        }

        public override void SetGDPR(bool state)
        {

        }

        public AdRequest GetAdRequest()
        {
            return new AdRequest()
            {
                Extras = new Dictionary<string, string>()
                {
                    { "npa", AdsManager.GetGDPRState() ? "1" : "0" }
                }
            };
        }

    #region Banner Callbacks
        public void HandleAdLoaded()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleAdLoaded event received");

                if (AdsManager.OnBannerAdLoadedEvent != null)
                    AdsManager.OnBannerAdLoadedEvent.Invoke();
            });
        }

        public void HandleAdFailedToLoad(LoadAdError error)
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleFailedToReceiveAd event received with message: " + error.GetMessage());

                if (AdsManager.OnBannerAdLoadFailedEvent != null)
                    AdsManager.OnBannerAdLoadFailedEvent.Invoke();
            });
        }

        private void HandleAdPaid(AdValue adValue)
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleAdPaid event received");
            });
        }

        public void HandleAdClicked()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleAdClicked event received");

                if (AdsManager.OnBannerAdClickedEvent != null)
                    AdsManager.OnBannerAdClickedEvent.Invoke();
            });
        }

        public void HandleAdClosed()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleAdClosed event received");
            });
        }
    #endregion

    #region Interstitial Callback
        public void HandleInterstitialOpened()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleInterstitialOpened event received");

                if (AdsManager.OnInterstitialDisplayedEvent != null)
                    AdsManager.OnInterstitialDisplayedEvent.Invoke();
            });
        }

        public void HandleInterstitialClosed()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleInterstitialClosed event received");

                if (AdsManager.OnInterstitialHiddenEvent != null)
                    AdsManager.OnInterstitialHiddenEvent.Invoke();

                AdsManager.ExecuteInterstitialCallback(true);

                AdsManager.ResetInterstitialDelayTime();
                AdsManager.RequestInterstitial();
            });
        }

        private void HandleInterstitialClicked()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleInterstitialClicked event received");

                if (AdsManager.OnInterstitialClickedEvent != null)
                    AdsManager.OnInterstitialClickedEvent.Invoke();
            });
        }
    #endregion

    #region RewardedVideo Callback
        private void HandleRewardBasedVideoFailedToShow(AdError error)
        {
            AdsManager.CallEventInMainThread(delegate
            {
                AdsManager.ExecuteRewardVideoCallback(false);

                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleRewardBasedVideoFailedToShow event received with message: " + error);

                rewardedRetryAttempt++;
                float retryDelay = Mathf.Pow(2, rewardedRetryAttempt);

                Tween.DelayedCall(rewardedRetryAttempt, () => AdsManager.RequestRewardBasedVideo(), true, TweenType.Update);

                if (AdsManager.OnRewardedAdLoadFailedEvent != null)
                    AdsManager.OnRewardedAdLoadFailedEvent.Invoke();
            });
        }

        public void HandleRewardBasedVideoOpened()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleRewardBasedVideoOpened event received");

#if UNITY_EDITOR
                //fix that helps display ads over store
                UnityEngine.Object[] canvases = GameObject.FindObjectsOfType(typeof(Canvas),false);

                for (int i = 0; i < canvases.Length; i++)
                {
                    if (regex.IsMatch(canvases[i].name))
                    {
                        ((Canvas)canvases[i]).sortingOrder = 9999;
                        break;
                    }
                }
#endif

                if (AdsManager.OnRewardedAdDisplayedEvent != null)
                    AdsManager.OnRewardedAdDisplayedEvent.Invoke();
            });
        }

        public void HandleRewardBasedVideoClosed()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleRewardBasedVideoClosed event received");

                if (AdsManager.OnRewardedAdHiddenEvent != null)
                    AdsManager.OnRewardedAdHiddenEvent.Invoke();
            });
        }

        private void HandleRewardBasedVideoClicked()
        {
            AdsManager.CallEventInMainThread(delegate
            {
                if (adsSettings.SystemLogs)
                    Debug.Log("[AdsManager]: HandleRewardBasedVideoClicked event received");

                if (AdsManager.OnRewardedAdClickedEvent != null)
                    AdsManager.OnRewardedAdClickedEvent.Invoke();
            });
        }
        #endregion

        public string GetBannerID()
        {
#if UNITY_EDITOR
            return "unused";
#elif UNITY_ANDROID
            return adsSettings.AdMobSettings.AndroidBannerID;
#elif UNITY_IOS
            return adsSettings.AdMobSettings.IOSBannerID;
#else
            return "unexpected_platform";
#endif
        }

        public string GetInterstitialID()
        {
#if UNITY_EDITOR
            return "unused";
#elif UNITY_ANDROID
            return adsSettings.AdMobSettings.AndroidInterstitialID;
#elif UNITY_IOS
            return adsSettings.AdMobSettings.IOSInterstitialID;
#else
            return "unexpected_platform";
#endif
        }

        public string GetRewardedVideoID()
        {
#if UNITY_EDITOR
            return "unused";
#elif UNITY_ANDROID
            return adsSettings.AdMobSettings.AndroidRewardedVideoID;
#elif UNITY_IOS
            return adsSettings.AdMobSettings.IOSRewardedVideoID;
#else
            return "unexpected_platform";
#endif
        }
    }
#endif
}

// -----------------
// Advertisement v 1.3
// -----------------