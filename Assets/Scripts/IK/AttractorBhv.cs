using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [SelectionBase]
    [ExecuteInEditMode]
    public class AttractorBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField] private TargetBhv _target;
        [SerializeField] private bool _projectOntoTerrain;
        [SerializeField] private AnimationCurve _lerpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField] private AnimationCurve _heightCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.5f, .5f), new Keyframe(1, 0));
        [SerializeField] private float _lerpSpeed = 1;

        // Public properties
        public bool ProjectOntoTerrain => _projectOntoTerrain;
        public bool IsAttracting => _lerp < 1;

        // Private fields
        private Vector3 _effectivePosition;
        private Vector3 _currentPosition;
        private Vector3 _previousPosition;
        private Vector3 _currentLocalPosition;
        private float _lerp = Mathf.Infinity;

        private void OnEnable()
        {
            EditorApplication.update += this.Update;
        }
        private void OnDisable()
        {
            EditorApplication.update -= this.Update;
        }

        private void Start()
        {
            _currentPosition = _target.Position;

            _previousPosition = _target.Position;

            _currentLocalPosition = _target.LocalPosition;
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            this.UpdateEffectivePosition();

            this.UpdateAttraction();
        }

        private void UpdateEffectivePosition()
        {
            if (!_projectOntoTerrain)
            {
                _effectivePosition = this.Position;
            }

            else if (this.Raycast(out RaycastHit hitInfo))
            {
                _effectivePosition = hitInfo.point + this.IK.TerrainOffset * Vector3.up;
            }
        }

        private void UpdateAttraction()
        {
            if (Vector3.Distance(_target.Position, _effectivePosition) >= _target.DeadZoneRadius && _lerp >= 1 && this.IK.IsGrounded)
            {
                _lerp = 0;

                _previousPosition = _target.Position;
            }

            if (_lerp < 1)
            {
                _lerp = Mathf.Clamp01(_lerp + Time.deltaTime * _lerpSpeed);

                _currentPosition = Vector3.Lerp(_previousPosition, _effectivePosition, _lerpCurve.Evaluate(_lerp));

                if (_projectOntoTerrain)
                {
                    _currentPosition.y += _heightCurve.Evaluate(_lerp);
                }

                _currentLocalPosition = _target.Parent.InverseTransformPoint(_currentPosition);
            }

            _target.LocalPosition = _currentLocalPosition;

            _target.Forward = this.Forward;
        }

        public bool Raycast(out RaycastHit hitInfo)
        {
            return Physics.Raycast(this.Position, -this.Up, out hitInfo, Mathf.Infinity, this.IK.TerrainLayerMask.value);
        }

        public override void UpdateName()
        {
            string sufix = _target == null ? "________" : _target.name;

            this.name = "Attractor" + " < " + sufix;
        }
    }
}