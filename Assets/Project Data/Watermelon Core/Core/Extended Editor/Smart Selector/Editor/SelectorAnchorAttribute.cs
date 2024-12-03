using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SelectorAnchorAttribute : Attribute
    {
        public SelectorAnchorAttribute()
        {
        }
    }
}
