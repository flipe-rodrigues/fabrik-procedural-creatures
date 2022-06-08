using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    public abstract class IKElementBhv : CachedTransformBhv
    {
        // Protected properties
        protected InverseKinematicsBhv IK
        {
            get
            {
                if (_inverseKinematics == null)
                {
                    _inverseKinematics = this.GetComponentInParent<InverseKinematicsBhv>();
                }

                return _inverseKinematics;
            }
        }

        // Private fields
        private InverseKinematicsBhv _inverseKinematics;

        private void Awake()
        {
            _inverseKinematics = this.IK;
        }

        private void OnEnable()
        {
            IK.ResetIKElements();
        }

        public virtual void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                this.UpdateName();
            }
        }
        
        private void OnDisable()
        {
            IK.ResetIKElements();
        }

        private void OnDestroy()
        {
            IK.ResetIKElements();
        }

        public abstract void UpdateName();
    }
}
