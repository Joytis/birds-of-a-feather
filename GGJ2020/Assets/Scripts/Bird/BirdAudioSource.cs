using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class BirdAudioSource : MonoBehaviour {
    BirdChannelAsset _birdChannel = null;
    AudioSource _source = null;

    public void Initialize(BirdChannelAsset asset) {
        _birdChannel = asset;
        _source = GetComponent<AudioSource>();
        _source.clip = _birdChannel.AudioClip;
        _source.outputAudioMixerGroup = _birdChannel.BirdGroup;
        _source.Play();
    }
}
