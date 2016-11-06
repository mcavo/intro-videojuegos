using UnityEngine;
using System.Collections;

// Extends from BlinkingObject to regulate difficulty
public class Trap : BlinkingObject {

	public float reduceSpeed;
	public float reduceTime;

	void OnTriggerEnter(Collider col) {
		gameObject.SetActive (false);
	}
}
