using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PacmanController : MonoBehaviour {

	public float MovementSpeed = 0f;

	private int pointsPerPacDot = 60;
	private int pointsPerPowerPellet = 300;
	private int pointsPerCherry = 1000;
	private int pointsPerGhost = 1000;
	private int score;
	private float deltaMovement;

	private Animator animator;

	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-37,41};

//	private bool isJumpingUp = false;
//	private bool isJumpingDown = false;
//	private int jumpCount = 0;
	private bool isJumping;
	private float jumpingDistance;

	private Vector3 up = Vector3.zero,
					down = new Vector3(0,180,0),
					right = new Vector3(0,90,0),
					left = new Vector3(0,270,0),
					nextDirection = Vector3.zero,
					currentDirection = Vector3.zero;
	private Vector3 InitialPosition = new Vector3(1,-7.5f,-7);
	private Vector3 CheckPointPosition = new Vector3(1,-7.5f,-7);

	public void Reset() {
		transform.position = InitialPosition;
		animator.SetBool ("IsDead", false);
		animator.SetBool ("IsMoving", false);
		isJumping = false;
		jumpingDistance = 0;
		currentDirection = down;
		nextDirection = down;
	}

	private void CheckInput() {
		if (Input.GetKey (KeyCode.UpArrow)) {
			nextDirection = up;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			nextDirection = down;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			nextDirection = right;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			nextDirection = left;
		} else if (Input.GetKey(KeyCode.Space) && !isJumping) {
			isJumping = true;
		}
	}

	private bool CrossedCheckPoint(Vector3 nextPosition) {
		Vector3 posibleCheckPoint;
		float num;
		if (currentDirection == up && Mathf.CeilToInt (transform.position.z) == Mathf.FloorToInt (nextPosition.z)) {
			posibleCheckPoint = new Vector3 (transform.position.x, transform.position.y, Mathf.FloorToInt (nextPosition.z));
			num = Mathf.Floor (nextPosition.z);
			deltaMovement = nextPosition.z - num;
		} else if (currentDirection == down && Mathf.FloorToInt (transform.position.z) == Mathf.CeilToInt (nextPosition.z)) {
			posibleCheckPoint = new Vector3 (transform.position.x, transform.position.y, Mathf.CeilToInt (nextPosition.z));
			num = Mathf.Ceil (nextPosition.z);
			deltaMovement = num - nextPosition.z;
		} else if (currentDirection == right && Mathf.CeilToInt (transform.position.x) == Mathf.FloorToInt (nextPosition.x)) {
			posibleCheckPoint = new Vector3 (Mathf.FloorToInt (nextPosition.x), transform.position.y, transform.position.z);
			num = Mathf.Floor (nextPosition.x);
			deltaMovement = nextPosition.x - num;
		} else if (currentDirection == left && Mathf.FloorToInt (transform.position.x) == Mathf.CeilToInt (nextPosition.x)) {
			posibleCheckPoint = new Vector3 (Mathf.CeilToInt (nextPosition.x), transform.position.y, transform.position.z);
			num = Mathf.Ceil (nextPosition.x);
			deltaMovement = num - nextPosition.x;
		} else {
			return false;
		}

		if ( Mathf.Approximately(0.25f, (float)(System.Math.Round (num / 4f, 2) - Mathf.Floor (num / 4f))) ) {
			CheckPointPosition = RoundVector3 (posibleCheckPoint, 1);
			return true;
		}
		return false;
	}

	private void UpdateDirection() {
		if (currentDirection != nextDirection) {
			transform.localEulerAngles = nextDirection;
			if (CanMove (GetBoardPosition(RoundVector3(CheckPointPosition + transform.forward * movementOffset, 0)))) {
				currentDirection = nextDirection;
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

	private float GetJump() {
		float a = -0.25f, b=2.0f;
		float jump = a * Mathf.Pow (jumpingDistance, 2) + b * jumpingDistance;
		if (jump < 0) {
			isJumping = false;
			jumpingDistance = 0;
			jump = 0;
		}
		return jump;

	}

	void Start() {
		QualitySettings.vSyncCount = 0;
		animator = GetComponent<Animator> ();
		score = GameManager.instance.score;
		Reset();
	}

	void Update() {
		var isMoving = true;
		var isDead = animator.GetBool ("IsDead");
		transform.localEulerAngles = currentDirection;
		Vector3 nextPosition = transform.position + transform.forward * MovementSpeed * Time.deltaTime;

		if (isDead) {
			isMoving = false;

		} else {

			CheckInput ();

			if (isJumping) {
				jumpingDistance += MovementSpeed * Time.deltaTime;
			}

			if (CrossedCheckPoint (nextPosition)) {
				UpdateDirection ();
				if (!CanMove (GetBoardPosition (CheckPointPosition + transform.forward * movementOffset))) {
					isMoving = false;
					nextPosition = CheckPointPosition;
				} else {
					nextPosition = CheckPointPosition + transform.forward * deltaMovement;
				}
			}

			if (!CanMove (GetBoardPosition (CheckPointPosition + transform.forward * movementOffset))) {
				isMoving = false;
				nextPosition = CheckPointPosition;
			}

			nextPosition.y = InitialPosition.y + GetJump ();

			animator.SetBool ("IsMoving", isMoving);

			transform.position = nextPosition;

		}


//		if (CrossedCheckPoint ()) {
//			UpdateDirection ();
//			transform.localEulerAngles = currentDirection;
//			if ( !CanMove (GetBoardPosition(RoundVector3(transform.position + transform.forward * movementOffset,2)))) {
//				isMoving = false;
//			}
//		}
//			
//		if (isJumpingUp) {
//			jumpCount++;
//			jumpintVector = Vector3.up * 3 / (jumpCount + 1);
//			Debug.Log (jumpintVector);
//			if (jumpCount == MovementSpeed) {
//				jumpCount = 0;
//				isJumpingUp = false;
//				isJumpingDown = true;
//			}	
//		} else if (isJumpingDown) {
//			jumpCount++;
//			jumpintVector = Vector3.down * 3 / (MovementSpeed + 2 - jumpCount);
//			Debug.Log (jumpintVector);
//			if (jumpCount == MovementSpeed) {
//				jumpCount = 0;
//				isJumpingDown = false;
//			}
//		} else {
//			jumpintVector = Vector3.zero;	
//		}
//			
//
//		//transform.localEulerAngles = currentDirection;
//		animator.SetBool ("IsMoving", (isMoving || isJumpingDown || isJumpingUp));
//
//		Vector3 movingVector = jumpintVector;
//		if (isMoving) {
//			Debug.Log (movementOffset / MovementSpeed + " - " + Time.deltaTime);
//			movingVector += RoundVector3 (Vector3.forward * (movementOffset / MovementSpeed), 2);
//		} 
//		transform.Translate (movingVector);
	}

	private Vector3 RoundVector3 ( Vector3 v, int decimals) {
		return new Vector3 ((float)System.Math.Round (v.x, decimals), (float)System.Math.Round (v.y, decimals), (float)System.Math.Round (v.z, decimals));
	}

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Ghost")) {
			animator.SetBool("IsDead", true);
		} else if (col.CompareTag("PacDot")) {
			IncrementScore (pointsPerPacDot);
		} else if (col.CompareTag("PowerPellet")) {
			IncrementScore (pointsPerPowerPellet);
		} else if(col.CompareTag("Cherry")) {
			IncrementScore (pointsPerCherry);
			GameManager.instance.AddCherry ();
		} else if(col.CompareTag("EatableGhost")) {
			IncrementScore (pointsPerGhost);
		}
	}

	void IncrementScore(int points) {
		score += points;
		GameManager.instance.score = score;
		Text scoreText = GameObject.Find("Score").GetComponent<Text>();
		Text scoreBorderText = GameObject.Find("ScoreBorder").GetComponent<Text>();
		scoreText.text = score.ToString ();
		scoreBorderText.text = score.ToString ();
	}
}
