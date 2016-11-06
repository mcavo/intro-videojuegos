using UnityEngine;
using System.Collections;

public class Bonus : BlinkingObject {

	void OnTriggerEnter(Collider col) {
		StopBlinking ();
		gameObject.SetActive (false);
	}
}
