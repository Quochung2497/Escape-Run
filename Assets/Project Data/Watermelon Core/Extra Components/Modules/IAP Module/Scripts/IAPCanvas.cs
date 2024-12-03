using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class IAPCanvas : MonoBehaviour
    {
        private static IAPCanvas iapCanvas;

        [Header("Message")]
        [SerializeField] GameObject messagePanel;
        [SerializeField] TextMeshProUGUI messageText;

        [Header("Loading")]
        [SerializeField] GameObject loadingContainer;
        [SerializeField] TextMeshProUGUI errorMessageText;

        [Space]
        [SerializeField] Animator circleAnimation;

        private TweenCase messageScaleTweenCase;
        private TweenCase loadingScaleTweenCase;

        public void Init()
        {
            iapCanvas = this;
        }

        public static void ShowMessage(string message)
        {
            if (iapCanvas.messageScaleTweenCase != null && !iapCanvas.messageScaleTweenCase.isCompleted)
                iapCanvas.messageScaleTweenCase.Kill();

            iapCanvas.messagePanel.SetActive(true);
            iapCanvas.messageScaleTweenCase = Tween.DelayedCall(1.5f, delegate
            {
                iapCanvas.messagePanel.SetActive(false);
            }, unscaledTime: true);

            iapCanvas.messageText.text = message;
        }

        public static void ShowLoadingPanel()
        {
            if (iapCanvas.loadingScaleTweenCase != null && !iapCanvas.loadingScaleTweenCase.isCompleted)
                iapCanvas.loadingScaleTweenCase.Kill();

            iapCanvas.circleAnimation.enabled = true;

            iapCanvas.loadingContainer.SetActive(true);
        }

        public static void ChangeLoadingMessage(string message)
        {
            iapCanvas.errorMessageText.text = message;
        }

        public static void HideLoadingPanel()
        {
            if (iapCanvas.loadingScaleTweenCase != null && !iapCanvas.loadingScaleTweenCase.isCompleted)
                iapCanvas.loadingScaleTweenCase.Kill();

            iapCanvas.circleAnimation.enabled = false;
            iapCanvas.loadingContainer.SetActive(false);
        }
    }
}
