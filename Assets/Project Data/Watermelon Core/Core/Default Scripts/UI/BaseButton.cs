using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Watermelon
{
    [RequireComponent(typeof(Graphic)), RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField, OnValueChanged("OnInteractableChange")]
        private bool interactable = true;

        [Space]
        public Color normalColor;
        public Color disabledColor;

        [Space]
        public Graphic graphic;
        public CanvasGroup canvasGroup;

        [Space]
        public AnimationType animationType;
        public Animator animator;
        public string pointerDownTrigger = "PointerDown";
        public string pointerUpTrigger = "PointerUp";

        private TweenCase tween;
        private bool isClickCanceled = false;
        private int pointerDownTriggerParameter;
        private int pointerUpTriggerParameter;
        private int lastPointerDownObjectHash;

        public enum AnimationType
        {
            Tween = 0,
            Animator = 1,
        }

        public bool Interactable
        {
            get { return interactable; }
            set
            {
                interactable = value;

                if (value)
                {
                    graphic.color = normalColor;
                    canvasGroup.alpha = 1;
                }
                else
                {
                    graphic.color = disabledColor;
                    canvasGroup.alpha = disabledColor.a;
                }
            }
        }

        public bool EnableAnimator
        {
            get { return animator.enabled; }
            set
            {
                Debug.Log("Enable property: " + value);
                animator.enabled = value;
            }
        }

        public virtual void Awake()
        {
            if (animationType == AnimationType.Animator)
            {
                pointerDownTriggerParameter = Animator.StringToHash(pointerDownTrigger);
                pointerUpTriggerParameter = Animator.StringToHash(pointerUpTrigger);
            }
        }


        public void OnInteractableChange()
        {
            Interactable = interactable;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isClickCanceled = !(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetHashCode() == lastPointerDownObjectHash);

            OnClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            lastPointerDownObjectHash = eventData.pointerPressRaycast.gameObject.GetHashCode();
            PointerDownAnimation();
        }

        private void PointerDownAnimation()
        {
            if (tween != null && !tween.isCompleted)
                tween.Complete();

            if (interactable)
            {
                if (animationType == AnimationType.Tween)
                {
                    tween = graphic.transform.DOScale(0.9f, 0.04f, unscaledTime: true).SetEasing(Ease.Type.QuadOut);
                }
                else
                {
                    animator.SetTrigger(pointerDownTriggerParameter);
                }
            }
            else
            {
                //tween = graphic.transform.DOScale(0.96f, 0.075f, true).SetEasing(Ease.Type.QuadOut);
            }
        }

        public virtual void OnClick(/*bool isClickCanceled,*/ Tween.TweenCallback callback = null)
        {
            if (tween != null && !tween.isCompleted)
                tween.Complete();

            if (interactable)
            {
#if MODULE_AUDIO_NATIVE
            NativeAudioHandler.PlayButtonPressSound();
#endif

                if (animationType == AnimationType.Tween)
                {
                    tween = graphic.transform.DOScale(1f, 0.04f, unscaledTime: true).SetEasing(Ease.Type.QuadIn).OnComplete(!isClickCanceled ? callback : null);
                }
                else
                {
                    animator.SetTrigger(pointerUpTriggerParameter);
                    Tween.DelayedCall(0.075f, !isClickCanceled ? callback : null);
                }
            }
            else
            {
                //if (disabledSound != null)
                //    AudioController.PlaySound(disabledSound, AudioController.AudioType.Sound);
                //NativeAudioHandler.PlayButtonPressSound();

                //tween = graphic.transform.DOScale(1f, 0.075f, true).SetEasing(Ease.Type.QuadIn);
            }
        }
    }
}