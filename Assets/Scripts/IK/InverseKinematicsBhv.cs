using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
                if (_chains == null || _chains.Length == 0)
                {
                    _chains = this.GetComponentsInChildren<ChainBhv>();
                }

                return _chains;
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

        // Private fields
        [SerializeField, ReadOnly] private ChainBhv[] _chains;
        [SerializeField, ReadOnly] private TargetBhv[] _targets;
        [SerializeField, ReadOnly] private AttractorBhv[] _attractors;
        [SerializeField, ReadOnly] private bool _isGrounded;

        private void Awake()
        {
            _chains = this.Chains;

            _targets = this.Targets;

            _attractors = this.Attractors;
        }

        private void Update()
        {
            _isGrounded = this.IsGrounded;
        }

        private void LateUpdate()
        {
            this.BRIK();
        }

        private void BRIK()
        {
            foreach (ChainBhv chain in this.Chains.OrderByDescending(chain => chain.Depth))
            {
                this.SnapshotIK(chain);
            }

            foreach (TargetBhv target in this.Targets.Where(target => target.IsActive).OrderBy(target => target.Priority))
            {
                foreach (JointBhv effector in target.Effectors)
                {
                    if (effector == null)
                    {
                        continue;
                    }

                    this.ResolveIK(effector, target);
                }
            }

            foreach (ChainBhv chain in this.Chains.OrderByDescending(chain => chain.Depth))
            {
                if (_snapToTerrain)
                {
                    this.SnapIKToTerrain(chain);
                }

                this.ApplyIK(chain);
            }

            return;
            //foreach (ChainBhv chain in this.Chains.OrderByDescending(chain => chain.Depth))
            //{
            //    this.SnapshotIK(chain);

            //    foreach (TargetBhv target in chain.Targets.Where(target => target.IsActive).OrderBy(target => target.Priority))
            //    {
            //        this.ResolveIK(chain, target);
            //    }

            //    if (_snapToTerrain)
            //    {
            //        this.SnapIKToTerrain(chain);
            //    }

            //    this.ApplyIK(chain);
            //}
        }

        private void SnapshotIK(ChainBhv chain)
        {
            for (int i = 0; i < chain.Joints.Length; i++)
            {
                chain.Joints[i].TentativePosition = chain.Joints[i].Position;
            }
        }

        private void ResolveIK(JointBhv effector, TargetBhv target)
        {
            effector.TentativePosition = target.EffectivePosition;

            for (int i = effector.Index - 1; i >= 0; i--)
            {
                Vector3 direction = (effector.Chain.Joints[i + 1].TentativePosition - effector.Chain.Joints[i].TentativePosition).normalized;

                effector.Chain.Joints[i].TentativePosition = effector.Chain.Joints[i + 1].TentativePosition - effector.Chain.Links[i].Length * direction;
            }

            for (int i = effector.Index + 1; i < effector.Chain.Joints.Length; i++)
            {
                Vector3 direction = (effector.Chain.Joints[i].TentativePosition - effector.Chain.Joints[i - 1].TentativePosition).normalized;

                effector.Chain.Joints[i].TentativePosition = effector.Chain.Joints[i - 1].TentativePosition + effector.Chain.Links[i - 1].Length * direction;
            }
        }

        private void SnapIKToTerrain(ChainBhv chain)
        {
            for (int i = 0; i < chain.Joints.Length; i++)
            {
                if (Physics.Raycast(chain.Joints[i].Position + 100 * Vector3.up, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, _terrainLayerMask))
                {
                    chain.Joints[i].TentativePosition = new Vector3(chain.Joints[i].TentativePosition.x, hitInfo.point.y + _terrainOffset, chain.Joints[i].TentativePosition.z);

                    chain.Joints[i].TentativeUpDirection = hitInfo.normal;
                }
            }
        }

        private void ApplyIK(ChainBhv chain)
        {
            for (int i = 0; i < chain.Joints.Length; i++)
            {
                Vector3 forward, up;

                if (i == chain.Joints.Length - 1)
                {
                    forward = chain.Joints[i - 1].Forward;
                }
                else
                {
                    forward = (chain.Joints[i + 1].TentativePosition - chain.Joints[i].TentativePosition).normalized;
                }

                up = _snapToTerrain ? chain.Joints[i].TentativeUpDirection : Vector3.up;

                chain.Joints[i].Rotation = Quaternion.LookRotation(forward, up);

                chain.Joints[i].Position = chain.Joints[i].TentativePosition;
            }
        }
    }
}