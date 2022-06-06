using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class HintBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField] private JointBhv _effector;
        [SerializeField] private bool _isActive;

        // Public properties
        public JointBhv Effector { get => _effector; set => _effector = value; }
        public bool IsActive { get => _isActive; }
        public Vector3 EffectivePosition { get => _effectivePosition; }

        // Private fields
        private Vector3 _effectivePosition;

        private void Update()
        {
            this.UpdateEffectivePosition();
        }

        private void UpdateEffectivePosition()
        {
            if (!this.IK.SnapToTerrain)
            {
                _effectivePosition = this.Position;
            }

            else if (Physics.Raycast(this.Position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, this.IK.TerrainLayerMask))
            {
                _effectivePosition = hitInfo.point + this.IK.TerrainOffset * Vector3.up;
            }
        }

        public override void UpdateName()
        {
            string sufix = _effector == null ? "________" : _effector.name;

            this.name = "Target" + " < " + sufix;
        }
    }
}
