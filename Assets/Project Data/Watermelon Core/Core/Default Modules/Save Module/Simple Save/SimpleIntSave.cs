using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class SimpleIntSave : ISaveObject
    {
        [SerializeField] int value;
        public virtual int Value
        {
            get => value; set
            {
                this.value = value;
            }
        }

        public virtual void Flush() { }
    }
}