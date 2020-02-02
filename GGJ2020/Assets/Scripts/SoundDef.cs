using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "LWG/SoundDef")]
public class SoundDef : ScriptableObject {
    [SerializeField] AudioClip _clip = null;
    [SerializeField] Vector2 _minMaxPitch = new Vector2(0.8f, 1.2f);

    public AudioClip Clip => _clip;
    public Vector2 MinMaxPitch => _minMaxPitch;

}
