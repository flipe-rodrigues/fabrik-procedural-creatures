using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(ParticleSystem))]
    public class PheromoneSourceBhv : MonoBehaviour
    {
        // Private fields
        [SerializeField] private SensorBhv[] _sensors;
        private ParticleSystem _particleSystem;
        private List<ParticleSystem.Particle> _particles;
        private ParticleSystem.ColliderData _colliderData;

        private void OnEnable()
        {
            _particleSystem = this.GetComponent<ParticleSystem>();

            _sensors = FindObjectsOfType<SensorBhv>();

            this.PopulateTriggerModule();
        }

        private void PopulateTriggerModule()
        {
            while (_particleSystem.trigger.colliderCount > 0)
            {
                _particleSystem.trigger.RemoveCollider(0);
            }

            foreach (SensorBhv sensor in _sensors)
            {
                _particleSystem.trigger.AddCollider(sensor.Collider);
            }
        }

        private void OnParticleTrigger()
        {
            if (!Application.isPlaying || !this.isActiveAndEnabled)
            {
                return;
            }

            if (_particleSystem == null)
            {
                _particleSystem = this.GetComponent<ParticleSystem>();
            }

            if (_particles == null)
            {
                _particles = new List<ParticleSystem.Particle>();
            }

            int particleCount = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, _particles, out _colliderData);

            foreach(SensorBhv sensor in _sensors)
            {
                sensor.OnPheromoneTrigger(particleCount, _colliderData);
            }
        }
    }
}
