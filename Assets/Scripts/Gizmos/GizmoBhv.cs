using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    public abstract class GizmoBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField] private Mesh _mesh;
        [SerializeField] private bool _drawIfInactive;
        [SerializeField] private Color _activeColor = new Color(1, 1, 1, 1);
        [SerializeField] private Color _inactiveColor = new Color(.2f, .2f, .2f, 1);
        [SerializeField, Range(0, 1)] private float _faceAlphaMultiplier = .5f;
        [SerializeField, Range(0, 1)] private float _wireAlphaMultiplier = 1f;
        [SerializeField] private Vector3 _activeScaleMultiplier = Vector3.one;
        [SerializeField] private Vector3 _inactiveScaleMultiplier = .5f * Vector3.one;

        // Public properties
        public abstract bool IsActive { get; }
        public Color FaceColor => this.Color.SetAlpha(this.Color.a * _faceAlphaMultiplier);
        public Color WireColor => this.Color.SetAlpha(this.Color.a * _wireAlphaMultiplier);

        // Protected properties
        protected Mesh Mesh => _mesh;
        protected Color Color => this.IsActive ? _activeColor : _inactiveColor;
        protected Vector3 Scale => (this.IsActive ? _activeScaleMultiplier : _inactiveScaleMultiplier).ElementWiseMultiplication(this.LocalScale);

        private void Start() { /* just so this can be enabled & disabled */ }

        public virtual void OnDrawGizmos()
        {
            if (this.Mesh == null || !this.isActiveAndEnabled || (!this.IsActive && !_drawIfInactive))
            {
                return;
            }

            Gizmos.matrix = Matrix4x4.TRS(this.Position, this.Rotation, this.Scale);

            Gizmos.color = this.FaceColor;

            Gizmos.DrawMesh(this.Mesh);

            Gizmos.color = this.WireColor;

            Gizmos.DrawWireMesh(this.Mesh);
        }
    }
}
