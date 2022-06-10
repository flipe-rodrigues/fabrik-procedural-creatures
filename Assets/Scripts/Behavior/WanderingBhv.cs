using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class WanderingBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField] private float _maxSpeed = 1;
        [SerializeField] private float _steerStrength = 1;
        [SerializeField] private float _wanderStrength = 1;
        [SerializeField] private Vector3 _velocityWeights = Vector3.one;
        [SerializeField] private Vector3 _positionMinConstraints = Vector3.negativeInfinity;
        [SerializeField] private Vector3 _positionMaxConstraints = Vector3.positiveInfinity;

        // Private fields
        private Vector3 _velocity;
        private Vector3 _desiredDirection;

        private void Update()
        {
            _desiredDirection = (_desiredDirection + Random.insideUnitSphere * _wanderStrength).normalized;

            Vector3 desiredVelocity = _desiredDirection * _maxSpeed;

            Vector3 desiredSteeringForce = (desiredVelocity - _velocity) * _steerStrength;

            Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, _steerStrength);

            _velocity = Vector3.ClampMagnitude(_velocity + acceleration * Time.deltaTime, _maxSpeed);

            _velocity = _velocity.ElementWiseMultiplication(_velocityWeights);

            Vector3 position = this.Position + _velocity * Time.deltaTime;

            this.Position = position.ClampBetween(_positionMinConstraints, _positionMaxConstraints);

            //Vector3 target = this.Position + _velocity;

            //target.y = this.Position.y;

            //this.Transform.LookAt(target);
        }
    }
}
