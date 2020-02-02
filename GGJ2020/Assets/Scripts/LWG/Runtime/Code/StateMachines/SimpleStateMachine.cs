using System;
using System.Collections.Generic;

using UnityEngine;

namespace LWG.Core.Fsm {

public class SimpleStateMachine<T> 
where T : struct, IComparable, IFormattable, IConvertible // ENUM
{
	// Keyed list of states. Should be enumaated by a type. 
	Dictionary<T, IState> _states;

	// Should be an enumerated type.
	T _entry_state;

	// Should be a current state pointer for update and reference stuff. 
	T _current_state;

	// Where we are going to go next frame
	//T _next_state;

	public SimpleStateMachine() {
		if (!typeof(T).IsEnum) {
			throw new ArgumentException("T must be an enumerated type");
		}
		_states = new Dictionary<T, IState>();
		_current_state = default(T);
		_entry_state = default(T);
	}

	public void AddState(T key, IState state) {
		if(!_states.ContainsKey(key)) {
			_states[key] = state;
		}
		else {
			Debug.LogError("State already exists!");
		}
	}

	public IState RemoveState(T key) {
		// Can't delete the entry state
		IState state;
		if(_states.TryGetValue(key, out state) && !key.Equals(_entry_state)) { 
			_states.Remove(key);
			return state;
		}
		Debug.LogError("Doesn't Exist or Trying to delete Entry state!");
		return null;
	}

	// Sets the entry state for the system. 
	public void SetEntryState(T state) => _entry_state = state;
	public T GetState() => _current_state;
	
	public void Update() => _states[_current_state].Update();

	public void Reset() {
		if(!_current_state.Equals(_entry_state))
		{
			_states[_current_state].Exit();
			_current_state = _entry_state;
			_states[_current_state].Enter();
		}
	}


	public void ChangeState(T state) {
		// Should we transition?
		if(!state.Equals(_current_state)) {
			_states[_current_state].Exit();
			_current_state = state;
			//Debug.Log("transition to " + _current_state);
			_states[_current_state].Enter();
		}
	}

}
} // LWG