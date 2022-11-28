using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class WanderingAttractorBhv : CachedTransformBhv
    {
        public Bounds bounds;
        public float movementSpeed = 5f;
        public float updateSpeed = 1f;
        public float wanderingStrength = 8.5f;
        public float oscillationAmplitude = 5f;
        public float oscillationFrequency = 1f;
        public float[] oscillationAmplitudes = new float[2];
        public float[] oscillationFrequencies = new float[2];

        private Vector3 _movementDirection;
        private Vector3 _closestPointOnBounds;
        private Vector3 _boundsRepulsion;
        private Vector3 _oscillationUpdate;

        private void Start()
        {
            _movementDirection = Random.onUnitSphere.ElementWiseMultiplication(new Vector3(1, 0, 1));
        }

        private void Update()
        {
            Vector3 translation = Time.deltaTime * movementSpeed * _movementDirection;

            this.Transform.Translate(translation);

            _closestPointOnBounds = bounds.ClosestPoint(this.Position);

            _boundsRepulsion = _closestPointOnBounds - this.Position;

            Vector3 repulsionUpdate = Time.deltaTime * updateSpeed * _boundsRepulsion;

            Vector3 wanderingUpdate = Time.deltaTime * wanderingStrength * Random.onUnitSphere;

            Vector3 directionUpdate = repulsionUpdate + wanderingUpdate;

            _movementDirection = (_movementDirection + directionUpdate).normalized.ElementWiseMultiplication(new Vector3(1, 0, 1));

            //_oscillationUpdate = Vector3.zero;

            //for (int i = 0; i < 2; i++)
            //{
            //    float oscillationMagnitude = Time.deltaTime * Mathf.Sin(2f * Mathf.PI * oscillationFrequencies[i] * Time.time) * oscillationAmplitudes[i];

            //    Vector3 oscillationDirection = Vector3.Cross(Vector3.up, _movementDirection - _oscillationUpdate);

            //    _oscillationUpdate += oscillationMagnitude * oscillationDirection;
            //}

            //_movementDirection += _oscillationUpdate;

            _oscillationUpdate = Mathf.Sin(2f * Mathf.PI * oscillationFrequency * Time.time) * oscillationAmplitude * Vector3.Cross(Vector3.up, _movementDirection);

            //_movementDirection += Time.deltaTime * _oscillationUpdate;

            this.Transform.Translate(Time.deltaTime * _oscillationUpdate);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.color = Color.blue;

            Gizmos.DrawLine(this.Position, this.Position + _movementDirection);

            Gizmos.color = Color.green;

            Gizmos.DrawLine(_closestPointOnBounds, _closestPointOnBounds + _boundsRepulsion);

            Gizmos.color = Color.magenta;

            Gizmos.DrawLine(this.Position, this.Position + _oscillationUpdate);
        }
    }
}
