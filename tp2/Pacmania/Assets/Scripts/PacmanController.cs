using UnityEngine;
using System.Collections;

public class PacmanController : MonoBehaviour {

	public float MovementSpeed = 0f;

	private Animator animator;

	private Vector3 up = Vector3.zero,
	down = new Vector3(0,180,0),
	right = new Vector3(0,90,0),
	left = new Vector3(0,270,0),
	currentDirection = Vector3.zero;
	private Vector3 initialPosition = new Vector3(1,-7.5f,-7);

	public void Reset() {
		transform.position = initialPosition;
		animator.SetBool ("IsDead", false);
		animator.SetBool ("IsMoving", false);
		currentDirection = right;
	}

	void Start() {
		QualitySettings.vSyncCount = 0;
		animator = GetComponent<Animator> ();
		Reset();
	}

	void Update() {
		var isMoving = true;
		var isDead = animator.GetBool ("IsDead");

		if (isDead) {
			isMoving = false;
		} else if (Input.GetKey(KeyCode.UpArrow)) {
			currentDirection = up;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			currentDirection = down;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			currentDirection = right;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			currentDirection = left;
		} else {
			isMoving = false;
		}

		transform.localEulerAngles = currentDirection;
		animator.SetBool ("IsMoving", isMoving);

		if (isMoving) {
			transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Ghost")) {
			animator.SetBool("IsDead", true);
		}
	}
}
