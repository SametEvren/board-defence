﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new ();

        [SerializeField]
        private List<TValue> values = new ();

        // Save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // Load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new Exception($"There are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");

            for (int i = 0; i < keys.Count; i++)
                this[keys[i]] = values[i];
        }
    }
}
