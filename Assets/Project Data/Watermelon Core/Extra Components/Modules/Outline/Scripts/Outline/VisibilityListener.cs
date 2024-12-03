using System;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.Outline
{
    [ExecuteAlways]
    public class VisibilityListener : MonoBehaviour
    {
        private List<Callback> callbacks = new List<Callback>();

        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        private void OnBecameVisible()
        {
            InvokeCallbacks();
        }

        private void OnBecameInvisible()
        {
            InvokeCallbacks();
        }

        public void InvokeCallbacks()
        {
            for(int i = 0; i < callbacks.Count; i++)
            {
                var callback = callbacks[i];

                if(callback.Target == null)
                {
                    callbacks.RemoveAt(i);
                    i--;
                    continue;
                }

                callback.Action?.Invoke();
            }
        }

        public void AddCallback(Outlinable outlinable, Action action, bool invoke = true)
        {
            var callback = new Callback(outlinable, action);
            callbacks.Add(callback);

            callback.Action?.Invoke();
        }

        public void RemoveCallback(Outlinable outlinable, Action callback)
        {
            for(int i = 0; i < callbacks.Count; i++)
            {
                var test = callbacks[i];

                if(test.Target == outlinable && test.Action == callback)
                {
                    callbacks.RemoveAt(i);
                    break;
                }
            }
        }

        private struct Callback
        {
            public Outlinable Target { get; private set; }
            public Action Action { get; private set; }

            public Callback(Outlinable target, Action action)
            {
                Target = target;
                Action = action;
            }
        }
    }
}