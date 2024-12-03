using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class SimpleFloatSave : ISaveObject
    {
        [SerializeField] float value;
        public float Value { get => value; set => this.value = value; }

        public virtual void Flush() { }
    }
}