using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class TargetBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField] private JointBhv _effector;
        [SerializeField, Min(0)] private int _priority;

        // Public properties
        public JointBhv Effector { get => _effector; set => _effector = value; }
        public GizmoBhv Gizmo
        {
            get
            {
                if (_gizmo == null)
                {
                    _gizmo = this.GetComponent<GizmoBhv>();
                }

                return _gizmo;
            }
        }
        public int Priority { get => _priority; }
        public Vector3 EffectivePosition { get => _effectivePosition; }

        // Private fields
        private GizmoBhv _gizmo;
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
            string sufix = _effector == null ? "________" : _effector.name;

            this.name = "Target" + " < " + sufix;
        }
    }
}
