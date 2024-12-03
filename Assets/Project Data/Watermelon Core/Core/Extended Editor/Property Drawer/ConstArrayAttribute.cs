using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ConstArrayAttribute : PropertyAttribute
    {
        public string[] labelValues;

        public ConstArrayAttribute(params string[] labelValues)
        {
            this.labelValues = labelValues;
        }
    }
}