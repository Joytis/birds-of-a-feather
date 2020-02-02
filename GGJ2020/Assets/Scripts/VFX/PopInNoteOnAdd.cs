using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BirdAssetReference))]
public class PopInNoteOnAdd : MonoBehaviour {
    [SerializeField] Animator _animator = null;
    [SerializeField] BirdChannelRuntimeSet _birdChannelSet = null;
    BirdAssetReference _ref = null;

    void Awake() => _ref = GetComponent<BirdAssetReference>();
    void OnEnable() => _birdChannelSet.ItemAdded += OnThingAdded;
    void OnOnDisable() => _birdChannelSet.ItemAdded -= OnThingAdded;
    void OnThingAdded(BirdChannelAsset asset) {
        if(asset == _ref.BirdChannel) {
            _animator.Play("PopIn");
        }
    } 
}
