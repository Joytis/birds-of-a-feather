using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerHashSet<T> : HashSet<T>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<T> values = new List<T>();

    // save the dictionary to lists
    public void OnBeforeSerialize() {
        values.Clear();
        foreach(var val in this) 
            values.Add(val);
    }

    // load dictionary from lists
    public void OnAfterDeserialize() {
        this.Clear();
        foreach(var val in values) 
            this.Add(val);
    }
}
