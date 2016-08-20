using UnityEngine;
using System.Collections;

public class littleCarPink : MonoBehaviour {

	public float speed = 1f;

	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Debug.Assert(rb != null, "Cars needs to have a RigidBody when initialize");
	}

	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = transform.position;
		transform.position = new Vector3(currentPosition.x - speed, currentPosition.y, currentPosition.z);
		checkBoundaries();
	}

	private void checkBoundaries() {
		//x = 0 is the mid of the screen
		// Por qué tengo que poner ese 100 inmundo?? 
		if (transform.position.x < Screen.width / -100f) { 
			transform.position = new Vector3 (Screen.width/100, transform.position.y, transform.position.z);
		}
	}
}
