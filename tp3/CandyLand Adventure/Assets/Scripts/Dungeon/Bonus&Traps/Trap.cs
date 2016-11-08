using UnityEngine;
using System.Collections;

// Extends from BlinkingObject to regulate difficulty
public class Trap : MonoBehaviour {
	
	public float reduceSpeed = 5f;
	public float reduceTime = 1f;
	public float duration = 1f;

	void OnTriggerEnter(Collider col) {
		GameManager.instance.TimeLeft += reduceTime;
		gameObject.SetActive (false);
	}
}
