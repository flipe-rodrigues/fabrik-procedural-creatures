using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(TargetBhv))]
    public class TargetGizmoBhv : GizmoBhv
    {
        // Public properties
        public override bool IsActive { get => this.Target.isActiveAndEnabled; }

        // Private properties
        private TargetBhv Target
        {
            get
            {
                if (_target == null)
                {
                    _target = this.GetComponent<TargetBhv>();
                }

                return _target;
            }
        }

        // Private fields
        private TargetBhv _target;

        private void Awake()
        {
            _target = this.Target;
        }

        public override void OnDrawGizmos()
        {
            if (this.IsActive)
            {
                Gizmos.color = this.WireColor;

                Gizmos.DrawLine(this.Target.Position, this.Target.EffectivePosition);

                if (this.Target.Effector != null)
                {
                    Gizmos.DrawLine(this.Target.Effector.Position, this.Target.EffectivePosition);
                }
            }

            base.OnDrawGizmos();
        }
    }
}
