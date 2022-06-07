using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public abstract class ChainElementBhv : IKElementBhv
    {
        // Serialized fields
        [SerializeField, ReadOnly] private ChainBhv _chain;
        [SerializeField, ReadOnly] private int _index;

        // Public properties
        public ChainBhv Chain
        {
            get
            {
                if (_chain == null)
                {
                    ChainBhv[] parentChains = this.GetComponentsInParentsExclusively<ChainBhv>();

                    if (parentChains != null && parentChains.Length > 0)
                    {
                        _chain = parentChains.First();
                    }
                }

                return _chain;
            }
            set
            {
                _chain = value;
            }
        }
        public int Index => _index;

        public virtual void PseudoConstructor(ChainBhv chain, int index)
        {
            _chain = chain;

            _index = index;
        }

        private void Awake()
        {
            _chain = this.Chain;
        }
    }
}
