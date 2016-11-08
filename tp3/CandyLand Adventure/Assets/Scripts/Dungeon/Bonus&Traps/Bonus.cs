using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	public float incrementSpeed = 5f;
	public float incrementTime = 1f;
	public float duration = 1f;

	void OnTriggerEnter(Collider col) {
		GameManager.instance.TimeLeft -= incrementTime;
		gameObject.SetActive (false);
	}
}
