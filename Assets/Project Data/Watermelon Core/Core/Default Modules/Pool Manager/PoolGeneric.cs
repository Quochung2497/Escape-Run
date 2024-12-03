using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    /// <summary>
    /// Generic pool. Caches specified component allowing not to use GetComponent<> after each call. Can not be added into the PoolManager.
    /// To use just create new instance.
    /// </summary>
    /// <typeparam name="T">Component to cache.</typeparam>
    [System.Serializable]
    public class PoolGeneric<T> : Pool where T : Component
    {
        public List<T> pooledComponents = new List<T>();
        public List<List<T>> multiPooledComponents = new List<List<T>>();

        public delegate void TCallback(T value);

        public void ForEach(TCallback callback)
        {
            for(int i = 0; i < pooledComponents.Count; i++)
            {
                callback(pooledComponents[i]);
            }
        }

        public PoolGeneric(PoolSettings settings) : base(settings)
        {

        }

        protected override void InitGenericSingleObject(GameObject prefab)
        {
            T component = prefab.GetComponent<T>();

            if (component != null)
            {
                pooledComponents.Add(component);
            }
            else
            {
                Debug.LogError("There's no attached component of type: " + typeof(T).ToString() + " on prefab at pool called: " + Name);
            }
        }

        protected override void InitGenericMultiObject(int poolIndex, GameObject prefab)
        {
            if (poolIndex >= multiPooledComponents.Count)
            {
                for (int i = 0; i < poolIndex - multiPooledComponents.Count + 1; i++)
                {
                    multiPooledComponents.Add(new List<T>());
                }
            }

            multiPooledComponents[poolIndex].Add(prefab.GetComponent<T>());
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public T GetPooledComponent(bool activateObject = true)
        {
            return GetPooledComponent(true, activateObject, false, Vector3.zero);
        }

        public T[] GetPooledComponents(int amount, bool activateObject = true)
        {
            return GetPooledComponents(amount, true, activateObject, false, Vector3.zero);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public T GetPooledComponent(Vector3 position, bool activateObject = true)
        {
            return GetPooledComponent(true, activateObject, true, position);
        }


        /// <summary>
        /// Rerurns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public T GetPooledComponent(PooledObjectSettings settings)
        {
            if (type == PoolType.Single)
            {
                return GetPooledComponentSingleType(settings);
            }
            else
            {
                return GetPooledComponentMultiType(settings, -1);
            }
        }

        /// <summary>
        /// Internal override of GetPooledObject and GetHierarchyPooledObject methods.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private T GetPooledComponent(bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            PooledObjectSettings settings = new PooledObjectSettings(activateObject, !checkTypeActiveSelf);

            if (setPosition)
            {
                settings = settings.SetPosition(position);
            }

            if (type == PoolType.Single)
            {
                return GetPooledComponentSingleType(settings);
            }
            else
            {
                return GetPooledComponentMultiType(settings, -1);
            }
        }

        private T[] GetPooledComponents(int amount, bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            PooledObjectSettings settings = new PooledObjectSettings(activateObject, !checkTypeActiveSelf);

            if (setPosition)
            {
                settings = settings.SetPosition(position);
            }

            if (type == PoolType.Single)
            {
                return GetPooledComponentsSingleType(amount, settings);
            }
            else
            {
                // Change Later
                //return GetPooledComponentMultiType(settings, -1);
                return GetPooledComponentsSingleType(amount, settings);
            }
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Single type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private T GetPooledComponentSingleType(PooledObjectSettings settings)
        {
            if (!inited)
                InitializeAsSingleTypePool();

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                var pooledObject = pooledObjects[i];

                if(pooledObject == null)
                {
                    // Creating a new object

                    Debug.LogWarning("Destroyed pool object located: " + singlePoolPrefab.name);

                    GameObject newObject = PoolManager.SpawnObject(singlePoolPrefab, objectsContainer);

                    newObject.name += " " + PoolManager.SpawnedObjectsAmount;
                    newObject.SetActive(false);

                    pooledObjects[i] = newObject;

                    InitGenericSingleObject(newObject);

                    pooledComponents[i] = newObject.GetComponent<T>();
                }

                if (settings.UseActiveOnHierarchy ? !pooledObjects[i].activeInHierarchy : !pooledObjects[i].activeSelf)
                {
                    SetupPooledObject(pooledObjects[i], settings);
                    return pooledComponents[i];
                }
            }

            if (autoSizeIncrement)
            {
                GameObject newObject = AddObjectToPoolSingleType(" e");
                SetupPooledObject(newObject, settings);

                return pooledComponents[pooledComponents.Count - 1];
            }

            return null;
        }

        private T[] GetPooledComponentsSingleType(int amount, PooledObjectSettings settings)
        {
            if (!inited)
                InitializeAsSingleTypePool();

            var result = new T[amount];

            var counter = 0;

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                var obj = pooledObjects[i];
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);

                    result[counter] = pooledComponents[i];

                    counter++;

                    if(counter == amount)
                    {
                        return result;
                    }
                }
            }

            for(int i = counter; i < amount; i++)
            {
                var index = pooledComponents.Count;

                GameObject newObject = AddObjectToPoolSingleType(" e");

                newObject.SetActive(true);

                result[i] = pooledComponents[index];
            }

            return result;
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Multi type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private T GetPooledComponentMultiType(PooledObjectSettings settings, int poolIndex)
        {
            if (!inited)
                InitializeAsMultiTypePool();

            int chosenPoolIndex = 0;

            if (poolIndex != -1)
            {
                chosenPoolIndex = poolIndex;
            }
            else
            {
                int randomPoolIndex = 0;
                bool randomValueWasInRange = false;
                int randomValue = UnityEngine.Random.Range(1, 101);
                int currentValue = 0;

                for (int i = 0; i < multiPoolPrefabsList.Count; i++)
                {
                    currentValue += multiPoolPrefabsList[i].weight;

                    if (randomValue <= currentValue)
                    {
                        randomPoolIndex = i;
                        randomValueWasInRange = true;
                        break;
                    }
                }

                if (!randomValueWasInRange)
                {
                    Debug.LogError("[Pool Manager] Random value(" + randomValue + ") is out of weights sum range at pool: \"" + name + "\"");
                }

                chosenPoolIndex = randomPoolIndex;
            }

            List<GameObject> objectsList = multiPooledObjects[chosenPoolIndex];

            for (int i = 0; i < objectsList.Count; i++)
            {
                if (settings.UseActiveOnHierarchy ? !objectsList[i].activeInHierarchy : !objectsList[i].activeSelf)
                {
                    SetupPooledObject(objectsList[i], settings);
                    return multiPooledComponents[chosenPoolIndex][i];
                }
            }

            if (autoSizeIncrement)
            {
                GameObject newObject = AddObjectToPoolMultiType(chosenPoolIndex, " e");
                SetupPooledObject(newObject, settings);

                return multiPooledComponents[chosenPoolIndex][multiPooledComponents[chosenPoolIndex].Count - 1];
            }

            return null;
        }

        protected override void OnPoolCleared()
        {
            if (type == PoolType.Single)
            {
                pooledComponents.Clear();
            }
            else
            {
                for (int i = 0; i < multiPooledComponents.Count; i++)
                {
                    multiPooledComponents[i].Clear();
                }

                multiPooledComponents.Clear();
            }
        }
    }
}

// -----------------
// Pool Manager v 1.6.5
// -----------------