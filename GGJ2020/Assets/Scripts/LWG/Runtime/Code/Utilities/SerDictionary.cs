using System;
using System.Collections.Generic;
using UnityEngine;

namespace LWG {

[Serializable]
public class SerDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> _keys = new List<TKey>();

    [SerializeField]
    List<TValue> _values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        
        foreach(var pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();
        if(_keys.Count != _values.Count) 
            throw new Exception($"there are {_keys.Count} keys and {_values.Count} values after deserialization. " +
                                 "Make sure that both key and value types are serializable.");

        for(int i = 0; i < _keys.Count; i++) 
            Add(_keys[i], _values[i]);

        _keys.Clear();
        _values.Clear();
    }
}

} // namespace LWG