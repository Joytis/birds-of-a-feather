using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CollectableBirdController))]
public class DriveChannelByOffset : MonoBehaviour {
    const float _min = 0f;
    const float _max = 1f;
    const float _distanceForSound = 50f;
    CollectableBirdController _birdController = null;

    void Awake() => _birdController = GetComponent<CollectableBirdController>();

    void Update() {
        var playerPosition = PlayerBirdController.Instance.transform.position;   
        var distance = Vector3.Distance(playerPosition, transform.position);

        var distanceScale = (_distanceForSound - distance) / _distanceForSound;
        var targetFade = Mathf.Lerp(_min, _max, Mathf.Clamp01(distanceScale));

        _birdController.BirdChannel.SetFade(targetFade);
    }
}
