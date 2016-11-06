using UnityEngine;
using System.Collections;

public class MoveCameraScript : MonoBehaviour {

	private Vector3 RotationVector = new Vector3(0,1f,0);
	public float RotationSpeed = 50f;
	public float MovementSpeed = 10f;
	
	void Update()
	{
		RaycastHit hit;

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.localEulerAngles += RotationVector * RotationSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.localEulerAngles -= RotationVector * RotationSpeed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (! Physics.Raycast (transform.localPosition, transform.forward, out hit, MovementSpeed * Time.deltaTime)) {
				transform.localPosition = transform.localPosition + transform.forward * MovementSpeed * Time.deltaTime;
			}
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (! Physics.Raycast (transform.localPosition, -1 * transform.forward, out hit, MovementSpeed * Time.deltaTime)) {
				transform.localPosition = transform.localPosition - transform.forward * MovementSpeed * Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Trap") {
			trapped (col.GetComponent<Trap>());	
		} else if (col.tag == "Bonus") {
			Debug.Log ("Bonus");
		}	
	}

	void trapped(Trap trap) {
		// TODO: Add camera effect
	}
}
