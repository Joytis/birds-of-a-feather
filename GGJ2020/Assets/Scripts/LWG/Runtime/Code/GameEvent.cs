using System.Collections.Generic;
using UnityEngine;

// A way of having project-scope game events that can be access with 'GameEventListener's
[CreateAssetMenu(fileName = "ge_NewGameEvent", menuName = "LWG/GameEvent")]
public class GameEvent : ScriptableObject {

	List<GameEventListener> listeners = new List<GameEventListener>();

	public void Raise() => listeners.ForEach(e => e.OnEventRaised());
	
	public void RegisterListener(GameEventListener listener) => listeners.Add(listener);
	public void UnregisterListener(GameEventListener listener) => listeners.Remove(listener);
}
