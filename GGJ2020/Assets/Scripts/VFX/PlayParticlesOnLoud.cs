using System;
using UnityEngine;
 
[RequireComponent(typeof(ParticleSystem))]
public class PlayParticlesOnLoud : MonoBehaviour {
    ParticleSystem _particles = null;
    [SerializeField] DoThingOnLoud _thingOnLoud = null;

    void Awake() => _particles = GetComponent<ParticleSystem>();
    void OnEnable() => _thingOnLoud.WasLoud += ThingOnLoud;
    void OnDisable() => _thingOnLoud.WasLoud -= ThingOnLoud;
    void ThingOnLoud()  => _particles.Play();
}

