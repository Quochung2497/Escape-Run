using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class SimplePool<T> where T: new()
    {
        protected Queue<T> available = new Queue<T>();

        protected List<T> everything = new List<T>();

        public virtual T GetPooledObject()
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

        public virtual T CreateNewObject() 
        {
            return new T();
        }

        public virtual void ReturnEverythingToPool()
        {
            available.Clear();
            foreach (var obj in everything)
                available.Enqueue(obj);
        }
    }
}