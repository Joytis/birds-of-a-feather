using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class BirdFollower : MonoBehaviour {
    public class Handle {
        readonly BirdFollower _follower;
        readonly int _targetIndex;
        public Handle(BirdFollower follower, int targetIndex) => 
            (_follower, _targetIndex) = (follower, targetIndex);

        public bool TryGetPosition(out Vector3 pos) {
            if(_targetIndex > _follower._positionStack.Count - 1) {
                pos = Vector3.zero;
                return false;
            }
            else {
                pos = _follower._positionStack[_targetIndex];
                return true;
            }
        }
    }

    Vector3 _previousPosition = Vector3.zero;
    List<Vector3> _positionStack = null;
    int _currentHandles = 0;

    [SerializeField] float _epsilon = 0.1f;
    [SerializeField] int _maxFollowers = 10;
    [SerializeField] int _clumpitude = 30;

    int MaxElements => _clumpitude * _maxFollowers;

    void Awake() {
        _positionStack = new List<Vector3>(Enumerable.Repeat(Vector3.zero, MaxElements));
        _previousPosition = transform.position;
        _positionStack.Add(_previousPosition);
    }

    public Handle GenerateHandle() {
        _currentHandles += 1;
        if(_currentHandles > _maxFollowers) throw new InvalidOperationException("NOPE LOL");
        return new Handle(this, _currentHandles * _clumpitude);
    }

    void Update() {
        var distancePerSec = Vector3.Distance(transform.position, _previousPosition) / Time.deltaTime; 
        if(distancePerSec > _epsilon) {
            _positionStack.Insert(0, transform.position);
            if(_positionStack.Count > MaxElements) {
                _positionStack.RemoveAt(_positionStack.Count - 1);
            }
        }
        _previousPosition = transform.position;
    }
}
