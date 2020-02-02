using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CollectableBirdController))]
public class CollectableBirdAudioSource : MonoBehaviour {
    AudioSource _source = null;

    // public void PlayAudio() {

    // }

    void Awake() {
        var bird = GetComponent<CollectableBirdController>();
        var channel = bird.BirdChannel;
        _source = GetComponent<AudioSource>();
        _source.clip = channel.AudioClip;
        _source.outputAudioMixerGroup = channel.BirdGroup;
        _source.Play();
    }
}
