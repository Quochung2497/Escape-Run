#pragma warning disable 0618
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class FloatingTextUpgradesBehaviour : FloatingTextBaseBehaviour
    {
        [SerializeField] Transform containerTransform;
        [SerializeField] CanvasGroup containerCanvasGroup;
        [SerializeField] Image iconImage;
        [SerializeField] Text floatingText;

        [Space]
        [SerializeField] Vector3 offset;
        [SerializeField] float time;
        [SerializeField] Ease.Type easing;

        [Space]
        [SerializeField] float scaleTime;
        [SerializeField] AnimationCurve scaleAnimationCurve;

        [Space]
        [SerializeField] float fadeTime;
        [SerializeField] Ease.Type fadeEasing;

        private Transform targetTransform;
        private Vector3 targetOffset;
        private bool fixToTarget;

        private void LateUpdate()
        {
            if (fixToTarget)
                transform.position = targetTransform.position + targetOffset;
        }

        public void SetIconAndColor(Sprite icon, Color color)
        {
            iconImage.sprite = icon;
            iconImage.color = color;

            floatingText.color = color;
        }

        public override void Activate(string text, float scale = 1.0f)
        {
            fixToTarget = false;

            floatingText.text = text;

            containerCanvasGroup.alpha = 1.0f;

            containerTransform.localScale = Vector3.zero;
            containerTransform.DOScale(Vector3.one * scale, scaleTime).EnableCustomEasing(scaleAnimationCurve);

            containerCanvasGroup.DOFade(0.0f, fadeTime).SetEasing(fadeEasing);

            containerTransform.localPosition = Vector3.zero;
            containerTransform.DOLocalMove(offset, time).SetEasing(easing).OnComplete(delegate
            {
                gameObject.SetActive(false);
                transform.SetParent(null);
            });
        }

        public void FixToTarget(Transform target, Vector3 offset)
        {
            fixToTarget = true;

            targetOffset = offset;
            targetTransform = target;
        }
    }
}