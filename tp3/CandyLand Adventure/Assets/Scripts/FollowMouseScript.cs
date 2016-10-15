using UnityEngine;
using System.Collections;

public class FollowMouseScript : MonoBehaviour {

	private Vector3 RotationVector = new Vector3(0,1f,0);
	public float RotationSpeed = 50f;
	public float MovementSpeed = 10f;

	// Use this for initialization
	void Start () {
	}
	
	void Update()
	{
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.localEulerAngles += RotationVector * RotationSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.localEulerAngles -= RotationVector * RotationSpeed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.localPosition = transform.position + transform.forward * MovementSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			transform.localPosition = transform.position - transform.forward * MovementSpeed * Time.deltaTime;
		}
	}
}
