using UnityEngine;

public class BirdAssetReference : MonoBehaviour {
    [SerializeField] BirdChannelAsset _birdChannel = null;
    public BirdChannelAsset BirdChannel => _birdChannel;
}
