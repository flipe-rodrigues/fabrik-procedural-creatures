using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public AttractorBhv contralateralAttractor;

        // Public properties
        public TargetBhv Target => _target;
        public bool ProjectOntoTerrain => _projectOntoTerrain;
        public bool IsAttracting => _lerp < 1;
        public Vector3 EffectivePosition { get => _effectivePosition; }

        // Private properties
        private bool IsGrounded => true; //this.IK.IsGrounded && (contralateralAttractor == null ? true : !contralateralAttractor.IsAttracting);

        // Private fields
        private Vector3 _effectivePosition = Vector3.zero;
        private Vector3 _currentPosition = Vector3.zero;
        private Vector3 _previousPosition = Vector3.zero;
        private Vector3 _currentForward= Vector3.forward;
        private Vector3 _previousForward = Vector3.forward;
        private float _lerp = Mathf.Infinity;

#if UNITY_EDITOR
        private void OnEnable()
        {
            EditorApplication.update += this.Update;
        }

        private void OnDisable()
        {
            EditorApplication.update -= this.Update;
        }
#endif
        private void OnValidate()
        {
            if (contralateralAttractor != null && contralateralAttractor.contralateralAttractor == null)
            {
                contralateralAttractor.contralateralAttractor = this;
            }
        }

        private void Start()
        {
            _currentPosition = _target.Position;

            _previousPosition = _target.Position;

            _previousForward = _target.Forward;

            _currentForward = _target.Forward;
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
            if (Vector3.Distance(_target.Position, _effectivePosition) >= _target.DeadZoneRadius && _lerp >= 1 && this.IsGrounded)
            {
                _lerp = 0;

                _previousPosition = _target.Position;

                _previousForward = _target.Forward;
            }

            if (_lerp < 1)
            {
                _lerp = Mathf.Clamp01(_lerp + Time.deltaTime * _lerpSpeed);

                _currentPosition = Vector3.Lerp(_previousPosition, _effectivePosition, _lerpCurve.Evaluate(_lerp));

                if (_projectOntoTerrain)
                {
                    _currentPosition.y += _heightCurve.Evaluate(_lerp);
                }

                _currentForward = Vector3.Lerp(_previousForward, this.Forward, _lerpCurve.Evaluate(_lerp));
            }

            if (Selection.Contains(_target.gameObject))
            {
                this.Start();
            }

            _target.Position = _currentPosition;

            _target.Forward = _currentForward;
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