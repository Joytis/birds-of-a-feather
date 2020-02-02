using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BirdAssetReference))]
public class MapBirdColorToImages : MonoBehaviour {
    [SerializeField] Image[] _images = null;
    BirdAssetReference _ref = null;

    void Awake() {
        _ref = GetComponent<BirdAssetReference>();
        foreach(var image in _images) {
            image.color = _ref.BirdChannel.BirdColor;
        }
    }
}
