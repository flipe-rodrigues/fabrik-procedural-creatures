using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [SelectionBase]
    [ExecuteInEditMode]
    public class JointBhv : ChainElementBhv
    {
        // Public properties
        public ChainBhv[] ImmediateChildChains
        {
            get
            {
                if (_immediateChildChains == null || _immediateChildChains.Length == 0)
                {
                    _immediateChildChains = this.GetComponentsInImmediateChildren<ChainBhv>();
                }

                return _immediateChildChains;
            }
        }
        public JointBhv[] ImmediateChildJoints
        {
            get
            {
                if (_immediateChildJoints == null || _immediateChildJoints.Length == 0)
                {
                    _immediateChildJoints = this.ImmediateChildChains.Select(chain => chain.Joints[0]).Union(this.GetComponentsInImmediateChildren<JointBhv>()).ToArray();
                }

                return _immediateChildJoints;
            }
        }
        public JointBhv FirstParentJoint
        {
            get
            {
                if (_firstParentJoint == null)
                {
                    if (this.GetComponentsInParentsExclusively<JointBhv>().Length > 0)
                    {
                        _firstParentJoint = this.GetComponentsInParentsExclusively<JointBhv>().First();
                    }
                }

                return _firstParentJoint;
            }
        }
        public Vector3 TentativePosition { get => _tentativePosition; set => _tentativePosition = value; }
        public Vector3 TentativeUpDirection { get => _tentativeUpDirection; set => _tentativeUpDirection = value; }

        // Private fields
        [SerializeField, ReadOnly] private ChainBhv[] _immediateChildChains;
        [SerializeField, ReadOnly] private JointBhv[] _immediateChildJoints;
        private JointBhv _firstParentJoint;
        private Vector3 _tentativePosition;
        private Vector3 _tentativeUpDirection;
        private int _parentJointCount;
        private int _childJointCount;
        private int _immediateChildChainCount;

        public override void PseudoConstructor(ChainBhv chain, int index)
        {
            base.PseudoConstructor(chain, index);

            this.UpdateName();

            this.Transform.SetAsFirstSibling();
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                _immediateChildChains = this.ImmediateChildChains;

                _immediateChildJoints = this.ImmediateChildJoints;

                _firstParentJoint = this.FirstParentJoint;
            }
        }

        public override void UpdateName()
        {
            this.name = "Joint " + "(" + this.Index + " - " + this.Chain.name + ")";
        }
    }
}