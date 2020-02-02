using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FadeAllBirdsOnClick : MonoBehaviour {
    [SerializeField] BirdChannelRuntimeSet _birdSet = null;

    Button _button = null;
    void Awake() => _button = GetComponent<Button>();
    void OnEnable() => _button.onClick.AddListener(FadeBirds);
    void OnDisable() => _button.onClick.RemoveListener(FadeBirds);
    void FadeBirds() {
        foreach(var bird in _birdSet.Items) {
            StartCoroutine(bird.FadeOut(0.5f));
        }
    }
}
