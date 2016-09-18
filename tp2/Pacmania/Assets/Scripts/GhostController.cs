using UnityEngine;
using System.Collections;

public class GhostController : ObserverPattern.Observer {

	private System.Random rnd = new System.Random(new System.DateTime().Millisecond);
	public float MovementSpeed = 10f;
	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-37,41};

	private Vector3 up = Vector3.zero,
		down = new Vector3(0,180,0),
		right = new Vector3(0,90,0),
		left = new Vector3(0,270,0),
		currentDirection = Vector3.zero;

	private Vector3 initialPosition = new Vector3(1,-6.2f,9);

	public void Reset() {
		transform.position = initialPosition;
		currentDirection = down;
	}

	private bool AtCheckPoint() {
		Vector3 pos = transform.position;
		return Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.x / 4f, 2) - Mathf.Floor (pos.x / 4f))) &&
			Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.z / 4f, 2) - Mathf.Floor (pos.z / 4f)));
	}

	//TODO
	private void UpdateDirection() {
		
		ArrayList posibleDirections = new ArrayList();

		ifCanMoveAddToArrayList (up, posibleDirections);
		ifCanMoveAddToArrayList (down, posibleDirections);
		ifCanMoveAddToArrayList (left, posibleDirections);
		ifCanMoveAddToArrayList (right, posibleDirections);
		currentDirection = (Vector3)posibleDirections[rnd.Next(0, posibleDirections.Count)];

	}

	private bool NotBackDirection( Vector3 posibleAngle) {
		posibleAngle = RoundVector3 (posibleAngle, 2);
		Vector3 currentAngle = RoundVector3 (transform.localEulerAngles, 2);
		if (currentAngle == up) {
			return posibleAngle != down;
		} else if (currentAngle == down) {
			return posibleAngle != up;
		} else if (currentAngle == left) {
			return posibleAngle != right;
		} else if (currentAngle == right) {
			return posibleAngle != left;
		} else {
			Debug.Log ("Problem detected!");
			return false;
		}
	}

	private void ifCanMoveAddToArrayList( Vector3 angle, ArrayList posibleDirections) {
		if (NotBackDirection (angle)) {
			transform.localEulerAngles = angle;
			if (CanMove (GetBoardPosition (RoundVector3 (transform.position + transform.forward * movementOffset, 2)))) {
				posibleDirections.Add (angle);
			}
			transform.localEulerAngles = currentDirection;
		}
	}

	private bool CanMove( int[] toPosition ) {
		return GameManager.instance.board [toPosition[1], toPosition[0]] == 0;
	}

	private int[] GetBoardPosition( Vector3 position ) {
		return new int[2] { (int) ((position.x - matrixOffset [0]) / movementOffset), (int) ((matrixOffset [1] - position.z) / movementOffset) };
	}

	void Start() {
		QualitySettings.vSyncCount = 0;
		Reset();
		ObserverPattern.Subject.getInstance ().AddObserver (this); //Subscribe to notification
	}

	void Update() {
		var isMoving = true;
		var isDead = false; //TODO

		if (isDead) {
			isMoving = false;
		} 

		if (AtCheckPoint ()) {
			UpdateDirection ();
			transform.localEulerAngles = currentDirection;
		}


		if (isMoving) {
			transform.Translate(RoundVector3(Vector3.forward *(movementOffset/ MovementSpeed), 2));
		}
	}

	private Vector3 RoundVector3 ( Vector3 v, int decimals) {
		return new Vector3 ((float)System.Math.Round (v.x, decimals), (float)System.Math.Round (v.y, decimals), (float)System.Math.Round (v.z, decimals));
	}

	override public void OnNotify() {
		Debug.Log ("Hola");
	}

}
