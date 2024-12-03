using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class UnityAdsLegacyContainer
    {
        //Application ID
        [Header("Application ID")]
        [SerializeField] string androidAppID = "1234567";
        public string AndroidAppID => androidAppID;
        [SerializeField] string iOSAppID = "1234567";
        public string IOSAppID => iOSAppID;

        //Banned ID
        [Header("Banner ID")]
        [SerializeField] string androidBannerID = "banner";
        public string AndroidBannerID => androidBannerID;
        [SerializeField] string iOSBannerID = "banner";
        public string IOSBannerID => iOSBannerID;

        //Interstitial ID
        [Header("Interstitial ID")]
        [SerializeField] string androidInterstitialID = "video";
        public string AndroidInterstitialID => androidInterstitialID;
        [SerializeField] string iOSInterstitialID = "video";
        public string IOSInterstitialID => iOSInterstitialID;

        //Rewarder Video ID
        [Header("Rewarded Video ID")]
        [SerializeField] string androidRewardedVideoID = "rewardedVideo";
        public string AndroidRewardedVideoID => androidRewardedVideoID;
        [SerializeField] string iOSRewardedVideoID = "rewardedVideo";
        public string IOSRewardedVideoID => iOSRewardedVideoID;

        [Space]
        [SerializeField] BannerPlacement bannerPosition = BannerPlacement.BOTTOM_CENTER;
        public BannerPlacement BannerPosition => bannerPosition;

        public enum BannerPlacement
        {
            TOP_LEFT = 0,
            TOP_CENTER = 1,
            TOP_RIGHT = 2,
            BOTTOM_LEFT = 3,
            BOTTOM_CENTER = 4,
            BOTTOM_RIGHT = 5,
            CENTER = 6
        }
    }
}
