using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Jobs;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class InverseKinematicsBhv : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private bool _rootIsFixed = true;
        [SerializeField] private LayerMask _terrainLayerMask = 0;
        [SerializeField] private bool _snapToTerrain = false;
        [SerializeField, Min(0)] private float _terrainOffset = 0;
        [SerializeField, Range(1, 50)] private int _maxIterations = 15;
        [SerializeField, Min(1e-4f)] private float _tolerance = 1e-4f;

        // Public properties
        public bool SnapToTerrain => _snapToTerrain;
        public LayerMask TerrainLayerMask => _terrainLayerMask;
        public float TerrainOffset => _terrainOffset;
        public bool IsGrounded => _attractors.Where(attractor => attractor.ProjectOntoTerrain).Any(attractor => !attractor.IsAttracting);

        // Private properties
        private ChainBhv[] Chains
        {
            get
            {
                if (_chains == null || _chains.Length == 0)
                {
                    _chains = this.GetComponentsInChildren<ChainBhv>();
                }

                return _chains;
            }
        }
        private JointBhv[] Joints
        {
            get
            {
                if (_joints == null || _chains.Length == 0)
                {
                    _joints = this.GetComponentsInChildren<JointBhv>();
                }

                return _joints;
            }
        }
        public TargetBhv[] Targets
        {
            get
            {
                if (_targets == null || _targets.Length == 0)
                {
                    _targets = this.GetComponentsInChildren<TargetBhv>();
                }

                return _targets;
            }
            set
            {
                _targets = value;
            }
        }
        public AttractorBhv[] Attractors
        {
            get
            {
                if (_attractors == null || _attractors.Length == 0)
                {
                    _attractors = this.GetComponentsInChildren<AttractorBhv>();
                }

                return _attractors;
            }
            set
            {
                _attractors = value;
            }
        }
        public HintBhv[] Hints
        {
            get
            {
                if (_hints == null || _hints.Length == 0)
                {
                    _hints = this.GetComponentsInChildren<HintBhv>();
                }

                return _hints;
            }
            set
            {
                _hints = value;
            }
        }

        // Private fields
        [SerializeField, ReadOnly] private ChainBhv[] _chains;
        [SerializeField, ReadOnly] private JointBhv[] _joints;
        [SerializeField, ReadOnly] private TargetBhv[] _targets;
        [SerializeField, ReadOnly] private AttractorBhv[] _attractors;
        [SerializeField, ReadOnly] private HintBhv[] _hints;

        public void ResetIKElements()
        {
            _chains = null;

            _joints = null;

            _targets = null;

            _attractors = null;

            _hints = null;

            this.Start();
        }

        private void Start()
        {
            _chains = this.Chains;

            _joints = this.Joints;

            _targets = this.Targets;

            _attractors = this.Attractors;

            _hints = this.Hints;
        }

        private void LateUpdate()
        {
            this.FRIK();
        }

        private void FRIK()
        {
            this.SnapshotIK();

            this.ResolveIK();

            this.BendIK();

            if (_snapToTerrain)
            {
                this.SnapIKToTerrain();
            }

            this.ApplyIK();
        }

        private void SnapshotIK()
        {
            foreach (JointBhv joint in this.Joints)
            {
                joint.TentativePosition = joint.Position;
            }
        }

        private void ResolveIK()
        {
            for (int i = 0; i < _maxIterations; i++)
            {
                //foreach (TargetBhv target in this.Targets.Where(target => target.IsActive && target.Effector != null).OrderByDescending(target => target.Effector.Chain.Depth))
                foreach (TargetBhv target in this.Targets.Where(target => target.isActiveAndEnabled && target.Effector != null).OrderBy(target => target.Priority))
                {
                    JointBhv effector = target.Effector;

                    effector.TentativePosition = target.EffectivePosition;

                    effector.PropagateUpstream();
                }

                JointBhv root = this.Joints.Where(joint => joint.IsRoot).First();

                if (_rootIsFixed)
                {
                    root.TentativePosition = root.Position;
                }

                root.PropagateDownstream();
            }
        }

        private void BendIK()
        {
            foreach (HintBhv hint in this.Hints.Where(hint => hint.isActiveAndEnabled && hint.Chain != null))
            {
                for (int i = 1; i < hint.Chain.Joints.Length - 1; i++)
                {
                    Vector3 planeNormal = (hint.Chain.Joints[i + 1].TentativePosition - hint.Chain.Joints[i - 1].TentativePosition).normalized;

                    Plane plane = new Plane(planeNormal, hint.Chain.Joints[i - 1].TentativePosition);

                    Vector3 hintProjection = plane.ClosestPointOnPlane(hint.EffectivePosition);

                    Vector3 jointProjection = plane.ClosestPointOnPlane(hint.Chain.Joints[i].TentativePosition);

                    float angle = Vector3.SignedAngle(jointProjection - hint.Chain.Joints[i - 1].TentativePosition, hintProjection - hint.Chain.Joints[i - 1].TentativePosition, planeNormal);

                    hint.Chain.Joints[i].TentativePosition = Quaternion.AngleAxis(angle, planeNormal) * (hint.Chain.Joints[i].TentativePosition - hint.Chain.Joints[i - 1].TentativePosition) + hint.Chain.Joints[i - 1].TentativePosition;
                }
            }
        }


        private void SnapIKToTerrain()
        {
            foreach (JointBhv joint in this.Joints)
            {
                if (Physics.Raycast(joint.Position + 100 * Vector3.up, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, _terrainLayerMask))
                {
                    joint.TentativePosition = new Vector3(joint.TentativePosition.x, hitInfo.point.y + _terrainOffset, joint.TentativePosition.z);

                    joint.TentativeUpDirection = hitInfo.normal;
                }
            }
        }

        private void ApplyIK()
        {
            foreach (JointBhv joint in this.Joints)
            {
                Vector3 forward, up;

                forward = (joint.DownstreamJoints.Select(joint => joint.TentativePosition).Mean() - joint.TentativePosition).normalized;

                up = _snapToTerrain ? joint.TentativeUpDirection : Vector3.up;

                joint.Rotation = Quaternion.LookRotation(forward, up);

                joint.Position = joint.TentativePosition;
            }
        }
    }
}