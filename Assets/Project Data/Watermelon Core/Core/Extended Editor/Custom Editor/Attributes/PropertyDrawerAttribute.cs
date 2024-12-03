using System;

namespace Watermelon
{
    public class PropertyDrawerAttribute : BaseAttribute
    {
        public PropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
