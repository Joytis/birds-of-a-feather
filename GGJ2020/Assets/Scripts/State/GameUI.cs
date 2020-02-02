using UnityEngine;
using System.Collections;
using LWG.Core.Fsm;
using TMPro;

[RequireComponent(typeof(Animator))]
public class GameUI : MonoBehaviour {
    Animator _animator = null;

    void Awake() {
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void DropInWinScren() => _animator.Play("DropIn");
    public void ShowPause() => _animator.Play("ShowPause");
    public void HidePause() => _animator.Play("HidePause");
}