using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libs
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private List<SerializableDictionaryItem<TKey, TValue>> items = new();

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> newDictionary = new();

            if (items == null) return newDictionary;

            foreach (var item in items)
            {
                if (!newDictionary.ContainsKey(item.key))
                    newDictionary.Add(item.key, item.value);
                else
                    Debug.LogWarning($"Duplicate key found: {item.key}");
            }

            return newDictionary;
        }

        public void UpdateValue(TKey key, TValue value)
        {
            foreach (var item in items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.key, key))
                {
                    item.value = value; // Update value in the list
                    return;
                }
            }

            items.Add(new SerializableDictionaryItem<TKey, TValue> { key = key, value = value });
        }
    }

    [Serializable]
    public class SerializableDictionaryItem<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}