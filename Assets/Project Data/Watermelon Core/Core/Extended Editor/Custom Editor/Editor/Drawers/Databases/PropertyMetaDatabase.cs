using System;
using System.Collections.Generic;

namespace Watermelon
{
    public static class PropertyMetaDatabase
    {
        private static Dictionary<Type, PropertyMeta> metasByAttributeType;

        static PropertyMetaDatabase()
        {
            metasByAttributeType = new Dictionary<Type, PropertyMeta>();
            metasByAttributeType[typeof(InfoBoxAttribute)] = new InfoBoxPropertyMeta();
            metasByAttributeType[typeof(OnValueChangedAttribute)] = new OnValueChangedPropertyMeta();
        }

        public static PropertyMeta GetMetaForAttribute(Type attributeType)
        {
            PropertyMeta meta;
            if (metasByAttributeType.TryGetValue(attributeType, out meta))
            {
                return meta;
            }
            else
            {
                return null;
            }
        }
    }
}
