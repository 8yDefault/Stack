using UnityEngine;

namespace StackGame
{
    public abstract class APool : MonoBehaviour
    {
        public abstract GameObject GetObject(Vector3 position = default, Vector3 scale = default);
        public abstract void ReturnObject(GameObject objectToReturn);
    }
}