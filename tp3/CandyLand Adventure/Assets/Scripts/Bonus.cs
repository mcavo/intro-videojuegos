using UnityEngine;
using System.Collections;

public class Bonus : BlinkingObject {

	public int points;
	public float speed;
	public float time;

	void OnTriggerEnter(Collider col) {
		StopBlinking ();
		gameObject.SetActive (false);
	}
}
