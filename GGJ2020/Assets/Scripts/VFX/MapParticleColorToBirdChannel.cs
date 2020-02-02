using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CollectableBirdController))]
public class MapParticleColorToBirdChannel : MonoBehaviour {
    CollectableBirdController _ref = null;

    void Awake() {
        _ref = GetComponent<CollectableBirdController>();

        foreach(var particles in GetComponentsInChildren<ParticleSystem>()) {
            var main = particles.main;
            main.startColor = _ref.BirdChannel.BirdColor;
        }
    }
}
