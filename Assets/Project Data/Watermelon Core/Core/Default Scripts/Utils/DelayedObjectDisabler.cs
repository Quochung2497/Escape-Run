using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class DelayedObjectDisabler : MonoBehaviour
    {
        public float delay;

        private void OnEnable()
        {
            StartCoroutine(DelayedCall());
        }

        private IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(false);
        }
    }
}