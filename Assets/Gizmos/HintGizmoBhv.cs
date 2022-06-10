using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(HintBhv))]
    public class HintGizmoBhv : GizmoBhv
    {
        // Public properties
        public override bool IsActive { get => this.Hint.isActiveAndEnabled; }

        // Private properties
        private HintBhv Hint
        {
            get
            {
                if (_hint == null)
                {
                    _hint = this.GetComponent<HintBhv>();
                }

                return _hint;
            }
        }

        // Private fields
        private HintBhv _hint;

        private void Awake()
        {
            _hint = this.Hint;
        }

        public override void OnDrawGizmos()
        {
            if (this.IsActive)
            {
                Gizmos.color = this.WireColor;

                Gizmos.DrawLine(this.Hint.Position, this.Hint.EffectivePosition);
            }

            base.OnDrawGizmos();
        }
    }
}
