using UnityEngine;
using System.Collections.Generic;

namespace StackGame
{
    public class Pool : APool
    {
        private Stack<APooledObject> _availableObjects = null;

        private void Awake()
        {
            _availableObjects = new Stack<APooledObject>(_initialAmount);

            for (int i = 0; i < _initialAmount; i++)
            {
                var pooledObject = Instantiate(_pooledObjectPrefab);
                pooledObject.Init(this);
                pooledObject.transform.SetParent(transform);
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.rotation = Quaternion.identity;
                pooledObject.GetComponent<Rigidbody>().isKinematic = true;
                _availableObjects.Push(pooledObject);
            }
        }

        public override APooledObject GetObject(Vector3 position = default, Vector3 scale = default)
        {
            APooledObject objectToGet = default(APooledObject);
            if (_availableObjects.Count > 0)
            {
                objectToGet = _availableObjects.Pop();
            }
            else
            {
                var pooledObject = Instantiate(_pooledObjectPrefab);
                pooledObject.Init(this);
                objectToGet = pooledObject;
            }

            objectToGet.transform.SetParent(null);
            objectToGet.transform.position = position;
            objectToGet.transform.rotation = Quaternion.identity;
            objectToGet.transform.localScale = scale;
            objectToGet.gameObject.SetActive(true);
            objectToGet.GetComponent<Rigidbody>().isKinematic = false;

            return objectToGet;
        }

        public override void ReturnObject(APooledObject objectToReturn)
        {
            var pooledObject = objectToReturn.GetComponent<PooledObject>();
            if (pooledObject != null)
            {
                objectToReturn.transform.SetParent(transform);
                objectToReturn.gameObject.SetActive(false);
                objectToReturn.GetComponent<Rigidbody>().isKinematic = true;

                _availableObjects.Push(objectToReturn);
            }
            else
            {
                Destroy(objectToReturn);
            }
        }

        private void OnDestroy()
        {
            if (_availableObjects != null)
            {
                _availableObjects.Clear();
                _availableObjects = null;
            }
        }
    }
}