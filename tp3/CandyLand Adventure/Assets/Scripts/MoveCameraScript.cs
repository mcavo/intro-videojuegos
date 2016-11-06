using UnityEngine;
using System.Collections;

public class MoveCameraScript : MonoBehaviour {

	private Vector3 RotationVector = new Vector3(0,1f,0);
	public float RotationSpeed = 50f;
	public float MovementSpeed = 10f;

	// Use this for initialization
	void Start () {
	}
	
	void Update()
	{
		RaycastHit hit;

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.localEulerAngles += RotationVector * RotationSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.localEulerAngles -= RotationVector * RotationSpeed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			Vector3 v32 = transform.forward.normalized;
			Vector3 v3 = new Vector3 (v32.x, 1, v32.z);				 
			if (! Physics.Raycast (transform.localPosition, transform.forward, out hit, MovementSpeed * Time.deltaTime)) {
				transform.localPosition = transform.localPosition + transform.forward * MovementSpeed * Time.deltaTime;
			}
			if (Physics.Raycast (transform.localPosition, v3, out hit, 5, 9)) {
				Debug.Log ("Bonus!");
			}
			Debug.DrawLine (transform.localPosition, v3 * 5, Color.cyan, 2);
			//transform.localPosition = transform.position + transform.forward * MovementSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			Vector3 v32 = transform.forward.normalized;
			Vector3 v3 = new Vector3 (v32.x, 1, v32.z);
			if (! Physics.Raycast (transform.localPosition, -1 * transform.forward, out hit, MovementSpeed * Time.deltaTime)) {
				transform.localPosition = transform.localPosition - transform.forward * MovementSpeed * Time.deltaTime;
			}
			if (Physics.Raycast (transform.localPosition,  v3, out hit, 5, 9)) {
				Debug.Log ("Bonus!");
			}
			Debug.DrawLine (transform.localPosition, v3 * 5, Color.cyan, 2);
			//
			//transform.localPosition = transform.position - transform.forward * MovementSpeed * Time.deltaTime;
		}
	}
}
