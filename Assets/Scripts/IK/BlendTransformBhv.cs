using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class BlendTransformBhv : CachedTransformBhv
    {
        [SerializeField] private Transform[] _transforms = new Transform[2];
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private Vector3 _weights = Vector3.one;

        private void Update()
        {
            if (_transforms == null || _transforms.Length == 0 || _transforms.Any(t => t == null))
            {
                return;
            }

            Vector3 averagePosition = _transforms.Select(t => t.position).Mean();

            this.Position = new Vector3(averagePosition.x * _weights.x, averagePosition.y * _weights.y, averagePosition.z * _weights.z); // + _offset;

            this.LocalPosition = _offset;
        }
    }
}
