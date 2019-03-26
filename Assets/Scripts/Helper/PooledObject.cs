using UnityEngine;
using System.Collections;

namespace StackGame
{
    public class PooledObject : APooledObject
    {
        // TODO: add autodestroy DECORATOR + create variety of pooled object

        private WaitForSeconds _delay = new WaitForSeconds(DESTROY_DELAY);

        private const float DESTROY_DELAY = 5.0f;

        private void OnEnable()
        {
            StartCoroutine(AutoDestroy());
        }

        private IEnumerator AutoDestroy()
        {
            yield return _delay;
            Disable();
        }
    }
}