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
        // Serialized fields
        [SerializeField, ReadOnly] private JointBhv[] _upstreamJoints;
        [SerializeField, ReadOnly] private JointBhv[] _downstreamJoints;
        [SerializeField, ReadOnly] private int _depth;
        [SerializeField, ReadOnly] private bool _isRoot;
        [SerializeField, ReadOnly] private bool _isIntersection;
        [SerializeField, ReadOnly] private bool _isBranch;
        [SerializeField, ReadOnly] private bool _isLeaf;

        // Public properties
        public JointBhv[] UpstreamJoints
        {
            get
            {
                if (_upstreamJoints == null || _upstreamJoints.Length == 0)
                {
                    if (this.GetComponentsInParentsExclusively<JointBhv>().Length > 0)
                    {
                        _upstreamJoints = this.GetComponentsInParentsExclusively<JointBhv>().Where(joint => Mathf.Abs(this.Depth - joint.Depth) == 1).ToArray();
                    }
                }

                return _upstreamJoints;
            }
        }
        public JointBhv[] DownstreamJoints
        {
            get
            {
                if (_downstreamJoints == null || _downstreamJoints.Length == 0)
                {
                    if (this.GetComponentsInChildrenExclusively<JointBhv>().Length > 0)
                    {
                        _downstreamJoints = this.GetComponentsInChildrenExclusively<JointBhv>().Where(joint => Mathf.Abs(this.Depth - joint.Depth) == 1).ToArray();
                    }
                }

                return _downstreamJoints;
            }
        }
        public LinkBhv Link => this.Index < this.Chain.Links.Length ? this.Chain.Links[this.Index] : null;
        public Vector3 TentativePosition { get => _tentativePosition; set => _tentativePosition = value; }
        public Quaternion TentativeRotation { get => _tentativeRotation; set => _tentativeRotation = value; }
        public Vector3 TentativeUpDirection { get => _tentativeUpDirection; set => _tentativeUpDirection = value; }
        public int Depth => _depth;
        public bool IsRoot => _isRoot;
        public bool IsIntersection => _isIntersection;
        public bool IsBranch => _isBranch;
        public bool IsLeaf => _isLeaf;

        // Private properties
        private bool HasLink => this.Link != null;

        // Private fields
        private Vector3 _tentativePosition;
        private Quaternion _tentativeRotation;
        private Vector3 _tentativeUpDirection;

        public override void PseudoConstructor(ChainBhv chain, int index)
        {
            base.PseudoConstructor(chain, index);

            this.UpdateName();

            this.Transform.SetAsFirstSibling();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!Application.isPlaying)
            {
                _depth = this.GetComponentsInParentsExclusively<JointBhv>().Length;

                this.UpdateType();
            }
        }

        private void UpdateType()
        {
            _isRoot = this.UpstreamJoints.Length == 0;

            _isIntersection = this.DownstreamJoints.Any(joint => joint.Chain != this.Chain);

            _isBranch = this.UpstreamJoints.Any(joint => joint.Chain != this.Chain);

            _isLeaf = this.DownstreamJoints.Length == 0;
        }

        public override void UpdateName()
        {
            string prefix = "";

            prefix += this.IsRoot ? "Root " : "";

            prefix += this.IsIntersection ? "Intersection " : "";

            prefix += this.IsBranch ? "Branch " : "";

            prefix += this.IsLeaf ? "Leaf " : "";

            this.name = prefix + "Joint " + "(" + this.Chain.name + " - " + this.Index + ")";
        }

        public void PropagateUpstream()
        {
            foreach (JointBhv upstreamJoint in _upstreamJoints)
            {
                if (upstreamJoint.IsIntersection)
                {
                    upstreamJoint.TentativePosition = upstreamJoint.DownstreamJoints.AverageTentativePosition();
                }
                else
                {
                    if (upstreamJoint.HasLink)
                    {
                        Vector3 direction = (upstreamJoint.TentativePosition - this.TentativePosition).normalized;

                        upstreamJoint.TentativePosition = this.TentativePosition + upstreamJoint.Link.Length * direction;
                    }
                }

                upstreamJoint.PropagateUpstream();
            }
        }

        public void PropagateDownstream()
        {
            foreach (JointBhv downstreamJoint in _downstreamJoints)
            {
                if (downstreamJoint.IsBranch)
                {
                    downstreamJoint.TentativePosition = downstreamJoint.UpstreamJoints[0].TentativePosition;
                }
                else
                {
                    if (this.HasLink)
                    {
                        Vector3 direction = (downstreamJoint.TentativePosition - this.TentativePosition).normalized;

                        downstreamJoint.TentativePosition = this.TentativePosition + this.Link.Length * direction;
                    }
                }

                downstreamJoint.PropagateDownstream();
            }
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            this.Transform.SetPositionAndRotation(position, rotation);
        }
    }
}