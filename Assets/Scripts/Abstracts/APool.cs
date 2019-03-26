using UnityEngine;

namespace StackGame
{
    public abstract class APool : MonoBehaviour
    {
        [SerializeField] protected APooledObject _pooledObjectPrefab = null;
        [SerializeField] protected int _initialAmount = 0;

        public abstract APooledObject GetObject(Vector3 position = default, Vector3 scale = default);
        public abstract void ReturnObject(APooledObject objectToReturn);
    }
}