using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Game/BirdChannel")]
public class BirdChannelAsset : ScriptableObject
{
    [SerializeField] AudioMixer _mixer = null;
	[SerializeField] AudioMixerGroup _birdGroup = null;
    public AudioMixerGroup BirdGroup => _birdGroup;

    [SerializeField] AudioClip _audioClip = null;
    public AudioClip AudioClip => _audioClip;

    [SerializeField] Color _birdColor = Color.white;
    public Color BirdColor => _birdColor;

    public string TargetVolume => $"{_birdGroup.name}Vol";

	public void SetFade(float value) {
		var clamped = Mathf.Clamp01(value);
        var attenuation = Mathf.Lerp(-80f, 0f, clamped);
        _mixer.SetFloat(TargetVolume, attenuation);
	}

    public IEnumerator FadeOperation(float to, float time) {
        var currentTime = 0f;
        var hasAttenuation = _mixer.GetFloat(TargetVolume, out float attenuation);
        float from = Mathf.InverseLerp(-80f, 0f, attenuation);
        while(currentTime < time) {
            var timeFactor = currentTime / time;
            SetFade(Mathf.Lerp(from, to, timeFactor));
            yield return null;
            currentTime += Time.deltaTime;
        }
        // Set min attenuation. 
        SetFade(to);
    }

    public IEnumerator FadeOut(float time) => FadeOperation(0f, time);
    public IEnumerator FadeIn(float time) => FadeOperation(1f, time);
}
