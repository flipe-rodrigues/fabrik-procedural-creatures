using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class PheromoneTrackingBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField] private SensorManagerBhv _sensorManager;
        [SerializeField] private float _velocityModifier = 1;
        [SerializeField] private float _accelerationModifier = 1;
        [SerializeField, Range(0, 1)] private float _forwardPriorWeight = 1;
        [SerializeField, Range(0, 1)] private float _wanderWeight = 1;
        [SerializeField, Range(0, 1)] private float _pheromoneWeight = 1;
        [SerializeField] private Vector3 _forwardWeights = new Vector3(1, 0, 1);
        [SerializeField] private Vector3 _positionMinConstraints = Vector3.negativeInfinity;
        [SerializeField] private Vector3 _positionMaxConstraints = Vector3.positiveInfinity;

        // Private fields
        private Vector3 _pheromoneDirection;
        private Vector3 _wanderDirection;
        private Vector3 _desiredDirection;
        private Vector3 _effectiveVelocity;
        private Vector3 _desiredVelocity;
        private Vector3 _previousVelocity;
        private Vector3 _acceleration;

        private void FixedUpdate()
        {
            this.UpdatePosition();

            this.UpdateRotation();
        }

        private void UpdatePosition()
        {
            //_pheromoneDirection = Vector3.Lerp(_pheromoneDirection, _sensorManager.Direction.normalized, Time.deltaTime);

            //_wanderDirection = Vector3.Lerp(_wanderDirection, Random.insideUnitSphere, Time.deltaTime);

            _pheromoneDirection = _sensorManager.Direction.normalized;

            _wanderDirection = Random.insideUnitSphere;

            _desiredDirection = (_forwardPriorWeight * -this.Forward + _pheromoneWeight * _pheromoneDirection + _wanderWeight * _wanderDirection).normalized;

            _desiredVelocity = _velocityModifier * _desiredDirection;

            _acceleration = _accelerationModifier * (_desiredVelocity - _previousVelocity);

            _effectiveVelocity = _previousVelocity + Time.deltaTime * _acceleration;

            this.Position = (this.Position + Time.deltaTime * _effectiveVelocity).ClampBetween(_positionMinConstraints, _positionMaxConstraints);

            _previousVelocity = _effectiveVelocity;
        }

        private void UpdateRotation()
        {
            Vector3 forward = -_effectiveVelocity.ElementWiseMultiplication(_forwardWeights).normalized;

            Quaternion desiredRotation = Quaternion.LookRotation(forward, Vector3.up);

            this.Rotation = Quaternion.Slerp(this.Rotation, desiredRotation, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(this.Position, this.Position + _pheromoneDirection * 5);

            Gizmos.color = Color.green;

            Gizmos.DrawLine(this.Position, this.Position + _effectiveVelocity * 5);
        }
    }
}
