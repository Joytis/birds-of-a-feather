using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MainMenuAudio : MonoBehaviour {
    AudioSource _source = null;

    void Awake() {
        // NOTE(clark): Don't ask. We don't have a menu state machine. Lol. 
        Time.timeScale = 1f;


        _source = GetComponent<AudioSource>();
        if(SaveData.IsGameComplete()) {
            _source.Play();
        }
    } 
}
