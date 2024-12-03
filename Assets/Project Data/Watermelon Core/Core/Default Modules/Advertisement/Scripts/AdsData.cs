#pragma warning disable 0414

using UnityEngine;

namespace Watermelon
{
    [SetupTab("Advertising", texture = "icon_ads")]
    [CreateAssetMenu(fileName = "Ads Settings", menuName = "Settings/Ads Settings")]
    [HelpURL("https://docs.google.com/document/d/1JKw1XgdvJhdilZ7vC3HkzSvP861Q2eVbrL4udVKW9cc")]
    public class AdsData : ScriptableObject
    {
        [SerializeField] AdvertisingModules bannerType = AdvertisingModules.Dummy;
        public AdvertisingModules BannerType => bannerType;

        [SerializeField] AdvertisingModules interstitialType = AdvertisingModules.Dummy;
        public AdvertisingModules InterstitialType => interstitialType;

        [SerializeField] AdvertisingModules rewardedVideoType = AdvertisingModules.Dummy;
        public AdvertisingModules RewardedVideoType => rewardedVideoType;

        // Providers
        [SerializeField] AdMobData adMobContainer;
        public AdMobData AdMobSettings => adMobContainer;

        [SerializeField] UnityAdsLegacyContainer unityAdsContainer;
        public UnityAdsLegacyContainer UnityAdsSettings => unityAdsContainer;

        // Dummy
        [SerializeField] DummyContainer dummyContainer;
        public DummyContainer DummySettings => dummyContainer;

        [Group("Settings")]
        [Tooltip("Enables development mode to setup advertisement providers.")]
        [SerializeField] bool testMode = false;
        public bool TestMode => testMode;

        [Group("Settings")]
        [Tooltip("Enables logging. Use it to debug advertisement logic.")]
        [SerializeField] bool systemLogs = false;
        public bool SystemLogs => systemLogs;

        [Group("Settings")]
        [Tooltip("Delay in seconds before first interstitial appearings.")]
        [SerializeField] float insterstitialFirstDelay = 40f;
        public float InsterstitialFirstDelay => interstitialShowingDelay;

        [Group("Settings")]
        [Tooltip("Delay in seconds between interstitial appearings.")]
        [SerializeField] float interstitialShowingDelay = 30f;
        public float InterstitialShowingDelay => interstitialShowingDelay;

        [Group("Privacy")]
        [SerializeField] bool isGDPREnabled = false;
        public bool IsGDPREnabled => isGDPREnabled;

        [Group("Privacy")]
        [SerializeField] bool isIDFAEnabled = false;
        public bool IsIDFAEnabled => isIDFAEnabled;

        [Group("Privacy")]
        [SerializeField] string trackingDescription = "Your data will be used to deliver personalized ads to you.";
        public string TrackingDescription => trackingDescription;

        [Group("Privacy")]
        [SerializeField] string privacyLink = "https://mywebsite.com/privacy";
        public string PrivacyLink => privacyLink;

        [Group("Privacy")]
        [SerializeField] string termsOfUseLink = "https://mywebsite.com/terms";
        public string TermsOfUseLink => termsOfUseLink;

        public bool IsDummyEnabled()
        {
            if (bannerType == AdvertisingModules.Dummy)
                return true;

            if (interstitialType == AdvertisingModules.Dummy)
                return true;

            if (rewardedVideoType == AdvertisingModules.Dummy)
                return true;

            return false;
        }
    }

    public enum BannerPosition
    {
        Bottom = 0,
        Top = 1,
    }
}

// -----------------
// Advertisement v 1.3
// -----------------