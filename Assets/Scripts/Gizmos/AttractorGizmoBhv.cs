using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(AttractorBhv))]
    public class AttractorGizmoBhv : GizmoBhv
    {
        // Serialized fields
        [SerializeField, Range(0, 1)] private float _deadZoneAlphaMultiplier = .5f;

        // Public properties
        public override bool IsActive { get => this.Attractor.isActiveAndEnabled; }

        // Private properties
        private AttractorBhv Attractor
        {
            get
            {
                if (_attractor == null)
                {
                    _attractor = this.GetComponent<AttractorBhv>();
                }

                return _attractor;
            }
        }

        // Private fields
        private AttractorBhv _attractor;

        private void Awake()
        {
            _attractor = this.Attractor;
        }

        public override void OnDrawGizmos()
        {
            if (this.Attractor.Target != null)
            {
                Gizmos.color = this.WireColor;

                Gizmos.DrawLine(this.Attractor.Target.Position, this.Attractor.EffectivePosition);
            }

            if (this.Attractor.ProjectOntoTerrain && this.Attractor.Raycast(out RaycastHit hitInfo))
            {
                Vector3 projectionOffset = .025f * Vector3.up;

                Vector3 projectionPosition = this.Transform.InverseTransformPoint(hitInfo.point + projectionOffset);

                Quaternion projectionRotation = Quaternion.FromToRotation(this.Up, hitInfo.normal);

                Vector3 projectionScale = new Vector3(this.Scale.x, 0, this.Scale.z);

                Gizmos.matrix = this.Transform.localToWorldMatrix;

                Gizmos.color = this.FaceColor;

                Gizmos.DrawMesh(this.Mesh, projectionPosition, projectionRotation, projectionScale);

                Gizmos.color = this.WireColor;

                Gizmos.DrawWireMesh(this.Mesh, projectionPosition, projectionRotation, projectionScale);

                Gizmos.DrawLine(-this.Scale.y / 2 * Vector3.up, projectionPosition);
            }

            base.OnDrawGizmos();

            Gizmos.matrix = Matrix4x4.TRS(this.Attractor.Target.Position, this.Rotation, this.Attractor.DeadZoneRadius * 2 * Vector3.one);

            Gizmos.color = this.FaceColor.SetAlpha(this.FaceColor.a * _deadZoneAlphaMultiplier);

            Gizmos.DrawMesh(this.Attractor.Target.Gizmo.Mesh);

            Gizmos.color = this.WireColor.SetAlpha(this.WireColor.a * _deadZoneAlphaMultiplier);

            Gizmos.DrawWireMesh(this.Attractor.Target.Gizmo.Mesh);
        }
    }
}
