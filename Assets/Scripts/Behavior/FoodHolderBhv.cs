using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class FoodHolderBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField] private LayerMask _foodLayerMask;
        [SerializeField, Min(0)] private float _grabRadius = 5;
        [SerializeField, Min(0)] private float _grabSpeed = 5;
        [SerializeField, ReadOnly] private bool _isHolding;

        // Public properties
        public bool IsHolding => _isHolding;

        // Private fields
        private Transform _cargo;

        private void Update()
        {
            if (!_isHolding)
            {
                Collider[] colliders = Physics.OverlapSphere(this.Position, _grabRadius, _foodLayerMask);

                if (colliders.Length > 0)
                {
                    _cargo = colliders[0].transform;

                    _isHolding = true;
                }
            }

            else
            {
                _cargo.position = Vector3.Lerp(_cargo.position, this.Position + Vector3.up, Time.deltaTime * _grabSpeed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.Position, _grabRadius);
        }
    }
}
