using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class LinkBhv : ChainElementBhv
    {
        // Serialized fields
        [SerializeField] private Vector3 _scale = Vector3.one;
        [SerializeField, Range(0, 1)] private float _degreeOfOverlap = 0;

        // Public properties
        public Vector3 Scale
        {
            get
            {
                return this.LocalScale;
            }
            set
            {
                _scale = value;

                this.UpdateScale();
            }
        }
        public float Length => this.LocalScale.z * (1 - _degreeOfOverlap);

        public void PseudoConstructor(ChainBhv chain, int index, Vector3 scale)
        {
            base.PseudoConstructor(chain, index);

            this.Scale = scale;

            this.UpdateName();

            this.LockTransform();
        }

        private void OnValidate()
        {
            this.ValidateScale();

            this.UpdateScale();
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                this.UpdateName();
            }
        }

        private void ValidateScale()
        {
            _scale = new Vector3(Mathf.Max(0, _scale.x), Mathf.Max(0, _scale.y), Mathf.Max(0, _scale.z));

            if (this.Chain != null && this.Chain.Generator.LinkScales[this.Index] != _scale)
            {
                this.Chain.Generator.LinkScales[this.Index] = _scale;
            }
        }

        private void UpdateScale()
        {
            this.LocalScale = _scale;

            this.LocalPosition = Vector3.zero + this.Length / 2 * Vector3.forward;

            if (this.Chain != null)
            {
                this.Chain.Joints[this.Index + 1].LocalPosition = this.Length * Vector3.forward;
            }
        }

        public override void UpdateName()
        {
            this.name = "Link " + "(" + this.Index + "-" + (this.Index + 1) + " - " + this.Chain.name + ")";
        }

        private void LockTransform()
        {
            this.Transform.hideFlags = HideFlags.NotEditable;
        }
    }
}
