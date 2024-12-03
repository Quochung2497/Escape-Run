#pragma warning disable 0649 

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Settings Position Animation", menuName = "Content/Settings Animation/Position")]
    public class SettingsPositionAnimation : SettingsAnimation
    {
        [SerializeField] float initialDelay;
        [SerializeField] float elementDelay;
        [SerializeField] float elementMovementDuration;
        [SerializeField] float offsetPosition;
        [SerializeField] Ease.Type showEasing = Ease.Type.BackOut;
        [SerializeField] Ease.Type hideEasing = Ease.Type.BackIn;

        public override IEnumerator Show(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            TweenCase lastTweenCase = null;
            for (int i = 0; i < settingsButtonsInfo.Length; i++)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                settingsButtonsInfo[i].SettingsButton.RectTransform.anchoredPosition = settingsPanel.ButtonPositions[i].AddToX(offsetPosition);
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(true);

                lastTweenCase = settingsButtonsInfo[i].SettingsButton.RectTransform.DOAnchoredPosition(settingsPanel.ButtonPositions[i], elementMovementDuration).SetEasing(showEasing);

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
                lastTweenCase = settingsButtonsInfo[i].SettingsButton.RectTransform.DOAnchoredPosition(settingsPanel.ButtonPositions[i].AddToX(offsetPosition), elementMovementDuration).SetEasing(hideEasing).OnComplete(delegate
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