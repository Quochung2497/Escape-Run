using UnityEngine;

namespace Watermelon
{
    public class AnimationCurveEasingFunction : Ease.IEasingFunction
    {
        private AnimationCurve easingCurve;
        private float totalEasingTime;

        public AnimationCurveEasingFunction(AnimationCurve easingCurve)
        {
            this.easingCurve = easingCurve;

            totalEasingTime = easingCurve.keys[easingCurve.keys.Length - 1].time;
        }

        public float Interpolate(float p)
        {
            return easingCurve.Evaluate(p * totalEasingTime);
        }
    }
}


// -----------------
// Tween v 1.3.1
// -----------------