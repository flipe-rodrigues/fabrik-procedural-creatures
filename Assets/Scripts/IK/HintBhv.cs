using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public class HintBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField] private ChainBhv _chain;
        [SerializeField] private bool _lookAtChain;

        // Public properties
        public ChainBhv Chain { get => _chain; set => _chain = value; }
        public Vector3 EffectivePosition { get => _effectivePosition; }

        // Private fields
        private Vector3 _effectivePosition;

        private void Update()
        {
            this.UpdateEffectivePosition();

            if (_lookAtChain)
            {
                this.LookAtChain();
            }
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

        private void LookAtChain()
        {
            this.Transform.LookAt(this.Chain.Position);
        }

        public override void UpdateName()
        {
            string sufix = _chain == null ? "________" : _chain.name;

            this.name = "Hint" + " < " + sufix;
        }
    }
}
