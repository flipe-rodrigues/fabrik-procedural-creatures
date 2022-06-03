using System;
using System.Collections;
using UnityEngine;

namespace ProceduralAnimation
{
    [Serializable]
    public class MonoBehaviourPair<TKey, TValue> 
        where TKey : MonoBehaviour 
        where TValue : MonoBehaviour
    {
        // Public fields
        [SerializeField, ReadOnly] private TKey _key;
        [SerializeField] private TValue _value;

        // Public properties
        public TKey Key => _key;
        public TValue Value { get => _value; set => _value = value; }

        // Constructors
        public MonoBehaviourPair(TKey key)
        {
            _key = key;
        }
    }
}
