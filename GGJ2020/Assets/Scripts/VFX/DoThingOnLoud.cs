using System;
using UnityEngine;
 
[RequireComponent(typeof(AudioSource))]
public class DoThingOnLoud : MonoBehaviour { 
    AudioSource _audioSource;
    const float _updateStep = 0.03f;
    const int _sampleDataLength = 1024; 
    const float _loudnessThreshold = 0.04f; 
    float _currentUpdateTime = 0f; 
    float _clipLoudness;
    float[] _clipSampleData; 

    bool _wasLoudPreviously = false;

    public event Action WasLoud;

    // Use this for initialization
    void Awake () {     
        _audioSource = GetComponent<AudioSource>();
        _clipSampleData = new float[_sampleDataLength]; 
    }     
    // Update is called once per frame
    void Update () {     
        _currentUpdateTime += Time.deltaTime;
        if (_currentUpdateTime >= _updateStep) {
            _currentUpdateTime = 0f;
            //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            _audioSource.clip.GetData(_clipSampleData, _audioSource.timeSamples); 
            _clipLoudness = 0f;
            foreach (var sample in _clipSampleData) {
                _clipLoudness += Mathf.Abs(sample);
            }
            _clipLoudness /= _sampleDataLength; //_clipLoudness is what you are looking for

            if(_clipLoudness >= _loudnessThreshold) {
                if(!_wasLoudPreviously) {
                    WasLoud?.Invoke();
                    _wasLoudPreviously = true;
                }
            }
            else if(_wasLoudPreviously) {
                _wasLoudPreviously = false;
            }
        } 
    }
}