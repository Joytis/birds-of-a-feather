using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class BirdChannelFaderLoader : MonoBehaviour {
    [SerializeField] BirdChannelAsset[] _birdAssets = null;
    [SerializeField] GameObject _targetParent = null;
    [SerializeField] GameObject _birdPrefab = null;

    void Awake() {
        foreach(var bird in _birdAssets) {
            var newBird = Instantiate(_birdPrefab, _targetParent.transform);
            var fader = newBird.GetComponent<BirdChannelFader>();
            fader.Initialize(bird);

            var source = newBird.GetComponent<BirdAudioSource>();
            source.Initialize(bird);
        }
    }
}
