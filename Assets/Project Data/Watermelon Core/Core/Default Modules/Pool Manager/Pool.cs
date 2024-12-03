using UnityEngine;
using System;
using System.Collections.Generic;

namespace Watermelon
{
    /// <summary>
    /// Basic pool class. Contains pool settings and references to pooled objects.
    /// </summary>
    [Serializable]
    public class Pool
    {
        [SerializeField]
        protected string name;
        /// <summary>
        /// Pool name, use it get pool reference at PoolManager.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        [SerializeField]
        protected PoolType type = PoolType.Single;
        /// <summary>
        /// Type of pool.
        /// Single - classic pool with one object. Multiple - pool with multiple objects returned randomly using weights.
        /// </summary>
        public PoolType Type
        {
            get { return type; }
        }

        [SerializeField]
        protected GameObject singlePoolPrefab = null;
        /// <summary>
        /// Reference to single pool prefab.
        /// </summary>
        public GameObject SinglePoolPrefab
        {
            get { return singlePoolPrefab; }
        }


        /// <summary>
        /// List to multiple pool prefabs list.
        /// </summary>
        [SerializeField]
        protected List<MultiPoolPrefab> multiPoolPrefabsList = new List<MultiPoolPrefab>();

        /// <summary>
        /// Amount of prefabs at multi type pool.
        /// </summary>
        public int MultiPoolPrefabsAmount
        {
            get { return multiPoolPrefabsList.Count; }
        }

        [SerializeField]
        private int size = 10;
        /// <summary>
        /// Number of objects which be created be deffault.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        [SerializeField]
        protected bool autoSizeIncrement = true;
        /// <summary>
        /// If enabled pool size will grow automatically if there is no more available objects.
        /// </summary>
        public bool AutoSizeIncrement
        {
            get { return autoSizeIncrement; }
        }


        [SerializeField]
        protected Transform objectsContainer = null;
        /// <summary>
        /// Custom objects container for pool's objects.
        /// </summary>
        public Transform ObjectsContainer
        {
            get { return objectsContainer; }
        }

        [SerializeField]
        /// <summary>
        /// Is pool created at runtime indicator.
        /// </summary>
        private bool isRuntimeCreated;

        [SerializeField]
        /// <summary>
        /// True when all default objects spawned.
        /// </summary>
        protected bool inited = false;

        /// <summary>
        /// List of pooled objects for single pull.
        /// </summary>
        protected List<GameObject> pooledObjects = new List<GameObject>();
        /// <summary>
        /// List of pooled objects for multiple pull.
        /// </summary>
        protected List<List<GameObject>> multiPooledObjects = new List<List<GameObject>>();

#if UNITY_EDITOR
        /// <summary>
        /// Number of objects that where active at one time.
        /// </summary>
        protected int maxItemsUsedInOneTime = 0;
#endif

        public enum PoolType
        {
            Single = 0,
            Multi = 1,
        }

        [System.Serializable]
        public struct MultiPoolPrefab
        {
            public GameObject prefab;
            public int weight;
            public bool isWeightLocked;

            public MultiPoolPrefab(GameObject prefab, int weight, bool isWeightLocked)
            {
                this.prefab = prefab;
                this.weight = weight;
                this.isWeightLocked = isWeightLocked;
            }
        }

        public Pool(PoolSettings builder)
        {
            name = builder.name;
            type = builder.type;
            singlePoolPrefab = builder.singlePoolPrefab;
            multiPoolPrefabsList = builder.multiPoolPrefabsList;
            size = builder.size;
            autoSizeIncrement = builder.autoSizeIncrement;
            objectsContainer = builder.objectsContainer;

            isRuntimeCreated = !PoolManager.PoolExists(name);
            inited = false;
        }

        /// <summary>
        /// Initializes pool.
        /// </summary>
        public void Initialize()
        {
            if (inited)
                return;

            if (type == PoolType.Single)
            {
                InitializeAsSingleTypePool();
            }
            else
            {
                InitializeAsMultiTypePool();
            }
        }

