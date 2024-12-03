using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class SimpleBounce
    {
        [SerializeField] float bounceDuration;

        [Space]
        [SerializeField] AnimationCurve bounceScaleX;
        [SerializeField] AnimationCurve bounceScaleY;
        [SerializeField] AnimationCurve bounceScaleZ;

        private TweenCase bounceCase;
        private Transform transform;

        public void Initialise(Transform transform)
        {
            this.transform = transform;
        }

        public void Bounce(Tween.TweenCallback onComplete = null)
        {
            if (bounceCase != null && bounceCase.isActive)
                bounceCase.Kill();

            bounceCase = new TweenCaseSimpleBounce(this, 0, 1).SetTime(bounceDuration).OnComplete(onComplete).StartTween();
        }

        private void BounceTween(float t)
        {
            transform.localScale = new Vector3(bounceScaleX.Evaluate(t), bounceScaleY.Evaluate(t), bounceScaleZ.Evaluate(t));
        }

        public void Clear()
        {
            if (bounceCase != null)
            {
                bounceCase.Kill();
                bounceCase = null;
            }
        }

        public class TweenCaseSimpleBounce : TweenCase
        {
            private float startValue;
            private float resultValue;

            private SimpleBounce simpleBounce;

            public TweenCaseSimpleBounce(SimpleBounce simpleBounce, float startValue, float resultValue)
            {
                this.startValue = startValue;
                this.resultValue = resultValue;

                this.simpleBounce = simpleBounce;

                // Get parent object
                parentObject = simpleBounce.transform.gameObject;
            }

            public override bool Validate()
            {
                return parentObject != null;
            }

            public override void DefaultComplete()
            {
                simpleBounce.BounceTween(resultValue);
            }

            public override void Invoke(float deltaTime)
            {
                simpleBounce.BounceTween(startValue + (resultValue - startValue) * Interpolate(state));
            }
        }
    }
}