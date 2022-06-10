using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(Collider))]
    public class SensorBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField, ReadOnly] private float _pheromoneSignal;

        // Public properties
        public float PheromoneCount => _pheromoneSignal;
        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = this.GetComponent<Renderer>();
                }

                return _renderer;
            }
        }
        public Collider Collider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = this.GetComponent<Collider>();
                }

                return _collider;
            }
        }

        // Private fields
        private Renderer _renderer;
        private Collider _collider;

        private void OnEnable()
        {
            _renderer = this.GetComponent<Renderer>();

            _collider = this.GetComponent<Collider>();
        }

        public void UpdateAlpha()
        {
            Color baseColor = this.Renderer.material.GetColor("_BaseColor");

            Color newColor = baseColor.SetAlpha(this.PheromoneCount / 150f);

            this.Renderer.material.SetColor("_BaseColor", Color.Lerp(baseColor, newColor, Time.deltaTime));
        }

        public void OnPheromoneTrigger(int totalPheromoneCount, ParticleSystem.ColliderData pheromoneColliderData)
        {
            int count = 0;

            for (int i = 0; i < totalPheromoneCount; i++)
            {
                for (int j = 0; j < pheromoneColliderData.GetColliderCount(i); j++)
                {
                    if (pheromoneColliderData.GetCollider(i, j) == _collider)
                    {
                        count++;
                    }
                }
            }

            _pheromoneSignal = count == 0 ? _pheromoneSignal * .95f : count;

            this.UpdateAlpha();
        }
    }
}
