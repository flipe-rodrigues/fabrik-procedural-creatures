using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(TargetBhv))]
    public class TargetGizmoBhv : GizmoBhv
    {
        // Serialized fields
        [SerializeField, Range(0, 1)] private float _deadZoneAlphaMultiplier = .5f;

        // Public properties
        public override bool IsActive { get => this.Target.IsActive; }

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

            Gizmos.matrix = Matrix4x4.TRS(this.Position, this.Rotation, this.Target.DeadZoneRadius * 2 * Vector3.one);

            Gizmos.color = this.FaceColor.SetAlpha(this.FaceColor.a * _deadZoneAlphaMultiplier);

            Gizmos.DrawMesh(this.Mesh);

            Gizmos.color = this.WireColor.SetAlpha(this.WireColor.a * _deadZoneAlphaMultiplier);

            Gizmos.DrawWireMesh(this.Mesh);
        }
    }
}
