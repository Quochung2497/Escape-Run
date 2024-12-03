using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Watermelon
{
    public class FloatingMessage : MonoBehaviour
    {
        private static FloatingMessage floatingMessage;

        [SerializeField] RectTransform windowRectTransform;
        [SerializeField] CanvasGroup windowCanvasGroup;
        [SerializeField] TextMeshProUGUI floatingText;
        [SerializeField] Image panelImage;

        [Space]
        [SerializeField] Vector2 panelPadding = new Vector2(30, 25);

        private TweenCase animationTweenCase;

        public void Init()
        {
            floatingMessage = this;

            // Init clickable panel
            EventTrigger trigger = floatingText.gameObject.AddComponent<EventTrigger>();

            // Create new event entry
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnPanelClick(); });

            // Add entry to trigger
            trigger.triggers.Add(entry);
        }

        private void OnPanelClick()
        {
            if (floatingMessage.animationTweenCase != null && !floatingMessage.animationTweenCase.isCompleted)
                floatingMessage.animationTweenCase.Kill();

            floatingMessage.animationTweenCase = floatingMessage.windowCanvasGroup.DOFade(0, 0.3f, unscaledTime: true).SetEasing(Ease.Type.CircOut).OnComplete(delegate
            {
                floatingMessage.windowRectTransform.gameObject.SetActive(false);
            });
        }

        public static void ShowMessage(string message, float timeMultiplier = 1.0f)
        {
            if(floatingMessage != null)
            {
                if (floatingMessage.animationTweenCase != null && !floatingMessage.animationTweenCase.isCompleted)
                    floatingMessage.animationTweenCase.Kill();

                floatingMessage.floatingText.text = message;

                Tween.NextFrame(delegate
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(floatingMessage.floatingText.rectTransform);

                    floatingMessage.panelImage.rectTransform.anchoredPosition = floatingMessage.floatingText.rectTransform.anchoredPosition;
                    floatingMessage.panelImage.rectTransform.sizeDelta = floatingMessage.floatingText.rectTransform.sizeDelta + floatingMessage.panelPadding;
                });

                floatingMessage.windowRectTransform.gameObject.SetActive(true);
                floatingMessage.windowRectTransform.localScale = Vector2.one;

                floatingMessage.windowCanvasGroup.alpha = 1.0f;
                floatingMessage.animationTweenCase = Tween.DelayedCall(1.5f * timeMultiplier, delegate
                {
                    floatingMessage.animationTweenCase = floatingMessage.windowCanvasGroup.DOFade(0, 0.5f * timeMultiplier, unscaledTime: true).SetEasing(Ease.Type.CircOut).OnComplete(delegate
                    {
                        floatingMessage.windowRectTransform.gameObject.SetActive(false);
                    });
                }, unscaledTime: true);
            }
            else
            {
                Debug.Log("[Floating Message]: " + message);
                Debug.LogError("[Floating Message]: ShowMessage() method has called, but module isn't initialized! Add Floating Message module to Project Init Settings.");
            }
        }
    }
}

// -----------------
// Floating Message v 0.1
// -----------------

// Changelog
// v 0.1
// • Added basic version