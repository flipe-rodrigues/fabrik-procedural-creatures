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
        public bool debugging = false;
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
        public bool IsGrounded => _attractors.Where(attractor => attractor.ProjectOntoTerrain).All(attractor => !attractor.IsAttracting);

        // Private properties
        private ChainBhv[] Chains
        {
            get
            {
                if (!Application.isPlaying || _chains == null || _chains.Length == 0)
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
                if (!Application.isPlaying || _joints == null || _chains.Length == 0)
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
                if (!Application.isPlaying || _targets == null || _targets.Length == 0)
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
                if (!Application.isPlaying || _attractors == null || _attractors.Length == 0)
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

        // Private fields
        [SerializeField, ReadOnly] private ChainBhv[] _chains;
        [SerializeField, ReadOnly] private JointBhv[] _joints;
        [SerializeField, ReadOnly] private TargetBhv[] _targets;
        [SerializeField, ReadOnly] private AttractorBhv[] _attractors;

        private void Awake()
        {
            _chains = this.Chains;

            _joints = this.Joints;

            _targets = this.Targets;

            _attractors = this.Attractors;
        }

        private void Start()
        {
            StopAllCoroutines();

            if (debugging)
            {
                StartCoroutine(this.FRIK_Coroutine());
            }
        }

        private void LateUpdate()
        {
            if (!debugging)
            {
                this.FRIK();
            }
        }

        private void FRIK()
        {
            this.SnapshotIK();

            this.ResolveIK();

            if (_snapToTerrain)
            {
                this.SnapIKToTerrain();
            }

            this.ApplyIK();
        }

        private IEnumerator FRIK_Coroutine()
        {
            while (true)
            {
                this.SnapshotIK();

                //foreach (TargetBhv target in this.Targets.Where(target => target.IsActive).OrderBy(target => target.Priority))
                //{
                //    for (int i = 0; i < (target.Priority + 1) * 15; i++)
                //        this.ResolveIK(target.Effector, target);

                //    this.ApplyIK();

                //    yield return new WaitForSeconds(.5f);
                //}

                yield return new WaitForSeconds(1f);
            }
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
                foreach (TargetBhv target in this.Targets.Where(target => target.IsActive && target.Effector != null).OrderByDescending(target => target.Effector.Chain.Depth))
                {
                    JointBhv effector = target.Effector;

                    effector.TentativePosition = target.EffectivePosition;

                    effector.PropagateUpstream();
                }

                foreach (JointBhv intersection in this.Joints.Where(joint => joint.Type == JointType.Intersection))
                {
                    intersection.TentativePosition = intersection.DownstreamJoints.Select(joint => joint.TentativePosition).Mean();

                    intersection.PropagateUpstream();
                }

                foreach (ChainBhv chain in this.Chains.OrderBy(chain => chain.Depth))
                {
                    JointBhv firstJoint = chain.Joints.First();

                    if (firstJoint.Type == JointType.Root)
                    {
                        if (_rootIsFixed)
                        {
                            firstJoint.TentativePosition = firstJoint.Position;
                        }
                    }
                    else
                    {
                        firstJoint.TentativePosition = firstJoint.UpstreamJoints.First().TentativePosition;
                    }

                    firstJoint.PropagateDownstream();
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