using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using LWG.Core.Fsm;
using Cinemachine;
using TMPro;

public class MapTraumaToCamera : MonoBehaviour {
    [SerializeField] CinemachineFreeLook _freeLookCamera = null;
    [SerializeField] CameraTrauma _trauma = null;
    [SerializeField] int _numRigs = 1;
    [SerializeField] float _amplitude = 4f;
    // bool _updateTrauma = true;

    void Awake() => _trauma.RemoveAllTrauma();

    void SetNoiseFor(int i) {
        var noise = _freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = _trauma.Shake * _amplitude;
    }

    void Update() {
        _trauma.UpdateTrauma(Time.deltaTime);
        for(int i = 0; i < _numRigs; i++) {
            SetNoiseFor(i);
        }
    }

}