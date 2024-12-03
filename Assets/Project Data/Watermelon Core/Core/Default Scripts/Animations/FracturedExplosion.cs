using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class FracturedExplosion
    {
        [SerializeField] GameObject solidObject;
        [SerializeField] GameObject fracturedObject;

        [Space]
        [SerializeField] float force = 500;
        [SerializeField] float scaleDuration = 0.3f;
        [SerializeField] float scaleDelay = 0.3f;
        [SerializeField] Ease.Type scaleEasing = Ease.Type.QuadOut;

        private List<FractureData> fractures = new List<FractureData>();

        private TweenCase explodeTweenCase;
        private bool isExploded;

        public void Initialise()
        {
            Transform parent = fracturedObject.transform;
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                var fracture = new FractureData(parent.GetChild(i));

                fractures.Add(fracture);
            }

            solidObject.SetActive(true);
            fracturedObject.SetActive(false);

            isExploded = false;
        }

        public void Restore()
        {
            for(int i = 0; i < fractures.Count; i++)
            {
                fractures[i].Restore();
            }

            solidObject.SetActive(true);
            fracturedObject.SetActive(false);

            explodeTweenCase.KillActive();

            isExploded = false;
        }

        public void Explode(Tween.TweenCallback onExploded = null)
        {
            Explode(fracturedObject.transform.position, onExploded);
        }

        public void Explode(Vector3 epicenterPosition, Tween.TweenCallback onExploded = null)
        {
            if (isExploded) return;

            isExploded = true;

            solidObject.SetActive(false);
            fracturedObject.SetActive(true);

            for (int i = 0; i < fractures.Count; i++)
            {
                fractures[i].Explode(epicenterPosition, force, scaleDuration, scaleDelay, scaleEasing);
            }

            if(onExploded != null)
            {
                explodeTweenCase = Tween.DelayedCall(scaleDuration + scaleDelay, onExploded);
            }
        }

        private class FractureData
        {
            public Transform parent;

            public Rigidbody rb;
            public Vector3 localPos;
            public Vector3 localRot;

            public FractureData(Transform transform)
            {
                parent = transform.parent;

                rb = transform.GetComponent<Rigidbody>();
                localPos = transform.localPosition;
                localRot = transform.localEulerAngles;
            }

            public void Explode(Vector3 epicenter, float force, float scaleTime, float scaleDelay, Ease.Type scaleEasing)
            {
                rb.transform.SetParent(null);
                rb.AddForce((rb.transform.position - epicenter).normalized * force);

                rb.transform.DOScale(0, scaleTime, scaleDelay).SetEasing(scaleEasing);
            }

            public void Restore()
            {
                rb.velocity = Vector3.zero;
                rb.transform.SetParent(parent);
                rb.transform.localPosition = localPos;
                rb.transform.localEulerAngles = localRot;

                rb.transform.localScale = Vector3.one;
            }
        }
    }
}