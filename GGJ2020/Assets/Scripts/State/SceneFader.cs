using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game/SceneFader")]
public class SceneFader : ScriptableObject
{
    [SerializeField] GameObject _fader = null;
    const float _fadeTime = 1f;


    public void LoadScene(string name) {
        var newFader = Instantiate(_fader);
        var dummy = newFader.AddComponent<DummyBinding>();
        dummy.StartCoroutine(InternalFadeScene(newFader, name));
    }

    IEnumerator InternalFadeScene(GameObject fader, string scene) {
        DontDestroyOnLoad(fader);

        var canvasGroup = fader.GetComponent<CanvasGroup>();

        IEnumerator DoTime(float time, Action<float> factorThing) {
            float currentTime = 0f;
            while(currentTime < time) {
                var factor = currentTime / time;
                factorThing(factor);
                yield return null;
                currentTime += Time.unscaledDeltaTime;
            }
            factorThing(1f);
        }

        yield return DoTime(_fadeTime, factor => canvasGroup.alpha = factor);
        yield return SceneManager.LoadSceneAsync(scene);
        yield return DoTime(_fadeTime, factor => canvasGroup.alpha = 1 - factor);
        Destroy(fader);
    }

}
