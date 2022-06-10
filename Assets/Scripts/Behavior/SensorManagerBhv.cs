using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class SensorManagerBhv : CachedTransformBhv
    {
        // Public properties
        public Vector3 Direction
        {
            get
            {
                if (this.Sensors.Any(sensor => sensor.PheromoneCount > 0))
                {
                    _direction = this.Sensors.Select(sensor => sensor.PheromoneCount * sensor.Forward).ToArray().Mean();
                }
                else
                {
                    _direction = Vector3.zero;
                }

                return _direction;
            }
        }

        // Private properties
        private SensorBhv[] Sensors
        {
            get
            {
                if (_sensors == null || _sensors.Length == 0)
                {
                    _sensors = this.GetComponentsInChildren<SensorBhv>();
                }

                return _sensors;
            }
        }

        // Private fields
        private SensorBhv[] _sensors;
        private Vector3 _direction;
    }
}
