using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class UIScalableObject
    {
        [SerializeField] RectTransform rect;

        public RectTransform RectTransform => rect;

        private TweenCase scaleTweenCase;

        public void Show(bool immediately = true, float scaleMultiplier = 1.1f, float duration = 0.5f, SimpleCallback onCompleted = null)
        {
            scaleTweenCase.KillActive();

            if (immediately)
            {
                rect.localScale = Vector3.one;
                onCompleted?.Invoke();
                return;
            }

            // RESET
            rect.localScale = Vector3.zero;
            scaleTweenCase = rect.DOPushScale(Vector3.one * scaleMultiplier, Vector3.one, duration * 0.64f, duration * 0.36f, Ease.Type.CubicOut, Ease.Type.CubicIn).OnComplete(() =>
            {
                onCompleted?.Invoke();
            });
        }

        public void Hide(bool immediately = true, float scaleMultiplier = 1.1f, float duration = 0.5f, SimpleCallback onCompleted = null)
        {
            scaleTweenCase.KillActive();

            if (immediately)
            {
                rect.localScale = Vector3.zero;
                onCompleted?.Invoke();

                return;
            }

            scaleTweenCase = rect.DOPushScale(Vector3.one * scaleMultiplier, Vector3.zero, duration * 0.36f, duration * 0.64f, Ease.Type.CubicOut, Ease.Type.CubicIn).OnComplete(() =>
            {
                onCompleted?.Invoke();
            });
        }
    }
}