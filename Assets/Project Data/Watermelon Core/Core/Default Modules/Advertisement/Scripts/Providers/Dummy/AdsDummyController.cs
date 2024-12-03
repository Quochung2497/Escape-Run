#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class AdsDummyController : MonoBehaviour
    {
        [SerializeField]
        private GameObject bannerObject;

        [Space]
        [SerializeField]
        private GameObject interstitialObject;

        [Space]
        [SerializeField]
        private GameObject rewardedVideoObject;

        private RectTransform bannerRectTransform;

        private void Awake()
        {
            bannerRectTransform = (RectTransform)bannerObject.transform;

#if UNITY_EDITOR
            UnityEditor.SceneVisibilityManager.instance.ToggleVisibility(bannerObject, true);
            UnityEditor.SceneVisibilityManager.instance.TogglePicking(bannerObject, true);
#endif

            DontDestroyOnLoad(gameObject);
        }

        public void Init(AdsData settings)
        {
            switch (settings.DummySettings.bannerPosition)
            {
                case BannerPosition.Bottom:
                    bannerRectTransform.pivot = new Vector2(0.5f, 0.0f);

                    bannerRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                    bannerRectTransform.anchorMax = new Vector2(1.0f, 0.0f);

                    bannerRectTransform.anchoredPosition = Vector2.zero;
                    break;
                case BannerPosition.Top:
                    bannerRectTransform.pivot = new Vector2(0.5f, 1.0f);

                    bannerRectTransform.anchorMin = new Vector2(0.0f, 1.0f);
                    bannerRectTransform.anchorMax = new Vector2(1.0f, 1.0f);

                    bannerRectTransform.anchoredPosition = Vector2.zero;
                    break;
            }
        }

        public void ShowBanner()
        {
            bannerObject.SetActive(true);
        }

        public void HideBanner()
        {
            bannerObject.SetActive(false);
        }

        public void ShowInterstitial()
        {
            interstitialObject.SetActive(true);
        }

        public void CloseInterstitial()
        {
            interstitialObject.SetActive(false);

            if (AdsManager.OnInterstitialHiddenEvent != null)
                AdsManager.OnInterstitialHiddenEvent.Invoke();
        }

        public void ShowRewardedVideo()
        {
            rewardedVideoObject.SetActive(true);
        }

        public void CloseRewardedVideo()
        {
            rewardedVideoObject.SetActive(false);

            if (AdsManager.OnRewardedAdHiddenEvent != null)
                AdsManager.OnRewardedAdHiddenEvent.Invoke();
        }

        #region Buttons
        public void CloseInterstitialButton()
        {
            AdsManager.ExecuteInterstitialCallback(true);

            CloseInterstitial();
        }

        public void CloseRewardedVideoButton()
        {
            AdsManager.ExecuteRewardVideoCallback(false);

            CloseRewardedVideo();
        }

        public void GetRewardButton()
        {
            AdsManager.ExecuteRewardVideoCallback(true);

            CloseRewardedVideo();
        }
        #endregion
    }

    [System.Serializable]
    public class DummyContainer
    {
        public BannerPosition bannerPosition = BannerPosition.Bottom;
    }
}

// -----------------
// Advertisement v 1.3
// -----------------