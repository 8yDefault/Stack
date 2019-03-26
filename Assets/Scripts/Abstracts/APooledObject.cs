using UnityEngine;

namespace StackGame
{
    public abstract class APooledObject : MonoBehaviour
    {
        public APool Pool { get; private set; }

        public void Init(APool pool)
        {
            Pool = pool;
        }

        protected void Disable()
        {
            Pool.ReturnObject(this);
        }
    }
}