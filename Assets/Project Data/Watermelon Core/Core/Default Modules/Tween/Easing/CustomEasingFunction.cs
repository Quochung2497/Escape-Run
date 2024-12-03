using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class CustomEasingFunction : Ease.IEasingFunction
    {
        [SerializeField] string name;
        public string Name => name;

        [SerializeField] AnimationCurve easingCurve;

        private float totalEasingTime;

        public void Initialise()
        {
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