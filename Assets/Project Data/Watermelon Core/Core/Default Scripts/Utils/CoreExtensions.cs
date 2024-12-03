using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Watermelon
{
    public static class CoreExtensions
    {
        #region Int
        /// <summary>
        /// Set random sign
        /// </summary>
        public static int SetRandomSign(this int value)
        {
            return value * Random.value < 0.5f ? 1 : -1;
        }
        #endregion

        #region Float
        /// <summary>
        /// Set random sign
        /// </summary>
        public static float SetRandomSign(this float value)
        {
            return value * Random.value < 0.5f ? 1 : -1;
        }
        #endregion

        #region Double
        /// <summary>
        /// Set random sign
        /// </summary>
        public static double SetRandomSign(this double value)
        {
            return value * Random.value < 0.5f ? 1 : -1;
        }
        #endregion

        #region String
        /// <summary>
        /// Add space before capital letters
        /// </summary>
        public static string AddSpaces(this string value)
        {
            return Regex.Replace(value, "([a-z]) ?([A-Z])", "$1 $2");
        }

        /// <summary>
        /// Get value inside [] brackets
        /// </summary>
        public static string FindStringInsideBrackets(this string value)
        {
            Match match = Regex.Match(value, @"\[([^)]*)\]");

            return match.Result("$1");
        }

        /// <summary>
        /// Try to convert string to enum
        /// </summary>
        public static T ToEnum<T>(this string value, bool ignoreCase, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            T result;

            try
            {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                result = defaultValue;
            }

            return result;
        }
        #endregion

        #region Array & List
        /// <summary>
        /// Get array with unique ids of another array
        /// </summary>
        public static int[] GetUniqueRandomObjectIDs<T>(T[] array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Length)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjectIDs if count is 1!");
#endif

            List<int> objectIDs = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Length);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);
            }

            return objectIDs.ToArray();
        }

        /// <summary>
        /// Get unique random objects
        /// </summary>
        public static T[] GetUniqueRandomObjects<T>(T[] array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Length)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjects if count is 1!");
#endif

            List<int> objectIDs = new List<int>();
            List<T> resultList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Length);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);

                resultList.Add(array[randomValue]);
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// Get array with unique ids of another list
        /// </summary>
        public static int[] GetUniqueRandomObjectIDs<T>(List<int> array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Count)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjectIDs if count is 1!");
#endif

            List<int> objectIDs = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Count);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);
            }

            return objectIDs.ToArray();
        }

        /// <summary>
        /// Get unique random objects
        /// </summary>
        public static T[] GetUniqueRandomObjects<T>(List<T> array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Count)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjects if count is 1!");
