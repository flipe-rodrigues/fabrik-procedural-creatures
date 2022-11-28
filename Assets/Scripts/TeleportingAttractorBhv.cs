using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class TeleportingAttractorBhv : MonoBehaviour
    {
        public AttractorBhv attractor;

        private void Start()
        {
            StartCoroutine(TeleportCoroutine());
        }

        private IEnumerator TeleportCoroutine()
        {
            while (true)
            {
                Vector3 position = 10 * Random.insideUnitSphere;

                position.y = attractor.Position.y;

                attractor.Position = position;

                yield return new WaitForSeconds(5);
            }
        }
    }
}
