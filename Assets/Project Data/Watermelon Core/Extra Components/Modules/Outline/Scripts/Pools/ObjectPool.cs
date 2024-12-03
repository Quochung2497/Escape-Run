using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class ObjectPool<K> : SimplePool<K> where K : Object, new()
    {
        // Don't ask me why, with meshes compiler for some reason needs to know that it creates Object in order not to destroy it in some cases
        public override K GetPooledObject()
        {
            while (available.Count > 0 && available.Peek() == null)
                available.Dequeue();

            if (available.Count == 0)
            {
                var obj = CreateNewObject();

                everything.Add(obj);
                available.Enqueue(obj);
            }

            return available.Dequeue();
        }
    }
}