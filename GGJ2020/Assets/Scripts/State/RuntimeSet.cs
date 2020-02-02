using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class RuntimeSet<TItem> : ScriptableObject {
    HashSet<TItem> _set = new HashSet<TItem>();

    public event Action<TItem> ItemAdded;

    public int Count => _set.Count;

    void OnEnable() => Clear();

    public bool Add(TItem asset) {
        if(_set.Contains(asset)) {
            return false;
        }
        else {
            ItemAdded?.Invoke(asset);
            return _set.Add(asset);

        }
    }
    public bool Remove(TItem asset) {
        if(!_set.Contains(asset)) {
            Debug.LogWarning($"Set does not contain {asset}");
        }
        return _set.Remove(asset);
    }

    public bool Contains(TItem asset) => _set.Contains(asset);
    public void Clear() => _set.Clear();

    public IEnumerable<TItem> Items => _set;
}

