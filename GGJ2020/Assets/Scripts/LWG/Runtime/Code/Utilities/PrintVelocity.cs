using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWG {

[RequireComponent(typeof(Rigidbody2D))]
public class PrintVelocity : MonoBehaviour {
	
	Rigidbody2D _rb2d;

	void Awake() => _rb2d = GetComponent<Rigidbody2D>();

	// Update is called once per frame
	void Update () => Debug.Log(_rb2d.velocity);
}

}