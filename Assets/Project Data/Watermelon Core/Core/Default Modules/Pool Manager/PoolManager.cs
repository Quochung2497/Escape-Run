#pragma warning disable 0414

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Watermelon
{
    /// <summary>
    /// Class that manages all pool operations.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;

        /// <summary>
        /// List of all existing pools.
        /// </summary>
        [SerializeField] List<Pool> poolsList = new List<Pool>();

        /// <summary>
        /// Dictionary which allows to acces Pool by name.
        /// </summary>
        private Dictionary<string, Pool> poolsDictionary;

        private int spawnedObjectAmount = 0;

        /// <summary>
        /// Amount of spawned objects.
        /// </summary>
        public static int SpawnedObjectsAmount => instance.spawnedObjectAmount;

        private static Transform objectsContainer;
        public static Transform ObjectsContainerTransform => objectsContainer;

        private void Awake()
        {
            InitSingletone(this);
        }

        /// <summary>
        /// Initialize a single instance of PoolManager.
        /// </summary>
        private static void InitSingletone(PoolManager poolManager = null)
        {
            if (instance != null)
                return;

            if(poolManager == null)
                poolManager = FindObjectOfType<PoolManager>();

            if (poolManager != null)
            {
                // Save instance
                instance = poolManager;

#if UNITY_EDITOR
                // Create container object
                GameObject containerObject = new GameObject("[POOL OBJECTS]");
                objectsContainer = containerObject.transform;
                objectsContainer.ResetGlobal();
#endif

                // Link and initialise pools
                poolManager.poolsDictionary = new Dictionary<string, Pool>();

                foreach (Pool pool in poolManager.poolsList)
                {
                    poolManager.poolsDictionary.Add(pool.Name, pool);

                    pool.Initialize();
                }

                return;
            }

            Debug.LogError("[PoolManager]: Please, add PoolManager behaviour at scene.");
        }

        public static void Unload()
        {
            PoolManager poolManager = instance; 
            if(poolManager != null)
            {
                for(int i = 0; i < poolManager.poolsList.Count; i++)
                {
                    poolManager.poolsList[i].ReturnToPoolEverything(true);
                }
            }
        }

        public static GameObject SpawnObject(GameObject prefab, Transform parrent)
        {
#if UNITY_EDITOR
            if (parrent == null)
                parrent = ObjectsContainerTransform;
#endif

            instance.spawnedObjectAmount++;

            return Instantiate(prefab, parrent);
        }

        /// <summary>
        /// Returns reference to Pool by it's name.
        /// </summary>
        /// <param name="poolName">Name of Pool which should be returned.</param>
        /// <returns>Reference to Pool.</returns>
        public static Pool GetPoolByName(string poolName)
        {
            InitSingletone();

            if (instance.poolsDictionary.ContainsKey(poolName))
            {
                return instance.poolsDictionary[poolName];
            }

            Debug.LogError("[PoolManager] Not found pool with name: '" + poolName + "'");

            return null;
        }

        public static PoolGeneric<T> GetPoolByName<T>(string poolName) where T : Component
        {
            InitSingletone();

            if (instance.poolsDictionary.ContainsKey(poolName))
            {
                Pool unboxedPool = instance.poolsDictionary[poolName];

                try
                {
                    return unboxedPool as PoolGeneric<T>;
                }
                catch (Exception)
                {
                    Debug.Log($"[PoolManager] Could not convert pool with name {poolName} to {typeof(PoolGeneric<T>)}");

                    return null;
                }
            }

            Debug.LogError("[PoolManager] Not found generic pool with name: '" + poolName + "'");

            return null;
        }

        /// <summary>
        /// Adds new pool at runtime.
        /// </summary>
        /// <param name="poolBuilder">Pool builder settings.</param>
        /// <returns>Newly created pool.</returns>
        public static Pool AddPool(PoolSettings poolBuilder)
        {
            InitSingletone();

            if (instance.poolsDictionary.ContainsKey(poolBuilder.name))
            {
                Debug.LogError("[Pool manager] Adding a new pool failed. Name \"" + poolBuilder.name + "\" already exists.");
                return GetPoolByName(poolBuilder.name);
            }

            Pool newPool = new Pool(poolBuilder);
            instance.poolsDictionary.Add(newPool.Name, newPool);
            instance.poolsList.Add(newPool);

            newPool.Initialize();

            return newPool;
        }

        public static PoolGeneric<T> AddPool<T>(PoolSettings poolBuilder) where T : Component
        {
            InitSingletone();

            if (instance.poolsDictionary.ContainsKey(poolBuilder.name))
            {
                Debug.LogError("[Pool manager] Adding a new pool failed. Name \"" + poolBuilder.name + "\" already exists.");

                return GetPoolByName<T>(poolBuilder.name);
            }

            PoolGeneric<T> poolGeneric = new PoolGeneric<T>(poolBuilder);
            instance.poolsDictionary.Add(poolGeneric.Name, poolGeneric);
            instance.poolsList.Add(poolGeneric);

            poolGeneric.Initialize();

            return poolGeneric;
        }

        public static void AddPool(Pool pool)
        {
            InitSingletone();

            if (instance.poolsDictionary.ContainsKey(pool.Name))
            {
                Debug.LogError("[Pool manager] Adding a new pool failed. Name \"" + pool.Name + "\" already exists.");

                return;
            }

            instance.poolsDictionary.Add(pool.Name, pool);
            instance.poolsList.Add(pool);

            pool.Initialize();
        }

        public static void DestroyPool(Pool pool)
        {
            pool.Clear();

            instance.poolsDictionary.Remove(pool.Name);
            instance.poolsList.Remove(pool);
        }

        public static bool PoolExists(string name)
        {
            if (instance == null)
            {
                return false;
            }
            else
            {
                return instance.poolsDictionary.ContainsKey(name);
            }
        }

        public static void RemoveAllNullObjects()
        {
            foreach(var poolKeyValue in instance.poolsDictionary)
            {
                poolKeyValue.Value.DeleteAllNullRefsInSpawnedObjects();
            }
        }

        // editor methods

        private bool IsAllPrefabsAssignedAtPool(int poolIndex)
        {
            if (poolsList != null && poolIndex < poolsList.Count)
            {
                return poolsList[poolIndex].IsAllPrefabsAssigned();
            }
            else
            {
                return true;
            }
        }

        private void RecalculateWeightsAtPool(int poolIndex)
        {
            poolsList[poolIndex].RecalculateWeights();
        }
    }
}

// -----------------
// Pool Manager v 1.6.5
// -----------------

// Changelog
// v 1.6.5
// • Removed Initialise method
// • Now manager works as Singleton
// • Added generic AddPool method
// v 1.6.4
// • Added pro theme support
// v 1.6 
// • Added runtime pool creation
// • Added extended functions for multi pool
// • Added new pool constructor and GetPooledObject overrides
// • Generic pool upgate
// • Added clear method to pool
// v 1.5.1 
// • Added Multi objects pool type
// • Added drag n drop support
// v 1.4.5  
// • Added editor changes save
// • Updated cache system
// • Added ability to ignore cache for required pools
// • Fixed created object's names
// • Core refactoring
// • Editor UX improvements
// v 1.3.1  
// • Added RandomPools system
// • Added objectsContainer access property
// v 1.2.1 
// • Added cache system
// • Fixed errors on build
// v 1.1.0 
// • Added PoolManager editor
// v 1.0.0 
// • Basic version of pool
