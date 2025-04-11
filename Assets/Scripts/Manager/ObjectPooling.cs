using GPHive.Game;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Manager
{
    public class ObjectPooling : Singleton<ObjectPooling>
    {
         public ObjectToPool[] _objectsToPool;

        public delegate void OnGetFromPool();

        public static event OnGetFromPool GotFromPool;

        void Awake()
        {
            InitializePool();

        }

        public GameObject GetFromPool(string poolName)
        {
            GameObject objectToReturn = null;
            ObjectToPool currentPool = null;
            foreach (var pool in _objectsToPool)
            {
                if (pool.name != poolName) continue;

                currentPool = pool;
                foreach (var pooledObject in pool.pooledObjects)
                {
                    if (pooledObject.activeSelf) continue;

                    objectToReturn = pooledObject;
                }
            }

            if (!objectToReturn)
            {
                for (int i = 0; i < currentPool.expandAmount; i++)
                {
                    var pooled = Instantiate(currentPool.objectToPool, transform);
                    pooled.name = currentPool.name;
                    pooled.SetActive(false);
                    currentPool.pooledObjects.Add(pooled);
                }
                objectToReturn = GetFromPool(poolName);
            }

            GotFromPool?.Invoke();
            return objectToReturn;
        }

        public void Deposit(GameObject gameObject)
        {
            foreach (ObjectToPool pool in _objectsToPool)
            {
                if (pool.pooledObjects.Contains(gameObject))
                {
                    gameObject.transform.SetParent(transform);
                    gameObject.SetActive(false);
                    gameObject.transform.ResetTransform();

                    if (gameObject.TryGetComponent<Rigidbody>(out var rigidbodyObject))
                        rigidbodyObject.ResetVelocity();

                    return;
                }
            }

            Debug.LogError("Trying to deposit an object that isn't in the pool.");
        }

        public void Deposit(string poolName)
        {
            foreach (var pool in _objectsToPool)
            {
                if (pool.name != poolName) continue;

                foreach (var pooledObject in pool.pooledObjects)
                {
                    pooledObject.transform.SetParent(transform);
                    pooledObject.SetActive(false);
                    pooledObject.transform.ResetTransform();

                    if (pooledObject.TryGetComponent<Rigidbody>(out var rigidbodyObject))
                        rigidbodyObject.ResetVelocity();
                }
            }
        }

        public void DepositAll()
        {
            foreach (ObjectToPool pool in _objectsToPool)
            {
                foreach (GameObject poolObject in pool.pooledObjects)
                {
                    poolObject.transform.SetParent(transform);
                    poolObject.transform.ResetTransform();
                    poolObject.SetActive(false);

                    if (poolObject.TryGetComponent<Rigidbody>(out var rigidbodyObject))
                        rigidbodyObject.ResetVelocity();
                }
            }
        }

        public void Clear()
        {
            foreach (var pool in _objectsToPool)
            {
                foreach (var poolObject in pool.pooledObjects)
                {
                    Destroy(poolObject);
                }

                pool.pooledObjects.Clear();
            }
        }

        private void InitializePool()
        {
            foreach (var pool in _objectsToPool)
            {
                for (int i = 0; i < pool.poolCount; i++)
                {
                    var pooled = Instantiate(pool.objectToPool, transform);
                    pooled.name = pool.name;
                    pooled.SetActive(false);
                    pool.pooledObjects.Add(pooled);
                }
            }
        }
    }
}