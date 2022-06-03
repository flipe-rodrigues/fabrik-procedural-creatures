using System;
using UnityEngine;

namespace ProceduralAnimation
{
    [Serializable]
    public class Curve
    {
        // Public fields
        [SerializeField] private AnimationCurve _curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        [SerializeField] private float _offset = 0;
        [SerializeField] private float _gain = 1;

        public Vector3 GetScale(float time)
        {
            float value = _curve.Evaluate(time) * _gain + _offset;

            Vector3 scale = value * Vector3.one;

            return scale;
        }
    }
}
