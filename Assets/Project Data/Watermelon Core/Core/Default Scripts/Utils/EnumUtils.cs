using System;
using Random = UnityEngine.Random;

namespace Watermelon
{
    public static class EnumUtils
    {
        /// <summary>
        /// Get random enum value
        /// </summary>
        public static T GetRandomEnum<T>() where T : struct, IConvertible
        {
#if UNITY_EDITOR
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
#endif

            Array allValues = Enum.GetValues(typeof(T));
            T randomValue = (T)allValues.GetValue(Random.Range(0, allValues.Length));
            return randomValue;
        }

        /// <summary>
        /// Get first enum value
        /// </summary>
        public static T GetDefaultEnum<T>() where T : struct, IConvertible
        {
#if UNITY_EDITOR
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
#endif

            return (T)Enum.GetValues(typeof(T)).GetValue(0);
        }
    }
}