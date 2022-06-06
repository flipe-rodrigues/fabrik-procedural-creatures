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
        [SerializeField] private LayerMask _terrainLayerMask = 0;
        [SerializeField] private bool _snapToTerrain = false;
        [SerializeField, Min(0)] private float _terrainOffset = 0;

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

        private void LateUpdate()
        {
            this.FRIK();
        }

        private void FRIK()
        {
            this.SnapshotIK();

            foreach (TargetBhv target in this.Targets.Where(target => target.IsActive).OrderBy(target => target.Priority))
            {
                this.ResolveIK(target.Effector, target);
            }

            foreach (JointBhv joint in this.Joints)
            {
                //
            }

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

        private void ResolveIK(JointBhv effector, TargetBhv target)
        {
            if (effector == null)
            {
                return;
            }

            effector.TentativePosition = target.EffectivePosition;

            effector.PropagateUpstream(effector);

            effector.PropagateDownstream(effector);
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

            //for (int i = 0; i < chain.Joints.Length; i++)
            //{
            //    Vector3 forward, up;

            //    if (i == chain.Joints.Length - 1)
            //    {
            //        forward = chain.Joints[i - 1].Forward;
            //    }
            //    else
            //    {
            //        forward = (chain.Joints[i + 1].TentativePosition - chain.Joints[i].TentativePosition).normalized;
            //    }

            //    up = _snapToTerrain ? chain.Joints[i].TentativeUpDirection : Vector3.up;

            //    chain.Joints[i].Rotation = Quaternion.LookRotation(forward, up);

            //    chain.Joints[i].Position = chain.Joints[i].TentativePosition;
            //}
        }
    }
}