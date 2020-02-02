using System;
using UnityEngine;

namespace LWG.Core.Fsm {

// Should be used as a state in the Finite state machine.
// Enter: Called when the system first enters the listed state
// Exit: Called when the system leaves the state
// Update: Called every frame by the state system update().  
public class TimerState : IState 
{
    public float CurrentTime {get; private set;} = 0f;

    readonly float _maxTime;
    public TimerState(float time) => _maxTime = time;

    public Action enter { get; set; } = () => {};
    public Action exit { get; set; } = () => {};
    public Action update { get; set; } = () => {};
    public Action timeout { get; set; } = () => {};

    public virtual void Enter() {
        // Reset the state timer! :D
        CurrentTime = 0f;
        enter();
    }

    public virtual void Exit() => exit();

    public virtual void Update() {
        // Do Timer state stuff. 
        CurrentTime += Time.deltaTime;
        // Check for timeout. 
        if(_maxTime >= 0 && CurrentTime >= _maxTime) {
            timeout();
        }       
        update();
    }
}

} // LWG