using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class BirdChannelFader : MonoBehaviour {
    [SerializeField] float _fadeTime = 1f;
    [SerializeField] TextMeshProUGUI _birdText = null;
    // AudioSource _source = null;
    BirdChannelAsset _birdChannel = null;
    Coroutine _coroutine = null;

    public void Initialize(BirdChannelAsset asset) {
        _birdChannel = asset;
        _birdText.text = _birdChannel.name;
    }

    void DoAsCoroutine(IEnumerator coro) {
        if(_coroutine != null) {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(coro);
    }

    public void FadeOut() => DoAsCoroutine(_birdChannel.FadeOut(_fadeTime));
    public void FadeIn() => DoAsCoroutine(_birdChannel.FadeIn(_fadeTime));
}
