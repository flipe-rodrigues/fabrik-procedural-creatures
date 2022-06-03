using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class InverseKinematicsManager : Singleton<InverseKinematicsManager>
    {
        // Public properties
        public List<JointBhv> QueuedJoints { get => _queuedJoints; set => _queuedJoints = value; }

        // Private fields
        [SerializeField] private List<JointBhv> _queuedJoints = new List<JointBhv>();

        private void LateUpdate()
        {
            //this.QueuedJoints = this.QueuedJoints.OrderBy(j => j.Target.Priority).ToList();

            foreach (JointBhv joint in this.QueuedJoints)
            {
                //this.SnapshotIK(joint);

                this.ResolveIK(joint);

                //this.ApplyIK(joint);
            }

            this.QueuedJoints.Clear();
        }

        private void SnapshotIK(JointBhv joint)
        {
            for (int i = 0; i < joint.Chain.Joints.Length; i++)
            {
                joint.Chain.Joints[i].TentativePosition = joint.Chain.Joints[i].Position;
            }
        }

        private void ResolveIK(JointBhv joint)
        {
            //for (int i = 0; i < joint.Chain.Joints.Length; i++)
            //{
            //    joint.Chain.Joints[i].TentativePosition = joint.Chain.Joints[i].Position;
            //}

            //joint.TentativePosition = joint.Target.Position;

            //for (int i = joint.Index - 1; i >= 0; i--)
            //{
            //    Vector3 direction = (joint.Chain.Joints[i + 1].TentativePosition - joint.Chain.Joints[i].TentativePosition).normalized;

            //    joint.Chain.Joints[i].TentativePosition = joint.Chain.Joints[i + 1].TentativePosition - joint.Chain.Links[i].Length * direction;
            //}

            //for (int i = joint.Index + 1; i < joint.Chain.Joints.Length; i++)
            //{
            //    Vector3 direction = (joint.Chain.Joints[i].TentativePosition - joint.Chain.Joints[i - 1].TentativePosition).normalized;

            //    joint.Chain.Joints[i].TentativePosition = joint.Chain.Joints[i - 1].TentativePosition + joint.Chain.Links[i - 1].Length * direction;
            //}

            //for (int i = 0; i < joint.Chain.Joints.Length; i++)
            //{
            //    // rotation
            //    Vector3 forward, up;

            //    forward = i == joint.Chain.Joints.Length - 1 ? joint.Chain.Joints[i].Forward : (joint.Chain.Joints[i + 1].TentativePosition - joint.Chain.Joints[i].TentativePosition).normalized;

            //    up = Vector3.up;

            //    joint.Chain.Joints[i].Rotation = Quaternion.LookRotation(forward, up);

            //    // position
            //    joint.Chain.Joints[i].Position = joint.Chain.Joints[i].TentativePosition;
            //}
        }

        private void ApplyIK(JointBhv joint)
        {
            for (int i = 0; i < joint.Chain.Joints.Length; i++)
            {
                // rotation
                Vector3 forward, up;

                forward = i == joint.Chain.Joints.Length - 1 ? joint.Chain.Joints[i].Forward : (joint.Chain.Joints[i + 1].TentativePosition - joint.Chain.Joints[i].TentativePosition).normalized;

                up = Vector3.up;

                joint.Chain.Joints[i].Rotation = Quaternion.LookRotation(forward, up);

                // position
                joint.Chain.Joints[i].Position = joint.Chain.Joints[i].TentativePosition;
            }
        }
    }
}
