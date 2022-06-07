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
        [SerializeField, ReadOnly] private JointType _type;
        [SerializeField, ReadOnly] private int _depth;
        [SerializeField, ReadOnly] private LinkBhv _link;

        // Public properties
        public JointBhv[] UpstreamJoints
        {
            get
            {
                if (!Application.isPlaying || _upstreamJoints == null || _upstreamJoints.Length == 0)
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
                if (!Application.isPlaying || _downstreamJoints == null || _downstreamJoints.Length == 0)
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
        public Vector3 TentativeUpDirection { get => _tentativeUpDirection; set => _tentativeUpDirection = value; }
        public JointType Type => _type;
        public int Depth => _depth;
        public bool IsRoot => this.UpstreamJoints.Length == 0;
        public bool IsIntersection => this.DownstreamJoints.Any(joint => joint.Chain != this.Chain);
        public bool IsBranch => this.UpstreamJoints.Any(joint => joint.Chain != this.Chain);
        public bool IsLeaf => this.DownstreamJoints.Length == 0;

        // Private properties
        private bool HasLink => this.Link != null;

        // Private fields
        private Vector3 _tentativePosition;
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

                _upstreamJoints = this.UpstreamJoints;

                _downstreamJoints = this.DownstreamJoints;

                this.UpdateType();

                _link = this.Link;
            }
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

        private void UpdateType()
        {
            if (this.UpstreamJoints.Length == 0)
            {
                _type = JointType.Root;
            }
            else if (this.DownstreamJoints.Any(joint => joint.Chain != this.Chain))
            {
                _type = JointType.Intersection;
            }
            else
            {
                if (this.DownstreamJoints.Length == 0)
                {
                    _type = JointType.Leaf;
                }
                else if (this.DownstreamJoints.Length == 1)
                {
                    _type = JointType.Regular;
                }
                else
                {
                    _type = JointType.Intersection;
                }
            }
        }

        public void PropagateUpstream()
        {
            foreach (JointBhv upstreamJoint in this.UpstreamJoints)
            {
                if (!upstreamJoint.HasLink) // || upstreamJoint.Chain != this.Chain)
                {
                    continue;
                }

                if (upstreamJoint.IsIntersection)
                {
                    upstreamJoint.TentativePosition = upstreamJoint.DownstreamJoints.Select(joint => joint.TentativePosition).Mean();
                }
                else
                {
                    Vector3 direction = (upstreamJoint.TentativePosition - this.TentativePosition).normalized;

                    upstreamJoint.TentativePosition = this.TentativePosition + upstreamJoint.Link.Length * direction;
                }

                upstreamJoint.PropagateUpstream();
            }
        }

        public void PropagateDownstream()
        {
            foreach (JointBhv downstreamJoint in this.DownstreamJoints)
            {
                if (!this.HasLink) // || downstreamJoint.Chain != this.Chain)
                {
                    continue;
                }

                if (downstreamJoint.IsBranch)
                {
                    downstreamJoint.TentativePosition = downstreamJoint.UpstreamJoints.First().TentativePosition;
                }
                else
                {
                    Vector3 direction = (downstreamJoint.TentativePosition - this.TentativePosition).normalized;

                    downstreamJoint.TentativePosition = this.TentativePosition + this.Link.Length * direction;
                }

                downstreamJoint.PropagateDownstream();
            }
        }
    }
}