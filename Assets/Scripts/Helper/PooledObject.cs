using UnityEngine;
using System.Collections;

namespace StackGame
{
    public class PooledObject : MonoBehaviour
    {
        private WaitForSeconds _delay = new WaitForSeconds(DESTROY_DELAY);

        private const float DESTROY_DELAY = 5.0f;

        public APool Pool { get; private set; }

        public void Init(APool pool)
        {
            Pool = pool;
        }

        private void OnEnable()
        {
            StartCoroutine(AutoDestroy());
        }

        private IEnumerator AutoDestroy()
        {
            yield return _delay;
            Disable();
        }

        private void Disable()
        {
            Pool.ReturnObject(this.gameObject);
        }
    }
}