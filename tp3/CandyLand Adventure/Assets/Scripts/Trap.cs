using UnityEngine;
using System.Collections;

// Extends from BlinkingObject to regulate difficulty
public class Trap : BlinkingObject {

	public float reduceSpeed = 5;
	public float reduceTime = 1;
	public float duration = 1f;

	void OnTriggerEnter(Collider col) {
		gameObject.SetActive (false);
	}
}
