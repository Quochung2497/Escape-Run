using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Image icon;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private LayoutElement layoutElement;

        public string Text { get => text.text; set => text.text = value; }
        public Sprite Icon { get => icon.sprite; set => icon.sprite = value; }

        public bool IsVisible { get => gameObject.activeSelf; }

        private TweenCase fadeTweenCase;
        private TweenCase disableTweenCase;

        private Currency currency;
        public Currency Currency => currency;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            layoutElement = GetComponent<LayoutElement>();
        }

        public void Initialise(Currency currency)
        {
            this.currency = currency;

            canvasGroup.alpha = 0.0f;
            layoutElement.preferredHeight = CurrencyUIHelper.PANEL_HEIGHT;

            icon.sprite = currency.Icon;

            Redraw();
        }

        public void Redraw()
        {
            text.text = currency.AmountFormatted;

            rectTransform.sizeDelta = CurrencyUIHelper.GetPanelSize(text.text.Length - 1);
        }

        public void SetAmount(int amount)
        {
            text.text = CurrenciesHelper.Format(amount);

            rectTransform.sizeDelta = CurrencyUIHelper.GetPanelSize(text.text.Length - 1);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (fadeTweenCase != null && fadeTweenCase.isActive) fadeTweenCase.Kill();

            fadeTweenCase = canvasGroup.DOFade(1, 0.3f);
        }

        public void ShowImmediately()
        {
            if (fadeTweenCase != null && fadeTweenCase.isActive) fadeTweenCase.Kill();

            gameObject.SetActive(true);

            canvasGroup.alpha = 1.0f;
        }

        public void Hide()
        {
            if (fadeTweenCase != null && fadeTweenCase.isActive) fadeTweenCase.Kill();

            fadeTweenCase = canvasGroup.DOFade(0, 0.3f).OnComplete(() => {
                fadeTweenCase = layoutElement.DOPreferredHeight(0, 0.3f).SetEasing(Ease.Type.QuadOut).OnComplete(delegate
                {
                    gameObject.SetActive(false);
                });
            });
        }

        public void Clear()
        {
            if (fadeTweenCase != null && fadeTweenCase.isActive) fadeTweenCase.Kill();
        }

        public void DisableAfter(float seconds, Tween.TweenCallback onDisable)
        {
            disableTweenCase = Tween.DelayedCall(seconds, delegate
            {
                Hide();
            }).OnComplete(onDisable);
        }

        public void ResetDisable()
        {
            if (disableTweenCase != null && !disableTweenCase.isCompleted)
                disableTweenCase.state = 0.0f;
        }

        public void KillDisable()
        {
            if (disableTweenCase != null && !disableTweenCase.isCompleted)
            {
                disableTweenCase.Kill();
                disableTweenCase = null;
            }
        }
    }
}