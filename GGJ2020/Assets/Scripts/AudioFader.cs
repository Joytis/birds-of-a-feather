using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioFader : MonoBehaviour {
    AudioSource _source = null;

    void Awake() {
        _source = GetComponent<AudioSource>();
     } 

    Coroutine _coroutine = null;
    void DoAsCoroutine(IEnumerator coro) {
        if(_coroutine != null) {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(coro);
    }

    IEnumerator InternalFade(float to, float t) {
        var currentTime = 0f;
        var prevVolume = _source.volume;
        while(currentTime < t) {
            var timeFactor = currentTime / t;
            _source.volume = Mathf.Lerp(prevVolume, to, timeFactor);
            yield return null;
            currentTime += Time.deltaTime;
        }
        // Set min attenuation. 
        _source.volume = to;
    }

    public void FadeOut(float time) {
        DoAsCoroutine(InternalFade(0f, time));
    }

    public void FadeIn(float time) {
        DoAsCoroutine(InternalFade(1f, time));
    }

    public void FadeTo(float time, float to) {
        DoAsCoroutine(InternalFade(to, time));
    }
}
