using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class ChainGenerator : MonoBehaviour
    {
        // Serialized fields
        [SerializeField] private JointBhv _jointPrefab;
        [SerializeField] private LinkBhv _linkPrefab;
        [SerializeField] private TargetBhv _targetPrefab;
        [SerializeField, ReadOnly, Min(2)] private int _jointCount;
        [SerializeField, Min(1)] private int _linkCount = 2;
        [SerializeField] private ScaleCurves _linkScaleSamplingCurves;
        [SerializeField] private Vector3[] _linkScales = new Vector3[0];

        // Public properties
        public Vector3[] LinkScales { get => _linkScales; set => _linkScales = value; }
        public bool HasInitializedScales => _linkScales.Length == _linkCount;
        public bool HasChain => this.GetComponent<ChainBhv>() != null;
        public bool HasInverseKinematics => this.GetComponent<InverseKinematicsBhv>() != null;

        // Private properties
        private ChainBhv Chain
        {
            get
            {
                if (_chain == null)
                {
                    _chain = this.GetComponent<ChainBhv>();
                }

                return _chain;
            }
        }

        // Private fields
        private ChainBhv _chain;
        private Vector3[] _previousLinkScales;

        private void OnValidate()
        {
            _jointCount = _linkCount + 1;

            this.ValidateLinkScales();
        }

        public void Reset()
        {
            if (this.HasChain)
            {
                //foreach (JointBhv joint in this.Chain.Joints)
                //{
                //    if (joint != null)
                //    {
                //        DestroyImmediate(joint.gameObject);
                //    }
                //}

                //this.Chain.Joints = null;

                //foreach (LinkBhv link in this.Chain.Links)
                //{
                //    if (link != null)
                //    {
                //        DestroyImmediate(link.gameObject);
                //    }
                //}

                //this.Chain.Links = null;

                //foreach (TargetBhv target in this.Chain.Targets)
                //{
                //    if (target != null)
                //    {
                //        DestroyImmediate(target.gameObject);
                //    }
                //}

                //this.Chain.Targets = null;

                while (this.Chain.transform.childCount > 0)
                {
                    DestroyImmediate(this.Chain.transform.GetChild(0).gameObject);
                }

                DestroyImmediate(this.Chain);
            }

            if (this.HasInverseKinematics)
            {
                DestroyImmediate(this.GetComponent<InverseKinematicsBhv>());
            }
        }

        private void ValidateLinkScales()
        {
            if (!this.HasChain)
            {
                return;
            }

            if (_previousLinkScales == null || _previousLinkScales.Length != _linkScales.Length)
            {
                _previousLinkScales = _linkScales;
            }

            for (int i = 0; i < _linkScales.Length; i++)
            {
                if (_linkScales[i] != _previousLinkScales[i])
                {
                    _linkScales[i] = new Vector3(Mathf.Max(0, _linkScales[i].x), Mathf.Max(0, _linkScales[i].y), Mathf.Max(0, _linkScales[i].z));

                    this.Chain.Links[i].Scale = _linkScales[i];

                    _previousLinkScales[i] = _linkScales[i];
                }
            }
        }

        public void SampleLinkScales()
        {
            _linkScales = new Vector3[_linkCount];

            _previousLinkScales = new Vector3[_linkCount];

            for (int i = 0; i < _linkCount; i++)
            {
                _linkScales[i] = _linkScaleSamplingCurves.GetScale((float)i / (_linkCount - 1));
            }

            this.ValidateLinkScales();
        }

        public void InstantiateChain()
        {
            if (!this.HasInitializedScales)
            {
                return;
            }

            _chain = this.gameObject.AddComponent<ChainBhv>();

            this.InstantiateJoints();

            this.InstantiateLinks();

            //if (_targetPrefab != null)
            //{
            //    this.InstantiateTargets();
            //}

            //this.gameObject.AddComponent<InverseKinematicsBhv>();
        }

        private void InstantiateJoints()
        {
            JointBhv[] joints = new JointBhv[_jointCount];

            Vector3 position = this.Chain.Position;

            for (int i = 0; i < _jointCount; i++)
            {
                Transform parent = i == 0 ? this.Chain.transform : joints[i - 1].transform;

                position += i > 0 ? _linkScales[i - 1].z * Vector3.forward : Vector3.zero;

                joints[i] = Instantiate(_jointPrefab, position, Quaternion.identity, parent);

                joints[i].PseudoConstructor(this.Chain, i);
            }

            this.Chain.Joints = joints;
        }

        private void InstantiateLinks()
        {
            LinkBhv[] links = new LinkBhv[_linkCount];

            for (int i = 0; i < _linkCount; i++)
            {
                links[i] = Instantiate(_linkPrefab, this.Chain.Joints[i].transform);

                links[i].PseudoConstructor(this.Chain, i, _linkScales[i]);
            }

            this.Chain.Links = links;
        }

        //private void InstantiateTargets()
        //{
        //    Transform targetsParent = new GameObject("Targets").transform;

        //    targetsParent.parent = this.Chain.transform;

        //    targetsParent.localPosition = Vector3.zero;

        //    TargetBhv[] targets = new TargetBhv[_jointCount];

        //    for (int i = 0; i < _jointCount; i++)
        //    {
        //        targets[i] = Instantiate(_targetPrefab, this.Chain.Joints[i].Position, Quaternion.identity, targetsParent);

        //        targets[i].PseudoConstructor(this.Chain, i, this.Chain.Joints[i]);
        //    }

        //    this.Chain.Targets = targets;
        //} 

        public void RandomizePose()
        {
            if (!this.HasChain)
            {
                return;
            }

            for (int i = 0; i < this.Chain.Joints.Length; i++)
            {
                this.Chain.Joints[i].transform.rotation = Quaternion.Euler(Random.onUnitSphere * 90);
            }
        }
    }
}
