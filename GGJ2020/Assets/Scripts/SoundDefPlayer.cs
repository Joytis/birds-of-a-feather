using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundDefPlayer : MonoBehaviour {
    [SerializeField] SoundDef[] _sounds = null;
    AudioSource _source = null;

    void Awake() => _source = GetComponent<AudioSource>();

    public void PlaySound(int index) {
        // Play no sound if we can't
        if(index >= _sounds.Length || index < 0) return;

        // Otherwise play it!
        var sound = _sounds[index];
        _source.pitch = Random.Range(sound.MinMaxPitch.x, sound.MinMaxPitch.y);
        _source.PlayOneShot(sound.Clip);
    }

}
