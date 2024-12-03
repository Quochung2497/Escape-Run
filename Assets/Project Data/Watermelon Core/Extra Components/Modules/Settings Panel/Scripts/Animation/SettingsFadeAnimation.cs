#pragma warning disable 0649 

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Settings Fade Animation", menuName = "Content/Settings Animation/Fade")]
    public class SettingsFadeAnimation : SettingsAnimation
    {
        [SerializeField] float initialDelay;
        [SerializeField] float elementDelay;
        [SerializeField] float elementFadeDuration;
        [SerializeField] Ease.Type showEasing = Ease.Type.BackOut;
        [SerializeField] Ease.Type hideEasing = Ease.Type.BackIn;

        private CanvasGroup[] canvasGroups;

        protected override void AddExtraComponents()
        {
            canvasGroups = new CanvasGroup[settingsButtonsInfo.Length];
            for (int i = 0; i < settingsButtonsInfo.Length; i++)
            {
                canvasGroups[i] = settingsButtonsInfo[i].SettingsButton.gameObject.GetOrSetComponent<CanvasGroup>();
                canvasGroups[i].alpha = 0;
            }
        }

        public override IEnumerator Show(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            TweenCase lastTweenCase = null;
            for (int i = 0; i < settingsButtonsInfo.Length; i++)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                settingsButtonsInfo[i].SettingsButton.RectTransform.anchoredPosition = settingsPanel.ButtonPositions[i];
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(true);

                canvasGroups[i].alpha = 0;
                lastTweenCase = canvasGroups[i].DOFade(1, elementFadeDuration).SetEasing(showEasing);

                yield return new WaitForSeconds(elementDelay);
            }

            if (lastTweenCase != null)
            {
                while (!lastTweenCase.isCompleted)
                {
                    yield return null;
                }

                callback.Invoke();
            }
            else
            {
                callback.Invoke();
            }
        }

        public override IEnumerator Hide(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            TweenCase lastTweenCase = null;
            for (int i = settingsButtonsInfo.Length - 1; i >= 0; i--)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                int index = i;
                lastTweenCase = canvasGroups[i].DOFade(0, elementFadeDuration).SetEasing(hideEasing).OnComplete(delegate
                {
                    settingsButtonsInfo[index].SettingsButton.gameObject.SetActive(false);
                });

                yield return new WaitForSeconds(elementDelay);
            }

            if (lastTweenCase != null)
            {
                while (!lastTweenCase.isCompleted)
                {
                    yield return null;
                }

                callback.Invoke();
            }
            else
            {
                callback.Invoke();
            }
        }
    }
}

// -----------------
// Settings Panel v 0.3
// -----------------