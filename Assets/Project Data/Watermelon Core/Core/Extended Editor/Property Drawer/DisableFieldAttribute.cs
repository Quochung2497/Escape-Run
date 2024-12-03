using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DisableFieldAttribute : PropertyAttribute
    {
        public DisableFieldAttribute()
        {

        }
    }
}
