#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class AdsManagerExampleScript : MonoBehaviour
    {
        private Vector2 scrollView;

        [SerializeField]
        private Text logText;

        [Space]
        [SerializeField]
        private Text bannerTitleText;
        [SerializeField]
        private Button[] bannerButtons;

        [Space]
        [SerializeField]
        private Text interstitialTitleText;
        [SerializeField]
        private Button[] interstitialButtons;

        [Space]
        [SerializeField]
        private Text rewardVideoTitleText;
        [SerializeField]
        private Button[] rewardVideoButtons;

        private AdsData settings;

        private void Awake()
        {
            Application.logMessageReceived += Log;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= Log;
        }

        private void Start()
        {
            settings = AdsManager.Settings;

            logText.text = string.Empty;

            bannerTitleText.text = string.Format("Banner ({0})", settings.BannerType.ToString());
            if(settings.BannerType == AdvertisingModules.Disable)
            {
                for(int i = 0; i < bannerButtons.Length; i++)
                {
                    bannerButtons[i].interactable = false; 
                }
            }

            interstitialTitleText.text = string.Format("Interstitial ({0})", settings.InterstitialType.ToString());
            if (settings.InterstitialType == AdvertisingModules.Disable)
            {
                for (int i = 0; i < interstitialButtons.Length; i++)
                {
                    interstitialButtons[i].interactable = false;
                }
            }

            rewardVideoTitleText.text = string.Format("Rewarded Video ({0})", settings.RewardedVideoType.ToString());
            if (settings.RewardedVideoType == AdvertisingModules.Disable)
            {
                for (int i = 0; i < rewardVideoButtons.Length; i++)
                {
                    rewardVideoButtons[i].interactable = false;
                }
            }

            GameLoading.MarkAsReadyToHide();
        }

        private void Log(string condition, string stackTrace, LogType type)
        {
            logText.text = logText.text.Insert(0, condition + "\n");
        }

        private void Log(string condition)
        {
            logText.text = logText.text.Insert(0, condition + "\n");
        }

        #region Buttons
        public void ShowBannerButton()
        {
            AdsManager.ShowBanner();
        }

        public void HideBannerButton()
        {
            AdsManager.HideBanner();
        }

        public void DestroyBannerButton()
        {
            AdsManager.DestroyBanner();
        }

        public void InterstitialStatusButton()
        {
            Log("[AdsManager]: Interstitial " + (AdsManager.IsInterstitialLoaded() ? "is loaded" : "isn't loaded"));
        }

        public void RequestInterstitialButton()
        {
            AdsManager.RequestInterstitial();
        }

        public void ShowInterstitialButton()
        {
            AdsManager.ShowInterstitial( (isDisplayed) =>
            {
                Debug.Log("[AdsManager]: Interstitial " + (isDisplayed ? "is" : "isn't") + " displayed!");
            });
        }

        public void RewardedVideoStatusButton()
        {
            Log("[AdsManager]: Rewarded video " + (AdsManager.IsRewardBasedVideoLoaded() ? "is loaded" : "isn't loaded"));
        }

        public void RequestRewardedVideoButton()
        {
            AdsManager.RequestRewardBasedVideo();
        }

        public void ShowRewardedVideoButton()
        {
            AdsManager.ShowRewardBasedVideo( (hasReward) =>
            {
                if(hasReward)
                {
                    Log("[AdsManager]: Reward is received");
                }
                else
                {
                    Log("[AdsManager]: Reward isn't received");
                }
            });
        }
        #endregion
    }
}

// -----------------
// Advertisement v 1.3
// -----------------