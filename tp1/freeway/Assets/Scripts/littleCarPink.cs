using UnityEngine;
using System.Collections;

public class littleCarPink : MonoBehaviour {
	public static float assetsSize = 0.36f;
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
		// TODO: Por qué tengo que poner ese 100 inmundo?? 
		if (speed > 0) {
			if (transform.position.x < 0) { 
				transform.position = new Vector3 (17*assetsSize, transform.position.y, transform.position.z);
			}
		} else {
			if (transform.position.x > 17*assetsSize) { 
				transform.position = new Vector3 (0, transform.position.y, transform.position.z);
			}
		}
	}
}
