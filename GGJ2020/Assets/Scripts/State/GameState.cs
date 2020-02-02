using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using LWG.Core.Fsm;
using Cinemachine;
using TMPro;

public class GameState : MonoBehaviour {
    enum States {
        Playing,
        Paused,
        End,
    }

    enum Triggers {
        PausePressed,
        EndHappened,
    }

    const float _fadeTime = 0.5f;

    FiniteStateMachine<States, Triggers> _fsm = new FiniteStateMachine<States, Triggers>(States.Playing);

    [SerializeField] AudioFader _ambiance = null;
    [SerializeField] BirdChannelRuntimeSet _birdSet = null;
    [SerializeField] CollectableBirdRuntimeSet _collectableSet = null;
    [SerializeField] CinemachineVirtualCamera _finalCamera = null;
    [SerializeField] GameUI _gameUI = null;
    
    void ClearSets() {
        _birdSet.Clear();
        _collectableSet.Clear();
    }

    void Awake() {
        Time.timeScale = 1f;        
        ConstructStateMachine();
        ClearSets();
    }

    public void PressPause() => _fsm.SetTrigger(Triggers.PausePressed);

    void ConstructStateMachine() {
        // States.
        _fsm.AddState(States.Playing, new State() {
            enter = () => {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                _ambiance.FadeIn(_fadeTime);
                Time.timeScale = 1f;
            },
            exit = () => {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        });

        _fsm.AddState(States.Paused, new State() {
            enter = () => {
                Time.timeScale = 0f;
                _gameUI.ShowPause();
                _ambiance.FadeTo(0.4f, _fadeTime);
            },
            exit = () =>  {
                _gameUI.HidePause();
                _ambiance.FadeIn(_fadeTime);
            },
        });

        _fsm.AddState(States.End, new State() {
            enter = () => {
                _gameUI.DropInWinScren();
                _ambiance.FadeTo(0.7f, _fadeTime);
            },
        });

        _fsm.AddTransition(States.Playing, States.Paused, Triggers.PausePressed);
        _fsm.AddTransition(States.Playing, States.End, Triggers.EndHappened);

        _fsm.AddTransition(States.Paused, States.Playing, Triggers.PausePressed);
        _fsm.AddTransition(States.Paused, States.End, Triggers.EndHappened);
    }

    IEnumerator Start() {
        _fsm.Start();
        // Wait for the game to be over. 
        yield return null;
        // Wait for us to actually have enough birds to win. 
        while(_birdSet.Count < _collectableSet.Count) {
            yield return null;
        }

        // Disable the actors we know about. 
        foreach(var item in _collectableSet.Items) {
            item.Disable();
        }
        PlayerBirdController.Instance.Disable();

        // Set priority arbitrarily high. 
        _finalCamera.Priority = 100;
        _fsm.SetTrigger(Triggers.EndHappened);

        // Mark Game as complete. 
        SaveData.CompleteGame();
    }

    void Update() {
        // Do the jump
        if (Input.GetButtonDown("Pause")) {
            _fsm.SetTrigger(Triggers.PausePressed);
        }

        _fsm.Update(Time.deltaTime);
    }
}