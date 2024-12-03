using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class FloatingTextHitBehaviour : FloatingTextBaseBehaviour
    {
        [SerializeField] TextMeshProUGUI floatingText;

        [Space]
        [SerializeField] float delay;
        [SerializeField] float disableDelay;
        [SerializeField] float startScale;
        [SerializeField] float time;
        [SerializeField] Ease.Type easing;

        [Space]
        [SerializeField] float scaleTime;
        [SerializeField] Ease.Type scaleEasing;

        private Vector3 defaultScale;

        private void Awake()
        {
            defaultScale = transform.localScale;
        }

        public override void Activate(string text, float scale = 1.0f)
        {
            floatingText.text = text;

            int sign = Random.value >= 0.5f ? 1 : -1;

            transform.localScale = defaultScale * startScale * scale;
            transform.localRotation = Quaternion.Euler(70, 0, 18 * sign);

            Tween.DelayedCall(delay, delegate
            {
                transform.DOLocalRotate(Quaternion.Euler(70, 0, 0), time).SetEasing(easing).OnComplete(delegate
                {
                    Tween.DelayedCall(disableDelay, delegate
                    {
                        gameObject.SetActive(false);
                    });
                });

                transform.DOScale(defaultScale * scale, scaleTime).SetEasing(scaleEasing);
            });
        }
    }
}