#endif

            List<int> objectIDs = new List<int>();
            List<T> resultList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Count);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);

                resultList.Add(array[randomValue]);
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// Check if index is inside array range
        /// </summary>
        public static bool IsInRange<T>(this T[] array, int value)
        {
            return (value >= 0 && value < array.Length);
        }

        /// <summary>
        /// Check if index is inside list range
        /// </summary>
        public static bool IsInRange<T>(this List<T> list, int value)
        {
            return (value >= 0 && value < list.Count);
        }

        /// <summary>
        /// Check if array is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return (array == null || array.Length == 0);
        }

        public static int Sum<T>(this T[] array, Func<T, int> sumFun)
        {
            int sum = 0;

            for(int i = 0; i < array.Length; i++)
            {
                sum += sumFun(array[i]);
            }

            return sum;
        }

        public static float Sum<T>(this T[] array, Func<T, float> sumFun)
        {
            float sum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                sum += sumFun(array[i]);
            }

            return sum;
        }

        public static int Sum<T>(this List<T> list, Func<T, int> sumFun)
        {
            int sum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                sum += sumFun(list[i]);
            }

            return sum;
        }

        public static float Sum<T>(this List<T> list, Func<T, float> sumFun)
        {
            float sum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                sum += sumFun(list[i]);
            }

            return sum;
        }

        public static bool Has<T>(this T[] array, Func<T, bool> searchFun)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (searchFun(array[i])) return true;
            }

            return false;
        }

        public static bool Has<T>(this List<T> list, Func<T, bool> searchFun)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (searchFun(list[i])) return true;
            }

            return false;
        }

        public static List<T> MakeCopy<T>(this List<T> list)
        {
            var copy = new List<T>();
            for(int i = 0; i < list.Count; i++)
            {
                copy.Add(list[i]);
            }
            return copy;
        }

        public static T[] DropLast<T>(this T[] array, int count)
        {
            if (count >= array.Length) return new T[0];

            T[] result = new T[array.Length - count];

            for (int i = 0; i < result.Length; i++) {
                result[i] = array[i];
            }

            return result;
        }

        public static T[] Sort<T, K>(this T[] array, Func<T, K> sortFun) where K: IComparable
        {
            var result = new T[array.Length];

            Array.Copy(array, result, array.Length);

            Array.Sort(result, (t1, t2) => sortFun(t1).CompareTo(sortFun(t2)));

            return result;
        }

        public static T[] SortRevese<T, K>(this T[] array, Func<T, K> sortFun) where K : IComparable
        {
            var result = new T[array.Length];

            Array.Copy(array, result, array.Length);

            Array.Sort(result, (t1, t2) => -sortFun(t1).CompareTo(sortFun(t2)));

            return result;
        }

        public static List<T> Remove<T>(this T[] array, T itemToRemove)
        {
            var result = new List<T>();

            for(int i = 0; i < array.Length; i++)
            {
                if(!array[i].Equals(itemToRemove))
                {
                    result.Add(array[i]);
                }
            }

            return result;
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            for(int i = 0; i < array.Length; i++)
            {
                action(array[i]);
            }
        }

        public static T Find<T>(this T[] array, Func<T, bool> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if(action(array[i])) return array[i];
            }

            return default(T);
        }

        public static int IndexOf<T>(this T[] array, T t)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(t)) return i;
            }

            return -1;
        }

        /// <summary>
        /// Check if list is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (list == null || list.Count == 0);
        }

        /// <summary>
        /// Display array to debug console
        /// </summary>
        public static void Display<T>(this T[] array, Func<T, string> function)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log(function(array[i]));
            }
        }

        /// <summary>
        /// Display list to debug console
        /// </summary>
        public static void Display<T>(this List<T> array, Func<T, string> function)
        {
            for (int i = 0; i < array.Count; i++)
            {
                Debug.Log(function(array[i]));
            }
        }

        /// <summary>
        /// Get random item from array
        /// </summary>
        public static T GetRandomItem<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Get random item from list
        /// </summary>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Get item from array by index or random value if index higher than array size
        /// </summary>
        public static T GetByIndexOrRandom<T>(this T[] array, int index)
        {
            if (array.IsInRange(index))
            {
                return array[index];
            }

            return array.GetRandomItem();
        }

        /// <summary>
        /// Get item from list by index or random value if index higher than list size
        /// </summary>
        public static T GetByIndexOrRandom<T>(this List<T> list, int index)
        {
            if (list.IsInRange(index))
            {
                return list[index];
            }

            return list.GetRandomItem();
        }

        /// <summary>
        /// Randomize array elements
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            int k;
            T temp;

            while (n > 1)
            {
                k = Random.Range(0, n--);
                temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        /// <summary>
        /// Randomize list elements
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            int k;
            T temp;

            while (n > 1)
            {
                k = Random.Range(0, n--);
                temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        public static T FindRandomOrder<T>(this List<T> list, Func<T, bool> action) 
        {
            var tabsIndices = new List<int>(list.Count);
            for (int i = 0; i < list.Count; i++) tabsIndices.Add(i);

            while (tabsIndices.Count > 0)
            {
                var i = Random.Range(0, tabsIndices.Count);

                var index = tabsIndices[i];
                tabsIndices.RemoveAt(i);

                if (action(list[index]))
                {
                    return list[index];
                }
            }

            return default(T);
        }

        public static T FindRandomOrder<T>(this T[] array, Func<T, bool> action)
        {
            var tabsIndices = new List<int>(array.Length);
            for (int i = 0; i < array.Length; i++) tabsIndices.Add(i);

            while (tabsIndices.Count > 0)
            {
                var i = Random.Range(0, tabsIndices.Count);

                var index = tabsIndices[i];
                tabsIndices.RemoveAt(i);

                if (action(array[index]))
                {
                    return array[index];
                }
            }

            return default(T);
        }

        /// <summary>
        /// Check if arrays are equal
        /// </summary>
        public static bool ArraysEqual<T>(this T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if lists are equal
        /// </summary>
        public static bool ArraysEqual<T>(this List<T> a1, List<T> a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Count != a2.Count)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Count; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Crop array to length
        /// </summary>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Check if index is last
        /// </summary>
        public static bool IsLast<T>(this T[] array, int index)
        {
            return array.Length - 1 == index;
        }

        /// <summary>
        /// Check if index is last
        /// </summary>
        public static bool IsLast<T>(this List<T> list, int index)
        {
            return list.Count - 1 == index;
        }

        public static List<T> Filter<T>(this T[] array, Func<T, bool> func)
        {
            var resultList = new List<T>();

            for(int i = 0; i < array.Length; i++)
            {
                if (func(array[i])) resultList.Add(array[i]);
            }

            return resultList;
        }

        public static T Last<T>(this List<T> list)
        {
            return list[^1];
        }

        public static T Last<T>(this T[] array)
        {
            return array[^1];
        }
        #endregion

        #region GameObject
        /// <summary>
        /// Get component if it exists or add new one
        /// </summary>
        public static T GetOrSetComponent<T>(this GameObject gameObject) where T : Component
        {
            Component component = gameObject.GetComponent(typeof(T));

            if (component != null)
                return (T)component;

            return (T)gameObject.AddComponent(typeof(T));
        }
        #endregion

        #region Transform
        /// <summary>
        /// Flip object x scale
        /// </summary>
        public static void FlipX(this Transform transform, bool flip)
        {
            transform.localScale = new Vector3(flip ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        /// <summary>
        /// Flip object y scale
        /// </summary>
        public static void FlipY(this Transform transform, bool flip)
        {
            transform.localScale = new Vector3(transform.localScale.x, flip ? -Mathf.Abs(transform.localScale.y) : Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }

        /// <summary>
        /// Try to get child
        /// </summary>
        /// <returns>child or null if child is not exists</returns>
        public static Transform TryGetChild(this Transform transform, int index)
        {
            if (transform.childCount < index)
                return transform.GetChild(index);

            return null;
        }

        /// <summary>
        /// Reset transforms local position, rotation and scale
        /// </summary>
        public static Transform ResetLocal(this Transform transform)
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            transform.localScale = Vector3.one;

            return transform;
        }

        /// <summary>
        /// Reset transforms position, rotation and scale
        /// </summary>
        public static Transform ResetGlobal(this Transform transform)
        {
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            transform.localScale = Vector3.one;

            return transform;
        }

        public static Transform SetPositionX(this Transform transform, float x)
        {
            var position = transform.position;
            position.x = x;
            transform.position = position;

            return transform;
        }

        public static Transform SetPositionY(this Transform transform, float y)
        {
            var position = transform.position;
            position.y = y;
            transform.position = position;

            return transform;
        }

        public static Transform SetPositionZ(this Transform transform, float z)
        {
            var position = transform.position;
            position.z = z;
            transform.position = position;

            return transform;
        }

        #endregion

        #region RectTransform
        #endregion

        #region Quaternion


        #endregion

        #region Vector3
        /// <summary>
        ///  Adds to each component specified value
        /// </summary>
        /// <param name="floatValue">value to add</param>
        /// <returns></returns>
        public static Vector3 AddFloat(this Vector3 vector, float floatValue)
        {
            vector.x += floatValue;
            vector.y += floatValue;
            vector.z += floatValue;

            return vector;
        }

        /// <summary>
        /// Adds to x component specified value
        /// </summary>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector3 AddToX(this Vector3 vector, float value)
        {
            vector.x += value;

            return vector;
        }

        /// <summary>
        /// Adds to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector3 AddToY(this Vector3 vector, float value)
        {
            vector.y += value;

            return vector;
        }

        /// <summary>
        /// Adds to z component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector3 AddToZ(this Vector3 vector, float value)
        {
            vector.z += value;

            return vector;
        }

        /// <summary>
        /// Multiplies x component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector3 MultX(this Vector3 vector, float value)
        {
            vector.x *= value;

            return vector;
        }

        /// <summary>
        /// Multiplies y component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector3 MultY(this Vector3 vector, float value)
        {
            vector.y *= value;

            return vector;
        }

        /// <summary>
        /// Multiplies z component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector3 MultZ(this Vector3 vector, float value)
        {
            vector.z *= value;

            return vector;
        }

        /// <summary>
        /// Sets to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 vector, float value)
        {
            vector.x = value;

            return vector;
        }

        /// <summary>
        /// Sets to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;

            return vector;
        }

        /// <summary>
        /// Sets to z component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;

            return vector;
        }

        /// <summary>
        /// Sets x,y,z specified value
        /// </summary>
        /// <param name="valueOfXYZ">value to set</param>
        /// <returns></returns>
        public static Vector3 SetAll(this Vector3 vector, float valueOfXYZ)
        {
            vector.x = valueOfXYZ;
            vector.y = valueOfXYZ;
            vector.z = valueOfXYZ;

            return vector;
        }

        /// <summary>
        /// Convert float value to Vector3
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this float value)
        {
            return new Vector3(value, value, value);
        }

        /// <summary>
        /// Convert int value to Vector3
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this int value)
        {
            return new Vector3(value, value, value);
        }

        /// <summary>
        /// Convert to World position
        /// </summary>
        public static Vector3 ToWorldPosition(this Vector3 vector, float z = 0)
        {
            vector.z = z;
            return Camera.main.ScreenToWorldPoint(vector);
        }

        /// <summary>
        /// Get random position x,z position around position
        /// </summary>
        public static Vector3 GetRandomPositionAroundObject(this Vector3 position, float minRadius, float maxRadius)
        {
            float radius = Random.Range(minRadius, maxRadius);

            float angle = Random.Range(0, 360);

            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            return new Vector3(position.x + x, position.y, position.z + z);
        }

        /// <summary>
        /// Get random position x,z position around position
        /// </summary>
        public static Vector3 GetRandomPositionAroundObject(this Vector3 position, float radius)
        {
            float angle = Random.Range(0, 360);

            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            return new Vector3(position.x + x, position.y, position.z + z);
        }

        public static Vector3 GetRandomPosition(this Bounds bounds)
        {
            var halfWidth = bounds.size.x / 2f;
            var halfHeight = bounds.size.y / 2f;
            var halfDepth = bounds.size.z / 2f;

            var result = bounds.center + new Vector3(
                Random.Range(-halfWidth, halfWidth),
                Random.Range(-halfHeight, halfHeight),
                Random.Range(-halfDepth, halfDepth));

            return result;
        }

        public static Vector3 GetRandomPosition(this Bounds bounds, Quaternion rotation, float offset = 0)
        {
            var halfWidth = bounds.size.x / 2f + offset;
            var halfHeight = bounds.size.y / 2f + offset;
            var halfDepth = bounds.size.z / 2f + offset;

            var result = bounds.center + rotation * new Vector3(
                Random.Range(-halfWidth, halfWidth),
                Random.Range(-halfHeight, halfHeight),
                Random.Range(-halfDepth, halfDepth));

            return result;
        }

        public static Vector2 xz(this Vector3 value) => new Vector2(value.x, value.z);
        public static Vector2 xy(this Vector3 value) => new Vector2(value.x, value.y);

        public static Vector3 xyz(this Vector4 value) => new Vector3(value.x, value.y, value.z);
        public static Vector2 xy(this Vector4 value) => new Vector2(value.x, value.y);
        #endregion

        #region Vector2
        /// <summary>
        ///  Adds to each component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="floatValue">value to add</param>
        /// <returns></returns>
        public static Vector2 AddFloat(this Vector2 vector, float floatValue)
        {
            vector.x += floatValue;
            vector.y += floatValue;

            return vector;
        }

        /// <summary>
        /// Adds to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector2 AddToX(this Vector2 vector, float value)
        {
            vector.x += value;

            return vector;
        }

        /// <summary>
        /// Adds to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector2 AddToY(this Vector2 vector, float value)
        {
            vector.y += value;

            return vector;
        }

        /// <summary>
        /// Multiplies x component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector2 MultX(this Vector2 vector, float value)
        {
            vector.x *= value;

            return vector;
        }

        /// <summary>
        /// Multiplies y component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector2 MultY(this Vector2 vector, float value)
        {
            vector.y *= value;

            return vector;
        }

        /// <summary>
        /// Sets to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector2 SetX(this Vector2 vector, float value)
        {
            vector.x = value;

            return vector;
        }

        /// <summary>
        /// Sets to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector2 SetY(this Vector2 vector, float value)
        {
            vector.y = value;

            return vector;
        }

        /// <summary>
        /// Convert float value to Vector2
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this float value)
        {
            return new Vector2(value, value);
        }

        /// <summary>
        /// Convert int value to Vector2
        /// </summary>
        /// <param name="value">value to convert</param>
        public static Vector2 ToVector2(this int value)
        {
            return new Vector2(value, value);
        }

        /// <summary>
        /// Convert Vector2 to Vector3
        /// </summary>
        /// <param name="z">Z value</param>
        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }
        #endregion

        #region Color
        /// <summary>
        /// Set color alpha
        /// </summary>
        public static Color SetAlpha(this Color color, byte aValue)
        {
            color.a = aValue;

            return color;
        }

        /// <summary>
        /// Set color alpha
        /// </summary>
        public static Color SetAlpha(this Color color, float aValue)
        {
            color.a = aValue;

            return color;
        }

        /// <summary>
        /// Set color alpha (0-255)
        /// </summary>
        public static Color SetAlpha(this Color color, int aValue)
        {
            color.a = (float)aValue / 255;

            return color;
        }

        /// <summary>
        /// Convert to HEX
        /// </summary>
        public static string ToHex(this Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", (byte)(Mathf.Clamp01(color.r) * 255), (byte)(Mathf.Clamp01(color.g) * 255), (byte)(Mathf.Clamp01(color.b) * 255));
        }

        /// <summary>
        /// Set color alpha
        /// </summary>
        public static Graphic SetAlpha(this Graphic graphic, float a)
        {
            graphic.color = graphic.color.SetAlpha(a);

            return graphic;
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// Add element to dictionary or add some new values if it exists
        /// </summary>
        public static int AddOrAdjust<T>(this Dictionary<T, int> dictionary, T key, int value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] += value;
            else
                dictionary.Add(key, value);

            return dictionary[key];
        }

        /// <summary>
        /// Add element to dictionary or set some new values if it exists
        /// </summary>
        public static int AddOrSet<T>(this Dictionary<T, int> dictionary, T key, int value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            return dictionary[key];
        }

        public static void ForEachKey<T,K>(this Dictionary<T, K> dictionary, Action<T> action)
        {
            foreach(var key in dictionary.Keys)
            {
                action(key);
            }
        }

        public static void ForEachValue<T, K>(this Dictionary<T, K> dictionary, Action<K> action)
        {
            foreach (var value in dictionary.Values)
            {
                action(value);
            }
        }

        #endregion

        #region Object
        public static string ObjectToString(this object parentObject, int maxDepth = 3)
        {
            if (parentObject == null)
                return "NULL";

            maxDepth = Mathf.Clamp(maxDepth, 1, 10);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<b>" + parentObject.ToString() + "</b>");
            stringBuilder.AppendLine("");

            string fieldsString = GetFields(0, maxDepth, "  ", parentObject);
            if(!string.IsNullOrEmpty(fieldsString))
            {
                stringBuilder.AppendLine("Variables:");
                stringBuilder.Append(fieldsString);
            }

            return stringBuilder.ToString();
        }

        private static string GetFields(int depth, int maxDepth, string space, object parentObject)
        {
            depth += 1;
               
            StringBuilder stringBuilder = new StringBuilder();
            Type parentObjectType = parentObject.GetType();

            FieldInfo[] fieldInfos = parentObjectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            string fieldName;
            object value;

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                Type fieldType = fieldInfos[i].FieldType;

                fieldName = fieldInfos[i].Name;
                value = fieldInfos[i].GetValue(parentObject);

                if (fieldType.IsClass && fieldType != typeof(string))
                {
                    if (fieldType.IsArray)
                    {
                        Type elementType = fieldType.GetElementType();
                        if (elementType != null)
                        {
                            FieldInfo[] arrayFieldInfos = elementType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                            IList list = (IList)value;
                            if(list != null)
                            {
                                stringBuilder.AppendLine(space + fieldName + " - " + fieldType.ToString());

                                int listCount = list.Count;
                                for (int j = 0; j < listCount; j++)
                                {
                                    stringBuilder.AppendLine(space + " - Element " + (j + 1));
                                    if (list[j] != null)
                                    {
                                        for (int f = 0; f < arrayFieldInfos.Length; f++)
                                        {
                                            object arrayValue = arrayFieldInfos[f].GetValue(list[j]);

                                            stringBuilder.AppendLine(space + "    " + arrayFieldInfos[f].Name + " - " + (arrayValue != null ? arrayValue.ToString() : "NULL"));
                                        }
                                    }
                                    else
                                    {
                                        stringBuilder.AppendLine(space + "    NULL");
                                    }
                                }
                            }
                            else
                            {
                                stringBuilder.AppendLine(space + fieldName + " - NULL");
                            }
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            stringBuilder.AppendLine(space + fieldName + " - " + fieldType.ToString());

                            if (depth < maxDepth)
                                stringBuilder.Append(GetFields(depth, maxDepth, space + "    ", value));
                        }
                        else
                        {
                            stringBuilder.AppendLine(space + fieldName + " - NULL");
                        }
                    }
                }
                else
                {
                    stringBuilder.AppendLine(space + fieldName + " - " + (value != null ? value.ToString() : "NULL"));
                }
            }

            return stringBuilder.ToString();
        }
#endregion

#region Random

        public static class ExtendedRandom
        {
            public static bool TrueFalse()
            {
                return Random.Range(0, 2) == 0;
            }

            public static int Range(DuoInt duoInt)
            {
                return Random.Range(duoInt.firstValue, duoInt.secondValue);
            }

            public static float Range(DuoFloat duoFloat)
            {
                return Random.Range(duoFloat.firstValue, duoFloat.secondValue);
            }
        }

#endregion
    }
}