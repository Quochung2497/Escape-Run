using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{

    [System.Serializable]
    public abstract class ToggleType<T>
    {
        public bool enabled;
        public T newValue;

        public T Handle(T value)
        {
            if (enabled)
            {
                return newValue;
            }
            else
            {
                return value;
            }
        }
    }


    [System.Serializable]
    public class BoolToggle : ToggleType<bool> { }

    [System.Serializable]
    public class FloatToggle : ToggleType<float> { }

    [System.Serializable]
    public class IntToggle : ToggleType<int> { }

    [System.Serializable]
    public class LongToggle : ToggleType<long> { }

    [System.Serializable]
    public class StringToggle : ToggleType<string> { }

    [System.Serializable]
    public class DoubleToggle : ToggleType<double> { }
}