using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [Serializable]
    public class MonoBehaviourPairs<TKey, TValue>
        where TKey : MonoBehaviour
        where TValue : MonoBehaviour
    {
        // Public fields
        [SerializeField] private MonoBehaviourPair<TKey, TValue>[] _pairs;

        // Public properties
        public TKey[] Keys => this.Dictionary.Keys.ToArray();
        public TValue[] Values => this.Dictionary.Values.ToArray();
        public int Count => this.Dictionary.Count;

        // Private properties
        private Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                if (_dictionary == null)
                {
                    this.PopulateDictionary();
                }

                return _dictionary;
            }
        }

        // Private fields
        private Dictionary<TKey, TValue> _dictionary;

        // Constructors
        public MonoBehaviourPairs(TKey[] keys)
        {
            _pairs = new MonoBehaviourPair<TKey, TValue>[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                _pairs[i] = new MonoBehaviourPair<TKey, TValue>(keys[i]);
            }

            this.PopulateDictionary();
        }

        // Indexer
        public TValue this[TKey key] { get => this.Dictionary[key]; set => this.Dictionary[key] = value; }

        public void PopulateDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>(_pairs.Length);

            for (int i = 0; i < _pairs.Length; i++)
            {
                if (!_dictionary.ContainsKey(_pairs[i].Key))
                {
                    _dictionary.Add(_pairs[i].Key, _pairs[i].Value);
                }
                else
                {
                    _dictionary[_pairs[i].Key] = _pairs[i].Value;
                }
            }
        }
    }
}
