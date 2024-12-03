using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

namespace Watermelon
{
    [System.Serializable]
    public class DuoInt
    {
        public int firstValue;
        public int secondValue;

        public DuoInt(int firstValue, int secondValue)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
        }

        public DuoInt(int value)
        {
            this.firstValue = value;
            this.secondValue = value;
        }

        public static DuoInt One => new DuoInt(1);
        public static DuoInt Zero => new DuoInt(0);

        public int Random()
        {
            return UnityEngine.Random.Range(firstValue, secondValue + 1); // Because second parameter is exclusive. Withot + 1 method Random.Range(1,2) will always return 1
        }

        public int Clamp(int value)
        {
            return Mathf.Clamp(value, firstValue, secondValue);
        }

        public int Lerp(float t)
        {
            return Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp(firstValue, secondValue, t)), firstValue, secondValue);
        }

        public static implicit operator Vector2Int(DuoInt value) => new Vector2Int(value.firstValue, value.secondValue);
        public static explicit operator DuoInt(Vector2Int vec) => new DuoInt(vec.x, vec.y);

        public static implicit operator int2(DuoInt value) => new int2(value.firstValue, value.secondValue);
        public static explicit operator DuoInt(int2 value) => new DuoInt(value.x, value.y);

        public static DuoInt operator *(DuoInt a, DuoInt b) => new DuoInt(a.firstValue * b.firstValue, a.secondValue * b.secondValue);
        public static DuoInt operator /(DuoInt a, DuoInt b)
        {
            if ((b.firstValue == 0) || (b.secondValue == 0))
            {
                throw new System.DivideByZeroException();
            }

            return new DuoInt(a.firstValue / b.firstValue, a.secondValue / b.secondValue);
        }

        public static DuoInt operator *(DuoInt a, float b) => new DuoInt((int)(a.firstValue * b), (int)(a.secondValue * b));
        public static DuoInt operator /(DuoInt a, float b)
        {
            if (b == 0)
            {
                throw new System.DivideByZeroException();
            }

            return new DuoInt((int)(a.firstValue / b), (int)(a.secondValue / b));
        }

        public DuoFloat ToDuoFloat()
        {
            return new DuoFloat(firstValue, secondValue);
        }

        public override string ToString()
        {
            return "(" + firstValue + ", " + secondValue + ")";
        }
    }

    [System.Serializable]
    public class DuoFloat
    {
        public float firstValue;
        public float secondValue;

        public float this[int i] => i == 0 ? firstValue : secondValue;

        public DuoFloat(float firstValue, float secondValue)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
        }
        public DuoFloat(float value)
        {
            this.firstValue = value;
            this.secondValue = value;
        }

        public static DuoFloat One => new DuoFloat(1);
        public static DuoFloat Zero => new DuoFloat(0);
        public static DuoFloat ZeroOne => new DuoFloat(0, 1);
        public static DuoFloat OneZero => new DuoFloat(1, 0);
        public static DuoFloat MinusOneOne => new DuoFloat(-1, 1);

        public static DuoFloat operator -(DuoFloat value) => new DuoFloat(-value.x, -value.y);

        public float Random()
        {
            return UnityEngine.Random.Range(firstValue, secondValue);
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(firstValue, secondValue, t);
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, firstValue, secondValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(float value, DuoFloat inMinMax, DuoFloat outMinMax)
        {
            return outMinMax.firstValue + (value - inMinMax.firstValue) * (outMinMax.secondValue - outMinMax.firstValue) / (inMinMax.secondValue - inMinMax.firstValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DuoFloat Remap(DuoFloat value, DuoFloat inMin, DuoFloat inMax, DuoFloat outMin, DuoFloat outMax)
        {
            return new DuoFloat(
                Remap(value.x, new DuoFloat(inMin.x, inMax.x), new DuoFloat(outMin.x, outMax.x)),
                Remap(value.y, new DuoFloat(inMin.y, inMax.y), new DuoFloat(outMin.y, outMax.y))
            );
        }

        public static implicit operator Vector2(DuoFloat value) => value.xy;

        public static implicit operator DuoFloat(Vector2 vec) => new DuoFloat(vec.x, vec.y);
        public static implicit operator DuoFloat(float value) => new DuoFloat(value);
        public static implicit operator DuoFloat(int value) => new DuoFloat(value);

        public static DuoFloat operator *(DuoFloat a, DuoFloat b) => new DuoFloat(a.firstValue * b.firstValue, a.secondValue * b.secondValue);
        public static DuoFloat operator /(DuoFloat a, DuoFloat b)
        {
            if ((b.firstValue == 0) || (b.secondValue == 0))
            {
                throw new System.DivideByZeroException();
            }

            return new DuoFloat(a.firstValue / b.firstValue, a.secondValue / b.secondValue);
        }

        public static DuoFloat operator *(DuoFloat a, float b) => new DuoFloat(a.firstValue * b, a.secondValue * b);
        public static DuoFloat operator /(DuoFloat a, float b)
        {
            if (b == 0)
            {
                throw new System.DivideByZeroException();
            }

            return new DuoFloat(a.firstValue / b, a.secondValue / b);
        }

        public override string ToString()
        {
            return "(" + FormatValue(firstValue) + ", " + FormatValue(secondValue) + ")";
        }

        private string FormatValue(float value)
        {
            return value.ToString("0.0").Replace(',', '.');
        }

        public float Max => firstValue > secondValue ? firstValue : secondValue;
        public float Min => firstValue < secondValue ? firstValue : secondValue;

        public float x => firstValue;
        public float y => secondValue;

        public Vector2 xy => new Vector2(firstValue, secondValue);
        public Vector2 yx => new Vector2(secondValue, firstValue);
        public Vector3 x0y => new Vector3(firstValue, 0, secondValue);
        public Vector3 y0x => new Vector3(secondValue, 0, firstValue);

        public float r => UnityEngine.Random.Range(firstValue, secondValue);
        public Vector2 rr => new Vector2(r, r);
        public Vector3 rrr => new Vector3(r, r, r);
        public Vector3 r0r => new Vector3(r, 0, r);

        public static DuoFloat XZ(Vector3 value) => new DuoFloat(value.x, value.z);
    }

    [System.Serializable]
    public class DuoDouble
    {
        public double firstValue;
        public double secondValue;
        private static System.Random random;

        public DuoDouble(double firstValue, double secondValue)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
        }

        public DuoDouble(double value)
        {
            this.firstValue = value;
            this.secondValue = value;
        }

        public static DuoDouble One => new DuoDouble(1);
        public static DuoDouble Zero => new DuoDouble(0);

        public double Random()
        {
            if (random == null)
            {
                random = new System.Random();
            }

            return random.NextDouble() * (this.secondValue - this.firstValue) + this.firstValue;
        }

        public double Clamp(double value)
        {
            if (value < firstValue)
            {
                return firstValue;
            }
            else if (value > secondValue)
            {
                return secondValue;
            }
            else
            {
                return value;
            }
        }

        public static DuoDouble operator *(DuoDouble a, DuoDouble b) => new DuoDouble(a.firstValue * b.firstValue, a.secondValue * b.secondValue);
        public static DuoDouble operator /(DuoDouble a, DuoDouble b)
        {
            if ((b.firstValue == 0) || (b.secondValue == 0))
            {
                throw new System.DivideByZeroException();
            }

            return new DuoDouble(a.firstValue / b.firstValue, a.secondValue / b.secondValue);
        }

        public override string ToString()
        {
            return "(" + FormatValue(firstValue) + ", " + FormatValue(secondValue) + ")";
        }

        private string FormatValue(double value)
        {
            return value.ToString("0.0").Replace(',', '.');
        }
    }

    [System.Serializable]
    public class DuoVector3
    {
        public Vector3 firstValue;
        public Vector3 secondValue;

        public DuoVector3(Vector3 firstValue, Vector3 secondValue)
        {
            this.firstValue = firstValue;
            this.secondValue = secondValue;
        }

        public DuoVector3(Vector3 value)
        {
            this.firstValue = value;
            this.secondValue = value;
        }

        public static DuoVector3 One => new DuoVector3(Vector3.one);
        public static DuoVector3 Zero => new DuoVector3(Vector3.zero);

        public static DuoVector3 operator *(DuoVector3 a, DuoVector3 b) => new DuoVector3(new Vector3(a.firstValue.x * b.firstValue.x, a.firstValue.y * b.firstValue.y, a.firstValue.z * b.firstValue.z), new Vector3(a.secondValue.x * b.secondValue.x, a.secondValue.y * b.secondValue.y, a.secondValue.z * b.secondValue.z));
        public static DuoVector3 operator /(DuoVector3 a, DuoVector3 b)
        {
            if ((b.firstValue.x == 0) || (b.firstValue.y == 0) || (b.firstValue.z == 0) || (b.secondValue.x == 0) || (b.secondValue.y == 0) || (b.secondValue.z == 0))
            {
                throw new System.DivideByZeroException();
            }

            return new DuoVector3(new Vector3(a.firstValue.x / b.firstValue.x, a.firstValue.y / b.firstValue.y, a.firstValue.z / b.firstValue.z), new Vector3(a.secondValue.x / b.secondValue.x, a.secondValue.y / b.secondValue.y, a.secondValue.z / b.secondValue.z));
        }

        public Vector3 Random()
        {
            return new Vector3(UnityEngine.Random.Range(firstValue.x, secondValue.x), UnityEngine.Random.Range(firstValue.y, secondValue.y), UnityEngine.Random.Range(firstValue.z, secondValue.z));
        }

        public override string ToString()
        {
            return "[" + FormatValue(firstValue) + ", " + FormatValue(secondValue) + "]";
        }

        private string FormatValue(float value)
        {
            return value.ToString("0.0").Replace(',', '.');
        }

        private string FormatValue(Vector3 value)
        {
            return "(" + FormatValue(value.x) + ", " + FormatValue(value.y) + ", " + FormatValue(value.z) + ")";
        }
    }

    [System.Serializable]
    public class DuoColor
    {
        public Color32 firstValue;
        public Color32 secondValue;

        public DuoColor(Color32 first, Color32 second)
        {
            firstValue = first;
            secondValue = second;
        }

        public DuoColor(Color32 color)
        {
            firstValue = color;
            secondValue = color;
        }

        public Color32 RandomBetween()
        {
            return Color32.Lerp(firstValue, secondValue, UnityEngine.Random.value);
        }
    }
}