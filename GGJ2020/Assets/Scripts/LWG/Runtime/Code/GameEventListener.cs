using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {

	[SerializeField] GameEvent gameEvent = null;
	[SerializeField] UnityEvent response = null;

	// Use this for initialization
	void OnEnable () => gameEvent.RegisterListener(this);
	
	// Update is called once per frame
	void OnDisable () => gameEvent.UnregisterListener(this);

	public void OnEventRaised() => response.Invoke();
}
