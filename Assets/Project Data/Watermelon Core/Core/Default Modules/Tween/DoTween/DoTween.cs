using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public abstract class DoTween<C, T> : MonoBehaviour where C : Component
    {
        [SerializeField] protected float duration;
        [SerializeField] protected float delay;

        [Header("Easing")]
        [SerializeField] protected Ease.Type easing;

        [Space]
        [SerializeField] protected bool hasCustomCurve;
        [SerializeField] protected AnimationCurve customCurve;

        [Header("Loop")]
        [SerializeField] protected int loopAmount = 1;
        [SerializeField] protected LoopType loopType;

        [Header("Target")]

        [Space]
        [SerializeField] protected TweenTargetType type;
        [SerializeField] protected T target;

        private C targetComponent;
        public C TargetComponent
        {
            get
            {
                if (targetComponent != null) return targetComponent;

                targetComponent = GetComponent<C>();

                return targetComponent;
            }
        }

        protected int loopId;

        protected TweenCase tweenCase;

        protected T initialValue;
        protected T startValue;
        protected T endValue;

        protected abstract T TargetValue { get; set; }

        protected virtual void Awake()
        {
            initialValue = TargetValue;
        }

        protected virtual void OnEnable()
        {
            if (type == TweenTargetType.From)
            {
                startValue = target;
                endValue = initialValue;
            }
            else
            {
                endValue = target;
                startValue = initialValue;
            }

            loopId = 0;

            StartLoop(delay);
        }

        protected virtual void StartLoop(float delay)
        {
            loopId++;

            tweenCase.SetDelay(delay);

            if (hasCustomCurve)
            {
                tweenCase.SetCurveEasing(customCurve);
            }
            else
            {
                tweenCase.SetEasing(easing);
            }

            if (loopId != loopAmount)
            {
                tweenCase.OnComplete(OnComplete);
            }
        }

        protected virtual void OnComplete()
        {
            switch (loopType)
            {
                case LoopType.Yoyo:
                    var clone = startValue;
                    startValue = endValue;
                    endValue = clone;
                    break;
                case LoopType.Increment:
                    IncrementLoopChangeValues();
                    break;
            }

            StartLoop(0);
        }

        protected abstract void IncrementLoopChangeValues();

        protected virtual void OnDisable()
        {
            tweenCase.KillActive();
        }

        public enum LoopType
        {
            Repeat = 0,
            Yoyo = 1,
            Increment = 2,
        }

        public enum TweenTargetType
        {
            From = 0,
            To = 1
        }
    }
}
