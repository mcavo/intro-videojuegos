using UnityEngine;
using System.Collections;

public class littleCarPink : MonoBehaviour {
	public static float assetsSize = 0.36f;
	public float speed = 1f;
	public float initialDistance;
	public int difficult = 1;
	public int numberInRows = 1;
	public Vector3 initialPosition;

	// Use this for initialization
	void Start () {
//		initialPosition = new Vector3 (initialPosition.x*assetsSize, initialPosition.y*assetsSize, 0);
//		transform.position = initialPosition;
	}

	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = transform.position;
		transform.position = new Vector3 (currentPosition.x - speed*difficult, currentPosition.y, currentPosition.z);
		checkBoundaries ();
	}

	public void initialize(int position) {
		initialPosition = new Vector3 ((((position - 1)  * initialDistance + initialPosition.x) % 18) * assetsSize, initialPosition.y * assetsSize, initialPosition.z);
		transform.position = initialPosition;
	}

	private void checkBoundaries() {
		//x = 0 is the mid of the screen
		if (speed*difficult > 0) {
			if (transform.position.x < 0) { 
				transform.position = new Vector3 ((17 + initialDistance)*assetsSize, transform.position.y, transform.position.z);
			}
		} else {
			if (transform.position.x > 17*assetsSize) { 
				transform.position = new Vector3 ((0 - initialDistance), transform.position.y, transform.position.z);
			}
		}
	}
}