        /// <summary>
        /// Filling pool with spawned by default objects.
        /// </summary>
        protected void InitializeAsSingleTypePool()
        {
            pooledObjects = new List<GameObject>();

            if (singlePoolPrefab != null)
            {
                for (int i = 0; i < size; i++)
                {
                    AddObjectToPoolSingleType(" ");
                }

                inited = true;
            }
            else
            {
                Debug.LogError("[PoolManager] There's no attached prefab at pool: \"" + name + "\"");
            }
        }

        /// <summary>
        /// Filling pool with spawned by default objects.
        /// </summary>
        protected void InitializeAsMultiTypePool()
        {
            multiPooledObjects = new List<List<GameObject>>();

            for (int i = 0; i < multiPoolPrefabsList.Count; i++)
            {
                multiPooledObjects.Add(new List<GameObject>());

                if (multiPoolPrefabsList[i].prefab != null)
                {
                    for (int j = 0; j < size; j++)
                    {
                        AddObjectToPoolMultiType(i, " ");
                    }

                    inited = true;
                }
                else
                {
                    Debug.LogError("[PoolManager] There's not attached prefab at pool: \"" + name + "\"");
                }

            }
        }

        protected virtual void InitGenericSingleObject(GameObject prefab) { }
        protected virtual void InitGenericMultiObject(int poolIndex, GameObject prefab) { }
        protected virtual void OnPoolCleared() { }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, false, Vector3.zero);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, true, position);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, false, Vector3.zero);
        }

        /// <summary>
        /// Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, true, position);
        }

        /// <summary>
        /// Rerurns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public GameObject GetPooledObject(PooledObjectSettings settings)
        {
            if (type == PoolType.Single)
            {
                return GetPooledObjectSingleType(settings);
            }
            else
            {
                return GetPooledObjectMultiType(settings, -1);
            }
        }

        /// <summary>
        /// Internal override of GetPooledObject and GetHierarchyPooledObject methods.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObject(bool checkTypeActiveSelf, bool activateObject, bool setPosition, Vector3 position)
        {
            PooledObjectSettings settings = new PooledObjectSettings(activateObject, !checkTypeActiveSelf);

            if (setPosition)
            {
                settings = settings.SetPosition(position);
            }

            if (type == PoolType.Single)
            {
                return GetPooledObjectSingleType(settings);
            }
            else
            {
                return GetPooledObjectMultiType(settings, -1);
            }
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Single type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObjectSingleType(PooledObjectSettings settings)
        {
            if (!inited)
                InitializeAsSingleTypePool();

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                var obj = pooledObjects[i];

                if(obj == null)
                {
                    GameObject newObject = PoolManager.SpawnObject(singlePoolPrefab, objectsContainer);

                    newObject.name += " " + PoolManager.SpawnedObjectsAmount;
                    newObject.SetActive(false);

                    pooledObjects[i] = newObject;

                    InitGenericSingleObject(newObject);
                }

                if (settings.UseActiveOnHierarchy ? !pooledObjects[i].activeInHierarchy : !pooledObjects[i].activeSelf)
                {
                    SetupPooledObject(pooledObjects[i], settings);
                    return pooledObjects[i];
                }
            }

            if (autoSizeIncrement)
            {
                GameObject newObject = AddObjectToPoolSingleType(" e");
                SetupPooledObject(newObject, settings);

                return newObject;
            }

            return null;
        }

        /// <summary>
        /// Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Multi type pool.
        /// </summary>
        /// <param name="checkTypeActiveSelf">Which type of checking object's activation state is used: active self or active in hierarchy.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObjectMultiType(PooledObjectSettings settings, int poolIndex)
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
                int currentValue = 0;
                int totalWeight = 0;

                for (int i = 0; i < multiPoolPrefabsList.Count; i++)
                {
                    totalWeight += multiPoolPrefabsList[i].weight;
                }

                int randomValue = UnityEngine.Random.Range(1, totalWeight);
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
                    return objectsList[i];
                }
            }

            if (autoSizeIncrement)
            {
                GameObject newObject = AddObjectToPoolMultiType(chosenPoolIndex, " e");
                SetupPooledObject(newObject, settings);

                return newObject;
            }

            return null;
        }

        /// <summary>
        /// Applies pooled object settings to object.
        /// </summary>
        /// <param name="gameObject">Game object to apply settings.</param>
        /// <param name="settings">Settings to apply.</param>
        protected void SetupPooledObject(GameObject gameObject, PooledObjectSettings settings)
        {
            Transform objectTransform = gameObject.transform;

            if (settings.ApplyParrent)
            {
                objectTransform.SetParent(settings.Parrent);
            }

            if (settings.ApplyPosition)
            {
                objectTransform.position = settings.Position;
            }

            if (settings.ApplyLocalPosition)
            {
                objectTransform.localPosition = settings.LocalPosition;
            }

            if (settings.ApplyEulerRotation)
            {
                objectTransform.eulerAngles = settings.EulerRotation;
            }

            if(settings.ApplyLocalEulerRotation)
            {
                objectTransform.localEulerAngles = settings.LocalEulerRotation;
            }

            if (settings.ApplyRotation)
            {
                objectTransform.rotation = settings.Rotation;
            }

            if (settings.ApplyLocalRotation)
            {
                objectTransform.rotation = settings.LocalRotation;
            }

            if (settings.ApplyLocalScale)
            {
                objectTransform.localScale = settings.LocalScale;
            }

            gameObject.SetActive(settings.Activate);
        }

        /// <summary>
        /// Adds one more object to a single type pool.
        /// </summary>
        /// <param name="pool">Pool at which should be added new object.</param>
        /// <returns>Returns reference to just added object.</returns>
        protected GameObject AddObjectToPoolSingleType(string nameAddition)
        {
            GameObject newObject = PoolManager.SpawnObject(singlePoolPrefab, objectsContainer);

            newObject.name += nameAddition + PoolManager.SpawnedObjectsAmount;
            newObject.SetActive(false);

            pooledObjects.Add(newObject);
            InitGenericSingleObject(newObject);

            return newObject;
        }

        public void CreatePoolObjects(int count)
        {
            int sizeDifference = count - pooledObjects.Count;
            if (sizeDifference > 0)
            {
                for (int i = 0; i < sizeDifference; i++)
                {
                    AddObjectToPoolSingleType(" ");
                }
            }
        }

        /// <summary>
        /// Adds one more object to multi type Pool.
        /// </summary>
        /// <param name="pool">Pool at which should be added new object.</param>
        /// <returns>Returns reference to just added object.</returns>
        protected GameObject AddObjectToPoolMultiType(int PoolIndex, string nameAddition)
        {
            GameObject newObject = PoolManager.SpawnObject(multiPoolPrefabsList[PoolIndex].prefab, objectsContainer);

            newObject.name += nameAddition + PoolManager.SpawnedObjectsAmount;
            newObject.SetActive(false);
            multiPooledObjects[PoolIndex].Add(newObject);
            InitGenericMultiObject(PoolIndex, newObject);

            return newObject;
        }

        /// <summary>
        /// Sets initial parrents to all objects.
        /// </summary>
        public void ResetParrents()
        {
            if (type == PoolType.Single)
            {
                for (int i = 0; i < pooledObjects.Count; i++)
                {
                    pooledObjects[i].transform.SetParent(objectsContainer != null ? objectsContainer : PoolManager.ObjectsContainerTransform);
                }
            }
            else
            {
                for (int i = 0; i < multiPooledObjects.Count; i++)
                {
                    for (int j = 0; j < multiPooledObjects[i].Count; j++)
                    {
                        multiPooledObjects[i][j].transform.SetParent(objectsContainer != null ? objectsContainer : PoolManager.ObjectsContainerTransform);
                    }
                }
            }
        }

        /// <summary>
        /// Disables all active objects from this pool.
        /// </summary>
        /// <param name="resetParrent">Sets default parrent if checked.</param>
        public void ReturnToPoolEverything(bool resetParrent = false)
        {
            if (type == PoolType.Single)
            {
                for (int i = 0; i < pooledObjects.Count; i++)
                {
                    if (resetParrent)
                    {
                        pooledObjects[i].transform.SetParent(objectsContainer != null ? objectsContainer : PoolManager.ObjectsContainerTransform);
                    }

                    pooledObjects[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < multiPooledObjects.Count; i++)
                {
                    for (int j = 0; j < multiPooledObjects[i].Count; j++)
                    {
                        if (resetParrent)
                        {
                            multiPooledObjects[i][j].transform.SetParent(objectsContainer != null ? objectsContainer : PoolManager.ObjectsContainerTransform);
                        }
                        multiPooledObjects[i][j].SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Destroys all spawned objects. Note, this method is performance heavy.
        /// </summary>
        public void Clear()
        {
            if (type == PoolType.Single)
            {
                for (int i = 0; i < pooledObjects.Count; i++)
                {
                    UnityEngine.Object.Destroy(pooledObjects[i]);
                }

                pooledObjects.Clear();
            }
            else
            {
                for (int i = 0; i < multiPooledObjects.Count; i++)
                {
                    for (int j = 0; j < multiPooledObjects[i].Count; j++)
                    {
                        UnityEngine.Object.Destroy(multiPooledObjects[i][j]);
                    }

                    multiPooledObjects[i].Clear();
                }
            }

            OnPoolCleared();
        }

        /// <summary>
        /// Returns object from multi type pool by it's index on prefabs list.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="activateObject"></param>
        /// <returns></returns>
        public GameObject GetMultiPooledObjectByIndex(int index, PooledObjectSettings setting)
        {
            return GetPooledObjectMultiType(setting, index);
        }

        /// <summary>
        /// Rerurns prefab from multi type pool by it's index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MultiPoolPrefab MultiPoolPrefabByIndex(int index)
        {
            return multiPoolPrefabsList[index];
        }

        /// <summary>
        /// Evenly distributes the weight between multi pooled objects, leaving locked weights as is.
        /// </summary>
        public void RecalculateWeights()
        {
            List<MultiPoolPrefab> oldPrefabsList = new List<MultiPoolPrefab>(multiPoolPrefabsList);
            multiPoolPrefabsList = new List<MultiPoolPrefab>();

            if (oldPrefabsList.Count > 0)
            {
                int totalUnlockedPoints = 100;
                int unlockedPrefabsAmount = oldPrefabsList.Count;

                for (int i = 0; i < oldPrefabsList.Count; i++)
                {
                    if (oldPrefabsList[i].isWeightLocked)
                    {
                        totalUnlockedPoints -= oldPrefabsList[i].weight;
                        unlockedPrefabsAmount--;
                    }
                }

                if (unlockedPrefabsAmount > 0)
                {
                    int averagePoints = totalUnlockedPoints / unlockedPrefabsAmount;
                    int additionalPoints = totalUnlockedPoints - averagePoints * unlockedPrefabsAmount;

                    for (int j = 0; j < oldPrefabsList.Count; j++)
                    {
                        if (oldPrefabsList[j].isWeightLocked)
                        {
                            multiPoolPrefabsList.Add(oldPrefabsList[j]);
                        }
                        else
                        {
                            multiPoolPrefabsList.Add(new MultiPoolPrefab(oldPrefabsList[j].prefab, averagePoints + (additionalPoints > 0 ? 1 : 0), false));
                            additionalPoints--;
                        }
                    }
                }
                else
                {
                    multiPoolPrefabsList = oldPrefabsList;
                }
            }
        }

        /// <summary>
        /// Checks are all prefabs references assigned.
        /// </summary>
        public bool IsAllPrefabsAssigned()
        {
            if (type == PoolType.Single)
            {
                return singlePoolPrefab != null;
            }
            else
            {
                if (multiPoolPrefabsList.Count == 0)
                    return false;

                for (int i = 0; i < multiPoolPrefabsList.Count; i++)
                {
                    if (multiPoolPrefabsList[i].prefab == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void DeleteAllNullRefsInSpawnedObjects()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if(pooledObjects[i] == null)
                {
                    Debug.Log("Found null ref in pool: " + name);
                    pooledObjects.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

// -----------------
// Pool Manager v 1.6.5
// -----------------