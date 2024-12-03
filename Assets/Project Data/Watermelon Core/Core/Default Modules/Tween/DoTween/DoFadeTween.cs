using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DoFadeTween : DoTween<CanvasGroup, float>
    {
        protected override float TargetValue
        {
            get => TargetComponent.alpha;
            set => TargetComponent.alpha = value;
        }

        protected override void StartLoop(float delay)
        {
            TargetValue = startValue;
            tweenCase = TargetComponent.DOFade(endValue, duration);

            base.StartLoop(delay);
        }

        protected override void IncrementLoopChangeValues()
        {
            var difference = endValue - startValue;
            startValue = endValue;
            endValue += difference;
        }
    }
}
