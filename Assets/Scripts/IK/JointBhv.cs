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
        [SerializeField] private LinkBhv _link;
        [SerializeField, ReadOnly] private int _depth;

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
        public LinkBhv Link => _link; // this.Chain.Links[this.Index];
        public Vector3 TentativePosition { get => _tentativePosition; set => _tentativePosition = value; }
        public Vector3 TentativeUpDirection { get => _tentativeUpDirection; set => _tentativeUpDirection = value; }
        private bool HasLink => _link != null;
        public int Depth => _depth;

        // Private fields
        private Vector3 _tentativePosition;
        private Vector3 _tentativeUpDirection;

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
                _depth = this.GetComponentsInParentsExclusively<JointBhv>().Length;

                _upstreamJoints = this.UpstreamJoints;

                _downstreamJoints = this.DownstreamJoints;

                //_link = this.Chain.Links[this.Index];
            }
        }

        private void UpdatePosition()
        {
            //this.Position = 
        }

        public override void UpdateName()
        {
            this.name = "Joint " + "(" + this.Index + " - " + this.Chain.name + ")";
        }

        public void PropagateUpstream(JointBhv source)
        {
            foreach (JointBhv upstreamJoint in this.UpstreamJoints)
            {
                if (!upstreamJoint.HasLink)
                {
                    continue;
                }

                Vector3 direction = (upstreamJoint.TentativePosition - this.TentativePosition).normalized;

                upstreamJoint.TentativePosition = this.TentativePosition + upstreamJoint.Link.Length * direction;

                upstreamJoint.PropagateUpstream(this);

                foreach (JointBhv downstreamJoint in upstreamJoint.DownstreamJoints)
                {
                    if (downstreamJoint != source)
                    {
                        upstreamJoint.PropagateDownstream(this);
                    }
                }
            }
        }

        public void PropagateDownstream(JointBhv source)
        {
            foreach (JointBhv downstreamJoint in this.DownstreamJoints)
            {
                if (!this.HasLink)
                {
                    continue;
                }

                Vector3 direction = (downstreamJoint.TentativePosition - this.TentativePosition).normalized;

                downstreamJoint.TentativePosition = this.TentativePosition + this.Link.Length * direction;

                downstreamJoint.PropagateDownstream(this);
            }
        }
    }
}