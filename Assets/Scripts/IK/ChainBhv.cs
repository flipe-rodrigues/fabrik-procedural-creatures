using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(ChainGenerator))]
    public class ChainBhv : CachedTransformBhv
    {
        // Serialized fields
        [SerializeField, ReadOnly] private JointBhv[] _joints;
        [SerializeField, ReadOnly] private LinkBhv[] _links;
        [SerializeField, ReadOnly] private int _depth;

        // Public properties
        public ChainGenerator Generator
        {
            get
            {
                if (_chainGenerator == null)
                {
                    _chainGenerator = this.GetComponent<ChainGenerator>();
                }

                return _chainGenerator;
            }
        }
        public JointBhv[] Joints
        {
            get
            {
                if (_joints == null || _joints.Length == 0)
                {
                    _joints = this.GetComponentsInChildren<JointBhv>().Where(joint => joint.Chain == this).OrderBy(joint => joint.Index).ToArray();
                }

                return _joints;
            }
            set
            {
                _joints = value;
            }
        }
        public LinkBhv[] Links
        {
            get
            {
                if (_links == null || _links.Length == 0)
                {
                    _links = this.GetComponentsInChildren<LinkBhv>().Where(link => link.Chain == this).OrderBy(link => link.Index).ToArray();
                }

                return _links;
            }
            set
            {
                _links = value;
            }
        }
        public int Depth => _depth;

        // Private fields
        private ChainGenerator _chainGenerator;

        private void Awake()
        {
            _chainGenerator = this.Generator;

            _joints = this.Joints;

            _links = this.Links;
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                _depth = this.GetComponentsInParentsExclusively<ChainBhv>().Length;
            }
        }
    }
}
