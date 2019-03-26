using UnityEngine;
using System.Collections.Generic;

namespace StackGame
{
    public class Pool : APool
    {
        private Stack<GameObject> _availableObjects = null;

        private void Awake()
        {
            var poolObjectsCount = transform.childCount;
            _availableObjects = new Stack<GameObject>(poolObjectsCount);

            for (int i = 0; i < poolObjectsCount; i++)
            {
                var newObject = transform.GetChild(i).gameObject;
                newObject.transform.SetParent(transform);
                newObject.gameObject.SetActive(false);
                newObject.GetComponent<Rigidbody>().isKinematic = true;
                var pooledObject = newObject.AddComponent<PooledObject>();
                pooledObject.Init(this);
                newObject = pooledObject.gameObject;

                _availableObjects.Push(newObject);
            }
        }

        public override GameObject GetObject(Vector3 position = default, Vector3 scale = default)
        {
            GameObject objectToGet = default(GameObject);
            if (_availableObjects.Count > 0)
            {
                objectToGet = _availableObjects.Pop();
            }
            else
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<Rigidbody>();
                var pooledObject = go.AddComponent<PooledObject>();
                pooledObject.Init(this);
                objectToGet = pooledObject.gameObject;
            }

            objectToGet.transform.SetParent(null);
            objectToGet.transform.position = position;
            objectToGet.transform.rotation = Quaternion.identity;
            objectToGet.transform.localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            objectToGet.gameObject.SetActive(true);
            objectToGet.GetComponent<Rigidbody>().isKinematic = false;

            return objectToGet;
        }

        public override void ReturnObject(GameObject objectToReturn)
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