using System.Collections.Generic;

namespace Watermelon
{
    public class TweenCaseCollection
    {
        private List<TweenCase> tweenCases = new List<TweenCase>();
        public List<TweenCase> TweenCases => tweenCases;

        private Tween.TweenCallback onCompleteCallback;

        private int completedTweensCount = 0;
        private int tweensCount = 0;

        public void AddTween(TweenCase tweenCase)
        {
            tweenCase.OnComplete(OnTweenCaseComplete);

            tweenCases.Add(tweenCase);
            tweensCount++;
        }

        public bool IsComplete()
        {
            for(int i = 0; i < tweensCount; i++)
            {
                if (!tweenCases[i].isCompleted)
                    return false;
            }

            return true;
        }

        public void Complete()
        {
            for (int i = 0; i < tweensCount; i++)
            {
                tweenCases[i].Complete();
            }
        }

        public void Kill()
        {
            for (int i = 0; i < tweensCount; i++)
            {
                tweenCases[i].Kill();
            }
        }

        public void OnComplete(Tween.TweenCallback callback)
        {
            onCompleteCallback += callback;
        }

        private void OnTweenCaseComplete()
        {
            completedTweensCount++;

            if (completedTweensCount == tweensCount)
            {
                if (onCompleteCallback != null)
                    onCompleteCallback.Invoke();
            }
        }

        public static TweenCaseCollection operator +(TweenCaseCollection caseCollection, TweenCase tweenCase)
        {
            caseCollection.AddTween(tweenCase);

            return caseCollection;
        }
    }
}
