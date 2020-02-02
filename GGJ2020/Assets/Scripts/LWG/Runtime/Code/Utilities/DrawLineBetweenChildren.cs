using System.Collections.Generic;
using UnityEngine;

public class DrawLineBetweenChildren : MonoBehaviour {

	public Color color;
	List<Transform> objs = new List<Transform>();

	void OnDrawGizmos() { // Dawg we don't give a shit about efficency here B)
		objs.Clear();
    	foreach (Transform t in transform) {
    		objs.Add(t);
    	}

		for(int i = 0; i < objs.Count - 1; i++) {
			Gizmos.color = color;
			Gizmos.DrawLine(objs[i].position, objs[i+1].position);
		}
    }
}
