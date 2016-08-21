using UnityEngine;
using System.Collections;

public class chicken : MonoBehaviour {
	public float speed = 0.05f;
	//TODO: check if it should not be public. 
	//TODO: same problem with dimensions of screen
	private Vector3 initialPosition = new Vector3(0, 1f*0.35f, 0);


	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	private void checkInput() {
		Vector3 currentPosition = transform.position;
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position = new Vector3 (currentPosition.x, currentPosition.y + speed, currentPosition.z);			
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			if (currentPosition.y - speed < 1f * 0.35f) {
				transform.position = initialPosition;
			} else {
				transform.position = new Vector3 (currentPosition.x, currentPosition.y - speed, currentPosition.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col2d) {
		Debug.Log (col2d.tag);
		if (col2d.tag.Equals ("LittlePinkCar")) {
			Debug.Log ("perdí :(");
			transform.position = initialPosition;
		}

		if (col2d.tag.Equals("EndRoad")) {
			Debug.Log ("Pasé uno");
			transform.position = initialPosition;
		}
	}
}
