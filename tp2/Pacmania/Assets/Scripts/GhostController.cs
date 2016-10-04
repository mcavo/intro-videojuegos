using UnityEngine;
using System.Collections;



public class GhostController : ObserverPattern.Observer {
	
	public Color BodyColor = new Color(0, 0, 0, 1);
	private Color EyesColor = new Color(0, 0, 0, 1);
	private Color BodyColorEatable = new Color(0.18039f, 0.14510f, 0.70196f, 1.0f);
	private Color EyesColorEatable = new Color(0.97647f, 0.97647f, 0.97647f, 1.0f);
	private float SolidAlpha = 1.0f;
	private float TransparentAlpha = 0.3f;
	private float NotSoTransparentAlpha = 0.7f;

	private float MovementSpeed = 10f;
	private float deltaMovement = 0;

	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-37,41};

	private BoxCollider boxCollider;      //The BoxCollider2D component attached to this object.
	private Rigidbody rb;               //The Rigidbody2D component attached to this object.

	private bool isAlive = true;
	private bool isEatable = false;
	private bool justAwake = true;

	public int xMatrixPosition = 7; // 7 / 19
	public int yMatrixPosition = 9; // 9 22

	private Vector3[,] directionsBoard = null;

	private Coroutine co = null;

	private Vector3 up = Vector3.zero,
		down = new Vector3(0,180,0),
		right = new Vector3(0,90,0),
		left = new Vector3(0,270,0),
		currentDirection = Vector3.zero;

	public Vector3 initialPosition = new Vector3(-7,-6.2f,5);
	private Vector3 CheckPointPosition;

	private void SetScapeBoard(int xMatrixPosition, int yMatrixPosition) {
		// en x me muevo de 7 a 11
		// en y me muevo de 9 a 11
		for (int i = 7; i < 12; i++) {
			for (int j = 9; j < 12; j++) {
				if (xMatrixPosition < 9) {
					directionsBoard [j, i] = left;
				} else {
					directionsBoard [j, i] = right;
				}
			}
		}
		for (int i = 9; i < yMatrixPosition; i++) {
			directionsBoard [i, 9] = down;
		}
	}

	private System.Random rnd = new System.Random(new System.DateTime().Millisecond);

	void Start() {
		QualitySettings.vSyncCount = 0;
		transform.position = initialPosition;
		CheckPointPosition = initialPosition;
		directionsBoard = (Vector3[,])GameManager.instance.directions.Clone ();
		SetScapeBoard(xMatrixPosition, yMatrixPosition);
		Reset();
		ObserverPattern.Subject.getInstance ().AddObserver (this); //Subscribe to notification
	}

	public void Reset() {
		isEatable = false;
		SetGhostColor (BodyColor, EyesColor, SolidAlpha);
		transform.gameObject.tag = "Ghost";
		currentDirection = down;
	}

	private void SetGhostColor(Color body, Color eyes, float alpha) {
		body.a = alpha;
		eyes.a = alpha;
		transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].color = body;
		transform.GetChild(1).GetComponent<MeshRenderer>().materials[0].color = eyes;
		transform.GetChild(2).GetComponent<MeshRenderer>().materials[0].color = eyes;
	}

	void Update() {

		var isMoving = true;
		transform.localEulerAngles = currentDirection;
		Vector3 nextPosition = transform.position + transform.forward * MovementSpeed * Time.deltaTime;

		if (CrossedCheckPoint (nextPosition)) {
			int[] position = GetBoardPosition (CheckPointPosition);
			if (isAlive || position [0] != xMatrixPosition || position [1] != yMatrixPosition) {
				if (GameManager.instance.board [position [1], position [0]] != 2) {
					justAwake = false;
				}

				if (isAlive && !justAwake) {
					MovementSpeed = 10f;
					if (isEatable) {
						MovementSpeed = 8f;
					}
					UpdateDirection ();
				} else {
					if (justAwake) {
						MovementSpeed = 10f;
					} else {
						MovementSpeed = 8f;
					}
					isMoving = UpdateDirectedDirection ();
				}
				transform.localEulerAngles = currentDirection;
				nextPosition = CheckPointPosition + transform.forward * deltaMovement;
			} else {
				transform.localEulerAngles = down;
				nextPosition = CheckPointPosition;
				isMoving = false;
			}
		}

		if (isMoving) {
			transform.position = nextPosition;
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

		RaycastHit hit;
		ArrayList posibleDirections = new ArrayList();

		ifCanMoveAndNotOppositeAddToArrayList (up, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (down, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (left, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (right, posibleDirections);

		Vector3 curPosition = CheckPointPosition;
		Vector3 targetPosition;

		foreach (var angle in posibleDirections) {
			transform.localEulerAngles = (Vector3) angle;

			targetPosition = RoundVector3 (220 * 3 * movementOffset * transform.forward + transform.position, 2);
			if (Physics.Raycast (curPosition, targetPosition, out hit)) {
				if (hit.collider.tag == "Pacman") {
					if (isEatable) {
						ifCanMoveAddToArrayList (OppositeDirection(currentDirection), posibleDirections);
						posibleDirections.Remove (angle);
						break;
					} else {
						currentDirection = (Vector3)angle;
						return;
					}
				}
			}
		}

		currentDirection = (Vector3)posibleDirections[rnd.Next(0, posibleDirections.Count)];

	}

	private bool UpdateDirectedDirection() {
		currentDirection = GetNewDirection (CheckPointPosition);
		transform.localEulerAngles = currentDirection;
		return CanMove (GetBoardPosition (CheckPointPosition + transform.forward * movementOffset));
	}

	private Vector3 OppositeDirection( Vector3 angle) {
		if (angle == up) {
			return down;
		} else if (angle == down) {
			return up;
		} else if (angle == left) {
			return right;
		} else if (angle == right) {
			return left;
		} else {
			Debug.Log ("Problem detected!");
			return Vector3.zero;
		}
	}

	private void ifCanMoveAndNotOppositeAddToArrayList(Vector3 angle, ArrayList posibleDirections) {
		if (currentDirection != OppositeDirection (angle)) {
			ifCanMoveAddToArrayList (angle, posibleDirections);
		}
	}

	private void ifCanMoveAddToArrayList( Vector3 angle, ArrayList posibleDirections) {
		transform.localEulerAngles = angle;
		if (CanMove (GetBoardPosition (CheckPointPosition + transform.forward * movementOffset))) {
			posibleDirections.Add (angle);
		}
		transform.localEulerAngles = currentDirection;
	}

	private bool CanMove( int[] toPosition ) {
		int canMove = GameManager.instance.board [toPosition [1], toPosition [0]];
		if ( canMove == 0) {
			return true;
		}
		return canMove == 0 || (canMove == 2 && justAwake) || (canMove == 2 && !isAlive) ;
	}

	private Vector3 GetBoardDirection( int[] position ) {
		if (GameManager.instance.board [position [1], position [0]] == 2 && !isAlive) {
			return directionsBoard [position[1], position[0]];
		}
		return GameManager.instance.directions [position[1], position[0]];
	}

	private int[] GetBoardPosition( Vector3 position ) {
		return new int[2] { (int) ((position.x - matrixOffset [0]) / movementOffset), (int) ((matrixOffset [1] - position.z) / movementOffset) };
	}

	private Vector3 GetNewDirection( Vector3 position ) {
		int[] pos = GetBoardPosition (position);
		return GetBoardDirection (pos);
	}

	private Vector3 RoundVector3 ( Vector3 v, int decimals) {
		return new Vector3 ((float)System.Math.Round (v.x, decimals), (float)System.Math.Round (v.y, decimals), (float)System.Math.Round (v.z, decimals));
	}

	override public void OnNotify() {
		if(transform.gameObject.tag != "EatenGhost") {
			SetGhostColor (BodyColorEatable, EyesColorEatable, SolidAlpha);
			transform.gameObject.tag = "EatableGhost";
			isEatable = true;
			if (co != null) {
				StopCoroutine (co);
			}
			co = StartCoroutine(EatableRoutine());
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Pacman") && isEatable) {
			transform.gameObject.tag = "EatenGhost";
			EatenProperties ();
			if (co != null) {
				StopCoroutine (co);
			}

			co = StartCoroutine(GoBackRoutine());
		}
	}

	public IEnumerator GoBackRoutine()
	{
		while(!isAlive) {
			yield return new WaitForSeconds (7.0f);
			for (int i = 1; i <= 10; i++) {
				SetGhostColor (BodyColor, EyesColor, TransparentAlpha);
				yield return new WaitForSeconds (0.2f);
				SetGhostColor (BodyColor, EyesColor, NotSoTransparentAlpha);
				yield return new WaitForSeconds (0.2f);
			}
			SetGhostColor (BodyColor, EyesColor, SolidAlpha);
			transform.gameObject.tag = "Ghost";
			justAwake = true;
			isAlive = true;
			isEatable = false;
		}

	}

	public IEnumerator EatableRoutine()
	{
		while(isAlive && isEatable) {
			yield return new WaitForSeconds (7.0f);
			for (int i = 1; i <= 10; i++) {
				SetGhostColor (BodyColorEatable, EyesColorEatable, SolidAlpha);
				yield return new WaitForSeconds (0.2f);
				SetGhostColor (BodyColor, EyesColor, SolidAlpha);
				yield return new WaitForSeconds (0.2f);
			}
			isAlive = true;
			isEatable = false;
			transform.gameObject.tag = "Ghost";
		}
	}

	void EatenProperties() {
		isAlive = false;
		isEatable = false;
		SetGhostColor (BodyColorEatable, EyesColorEatable, TransparentAlpha);
	}

}
