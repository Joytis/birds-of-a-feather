using System;
using UnityEngine;

namespace LWG.Core.Fsm {

// Should be used as a state in the Finite state machine.
// Enter: Called when the system first enters the listed state
// Exit: Called when the system leaves the state
// Update: Called every frame by the state system update().  
public class State : IState 
{
	public Action enter { get; set; } = () => {};
	public Action exit { get; set; } = () => {};
	public Action update { get; set; } = () => {};

	public virtual void Enter() => enter();
	public virtual void Exit() => exit();
	public virtual void Update() => update();
}

} // LWG