using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class JuicyBounce
    {
        [SerializeField] float bounceDuration;
        [SerializeField] float maxBounceDepth = 2f;

        [Space]
        [SerializeField] AnimationCurve bounceScaleX;
        [SerializeField] AnimationCurve bounceScaleY;
        [SerializeField] AnimationCurve bounceScaleZ;

        private float bounceValue;
        private float bounceDepth;

        private TweenCase bounceCase;

        private Transform transform;

        public void Initialise(Transform transform)
        {
            this.transform = transform;

            bounceValue = 0;
            bounceDepth = 1;
        }

        public void Bounce()
        {
            if (bounceCase != null && bounceCase.isActive)
            {
                bounceCase.Kill();

                bounceDepth += 0.05f;

                if (bounceDepth > maxBounceDepth) bounceDepth = maxBounceDepth;
            }

            bounceCase = new TweenCaseJuicyBounce(this, GetStartBounceTime(), 1).SetTime(bounceDuration).OnComplete(delegate
            {
                bounceValue = 0;
                bounceDepth = 1;
            }).StartTween();
        }

        private void BounceTween(float t)
        {
            bounceValue = t;

            transform.localScale = new Vector3(Mathf.Pow(bounceScaleX.Evaluate(t), bounceDepth), Mathf.Pow(bounceScaleY.Evaluate(t), bounceDepth), Mathf.Pow(bounceScaleZ.Evaluate(t), bounceDepth));
        }

        private float GetStartBounceTime(int depth = 0)
        {
            var target = bounceScaleY.Evaluate(bounceValue);

            var step = depth == 0 ? 0.01f : 0.01f / (10 * depth);

            var error = 0.01f;

            for (var pointer = 0f; pointer <= 1f; pointer += step)
            {
                if (Mathf.Abs(bounceScaleY.Evaluate(pointer) - target) < error)
                {
                    return pointer;
                }
            }

            return GetStartBounceTime(depth + 1);
        }

        public void Clear()
        {
            if (bounceCase != null)
            {
                bounceCase.Kill();

                bounceValue = 0;
                bounceDepth = 1;

                bounceCase = null;
            }
        }

        public class TweenCaseJuicyBounce : TweenCase
        {
            private float startValue;
            private float resultValue;

            private JuicyBounce juicyBounce;

            public TweenCaseJuicyBounce(JuicyBounce juicyBounce, float startValue, float resultValue)
            {
                this.startValue = startValue;
                this.resultValue = resultValue;

                this.juicyBounce = juicyBounce;

                // Get parent object
                parentObject = juicyBounce.transform.gameObject;
            }

            public override bool Validate()
            {
                return parentObject != null;
            }

            public override void DefaultComplete()
            {
                juicyBounce.BounceTween(resultValue);
            }

            public override void Invoke(float deltaTime)
            {
                juicyBounce.BounceTween(startValue + (resultValue - startValue) * Interpolate(state));
            }
        }
    }
}