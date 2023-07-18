using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectUtils.ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<PoolObject> initialObjectPool;
        private List<PoolObject> _objectPool;
        
        private GameObject _objectPoolParent;
        
        [Serializable]
        struct PoolObject
        {
            public GameObject gameObject;
            public int quantity;
            public float lifeTime;
            [HideInInspector] public float activeTime;
        }

        public static ObjectPool Instance;

        private void Start()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;

            _objectPool = new List<PoolObject>();
            _objectPoolParent = new GameObject("ObjectsPool");

            //For every object in the list, instantiate it the requested number of times
            for (int i = 0; i < initialObjectPool.Count; i++)
            {
                for (int j = 0; j < initialObjectPool[i].quantity; j++)
                {
                    var temp = Instantiate(initialObjectPool[i].gameObject, _objectPoolParent.transform);
                    _objectPool.Add(new PoolObject
                    {
                        gameObject = temp,  
                        quantity = 1,
                        lifeTime = initialObjectPool[i].lifeTime 
                    });
                    temp.SetActive(false);
                }
            }
        }

        public GameObject InstantiateFromPool(GameObject prefab, Vector3 position, Quaternion rotation, bool disappearsWithTime = false)
        {
            //Searches for every object in the pool and see if it is an instance of the prefab and it is inactive
            for (var i = 0; i < _objectPool.Count; i++)
            {
                var prefabFromPool = _objectPool[i];
                if (prefabFromPool.gameObject.name != $"{prefab.name}(Clone)" ||
                    prefabFromPool.gameObject.activeInHierarchy) continue;

                prefabFromPool.activeTime = disappearsWithTime ? Time.time : float.MaxValue;
                prefabFromPool.gameObject.transform.position = position;
                prefabFromPool.gameObject.transform.localRotation = rotation;
                _objectPool[i] = prefabFromPool;
                prefabFromPool.gameObject.SetActive(true);
                return prefabFromPool.gameObject;
            }

            //If haven't found any valid object, create a new one
            prefab.transform.position = position;
            prefab.transform.localRotation = rotation;
            GameObject temp = Instantiate(prefab, _objectPoolParent.transform);
            
            PoolObject poolObject = new PoolObject
            {
                gameObject = temp,
                quantity = 1,
                lifeTime = GetObjectLifeSpan(temp),
                activeTime = disappearsWithTime ? Time.time : float.MaxValue
            };
            _objectPool.Add(poolObject);
            return temp;
        }
        
        public GameObject InstantiateFromPoolIndex(int prefabIndex, Vector3 position, Quaternion rotation, bool disappearsWithTime = false)
        {
            return InstantiateFromPool(initialObjectPool[prefabIndex].gameObject, position, rotation, disappearsWithTime);
        }


        private void Update()
        {
            if (_objectPool.Count <= 0) return;
            
            foreach (var poolObject in _objectPool)
            {
                if (!poolObject.gameObject.activeInHierarchy
                    || poolObject.activeTime > Time.time - poolObject.lifeTime) continue;

                poolObject.gameObject.SetActive(false);
            }
        }

        private float GetObjectLifeSpan(GameObject prefab)
        {
            for (int i = 0; i < initialObjectPool.Count; i++)
            {
                if(prefab.name == $"{initialObjectPool[i].gameObject.name}(Clone)")
                {
                    return initialObjectPool[i].lifeTime;
                }
            }
            return -1;
        }
    }
}
