using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class TargetBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField] private JointBhv[] _effectors = new JointBhv[1];
        [SerializeField] private bool _isActive;
        [SerializeField, Min(0)] private int _priority;
        [SerializeField, Min(0)] private float _deadZoneRadius;

        // Public properties
        public JointBhv[] Effectors { get => _effectors; set => _effectors = value; }
        public bool IsActive { get => _isActive; }
        public int Priority { get => _priority; }
        public float DeadZoneRadius { get => _deadZoneRadius; }
        public Vector3 EffectivePosition { get => _effectivePosition; }

        // Private fields
        private Vector3 _effectivePosition;

        public void PseudoConstructor(JointBhv endEffector)
        {
            //_endEffectors = endEffector;

            //this.UpdateName();
        }

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
            string sufix = "________";

            if (this.Effectors.Any(effector => effector != null))
            {
                sufix = this.Effectors.Where(effector => effector != null).Select(effector => effector.name).Aggregate((current, next) => current + ", " + next);
            }

            this.name = "Target" + " < " + sufix;
        }
    }
}
