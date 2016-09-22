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

	private Animator animator;

	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-37,41};

	private Vector3 up = Vector3.zero,
					down = new Vector3(0,180,0),
					right = new Vector3(0,90,0),
					left = new Vector3(0,270,0),
					nextDirection = Vector3.zero,
					currentDirection = Vector3.zero;
	private Vector3 initialPosition = new Vector3(1,-7.5f,-7);

	public void Reset() {
		transform.position = initialPosition;
		animator.SetBool ("IsDead", false);
		animator.SetBool ("IsMoving", false);
		currentDirection = down;
		nextDirection = down;
	}

	private bool AtCheckPoint() {
		Vector3 pos = transform.position;
		return Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.x / 4f, 2) - Mathf.Floor (pos.x / 4f))) &&
		Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.z / 4f, 2) - Mathf.Floor (pos.z / 4f)));
	}

	private void UpdateDirection() {
		if (currentDirection != nextDirection) {
			transform.localEulerAngles = nextDirection;
			if (CanMove (GetBoardPosition(RoundVector3(transform.position + transform.forward * movementOffset, 2)))) {
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

	void Start() {
		QualitySettings.vSyncCount = 0;
		animator = GetComponent<Animator> ();
		score = GameManager.instance.score;
		Reset();
	}

	void Update() {
		var isMoving = true;
		var isDead = animator.GetBool ("IsDead");

		if (isDead) {
			isMoving = false;
		} else if (Input.GetKey(KeyCode.UpArrow)) {
			nextDirection = up;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			nextDirection = down;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			nextDirection = right;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			nextDirection = left;
		}

		if (AtCheckPoint ()) {
			UpdateDirection ();
			transform.localEulerAngles = currentDirection;
			if ( !CanMove (GetBoardPosition(RoundVector3(transform.position + transform.forward * movementOffset,2)))) {
				isMoving = false;
			}
		}

		//transform.localEulerAngles = currentDirection;
		animator.SetBool ("IsMoving", isMoving);

		if (isMoving) {
			transform.Translate(RoundVector3(Vector3.forward *(movementOffset/ MovementSpeed), 2));
		}
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
