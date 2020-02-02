using UnityEngine;

public class MapToSine : MonoBehaviour {

	[Header("In seconds... ")]
	public float period = 1f;
	[Header("In Units... ")]
	public float amplitude = 1f;
	[Header("This gets normalized at awake")]
	public Vector3 direction = Vector3.zero;

	const float twoPi = 2 * Mathf.PI; 

	Vector3 startingPosition;
	Vector3 currentOffset;
	float magnitude;
	float rads;

	float currentTime = 0f;

	void Awake() {
		direction = direction.normalized;
		startingPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		// Map the current position to a sine wave in space. 
		currentTime += Time.deltaTime;
		rads = (currentTime / period) * twoPi;
		magnitude = Mathf.Sin(rads) * amplitude;
		currentOffset = startingPosition + (direction * magnitude);
		transform.localPosition = currentOffset;
	}
}
