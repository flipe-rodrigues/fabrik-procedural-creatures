using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class CachedTransformBhv : MonoBehaviour
    {
        // Public properties
        public Transform Parent
        {
            get
            {
                if (_parent == null)
                {
                    _parent = this.Transform.parent;
                }

                return _parent;
            }
        }
        public Vector3 Position { get => this.Transform.position; set => this.Transform.position = value; }
        public Vector3 LocalPosition { get => this.Transform.localPosition; set => this.Transform.localPosition = value; }
        public Quaternion Rotation { get => this.Transform.rotation; set => this.Transform.rotation = value; }
        public Vector3 LocalScale { get => this.Transform.localScale; set => this.Transform.localScale = value; }
        public Vector3 Forward { get => this.Transform.forward; set { this.Transform.forward = value; } }
        public Vector3 Up { get => this.Transform.up; set { this.Transform.up = value; } }
        public Vector3 Right { get => this.Transform.right; set { this.Transform.right = value; } }

        // Protected properties
        protected Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = this.transform;
                }

                return _transform;
            }
        }

        // Private fields
        private Transform _transform;
        private Transform _parent;

        private void Awake()
        {
            _transform = this.Transform;

            _parent = this.Parent;
        }
    }
}
