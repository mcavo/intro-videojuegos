using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	// Use this for initialization
	void Start () {
			
	}

	void OnTriggerEnter(Collider col) {
		gameObject.SetActive (false);
	}
}
