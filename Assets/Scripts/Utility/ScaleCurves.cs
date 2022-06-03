using System;
using UnityEngine;

namespace ProceduralAnimation
{
    [Serializable]
    public class ScaleCurves
    {
        // Public fields
        [SerializeField] private AnimationCurve _scaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        [SerializeField] private AnimationCurve _xScaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        [SerializeField] private AnimationCurve _yScaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        [SerializeField] private AnimationCurve _zScaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        [SerializeField] private Vector3 _curveOffsets = Vector3.zero;
        [SerializeField] private Vector3 _curveGains = Vector3.one;
        [SerializeField] private float _curveOffset = 0;
        [SerializeField] private float _curveGain = 1;
        [SerializeField] private bool _separateAxes = true;

        public Vector3 GetScale(float time)
        {
            Vector3 scale;

            if (_separateAxes)
            {
                float x = _xScaleCurve.Evaluate(time) * _curveGains.x + _curveOffsets.x;

                float y = _yScaleCurve.Evaluate(time) * _curveGains.y + _curveOffsets.y;

                float z = _zScaleCurve.Evaluate(time) * _curveGains.z + _curveOffsets.z;

                scale = new Vector3(x, y, z);
            }
            else
            {
                float s = _scaleCurve.Evaluate(time) * _curveGain + _curveOffset;

                scale = s * Vector3.one;
            }

            return scale;
        }
    }
}
