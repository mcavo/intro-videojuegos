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

	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-37,41};

	private BoxCollider boxCollider;      //The BoxCollider2D component attached to this object.
	private Rigidbody rb;               //The Rigidbody2D component attached to this object.

	private bool isAlive = true;
	private bool isEatable = false;

	private Coroutine co = null;

	private Vector3 up = Vector3.zero,
		down = new Vector3(0,180,0),
		right = new Vector3(0,90,0),
		left = new Vector3(0,270,0),
		currentDirection = Vector3.zero;

	private Vector3 initialPosition = new Vector3(1,-6.2f,9);

	private System.Random rnd = new System.Random(new System.DateTime().Millisecond);

	void Start() {
		QualitySettings.vSyncCount = 0;
		transform.position = initialPosition;
		Reset();
		ObserverPattern.Subject.getInstance ().AddObserver (this); //Subscribe to notification
		//Get a component reference to this object's BoxCollider2D
		boxCollider = GetComponent <BoxCollider> ();
		//Get a component reference to this object's Rigidbody2D
		rb = GetComponent <Rigidbody> ();
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

		if (AtCheckPoint ()) {
			if (isAlive) {
				MovementSpeed = 10f;
				if (isEatable) {
					MovementSpeed = 16f;
				}
				UpdateDirection ();
			} else {
				MovementSpeed = 16f;
				isMoving = UpdateEatenDirection ();
			}
			transform.localEulerAngles = currentDirection;
		}

		if (isMoving) {
			transform.Translate (RoundVector3 (Vector3.forward * (movementOffset / MovementSpeed), 2));
		}

	}

	private bool AtCheckPoint() {
		Vector3 pos = transform.position;
		return Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.x / 4f, 2) - Mathf.Floor (pos.x / 4f))) &&
			Mathf.Approximately (0.25f, (float)(System.Math.Round (pos.z / 4f, 2) - Mathf.Floor (pos.z / 4f)));
	}

	private void UpdateDirection() {

		RaycastHit hit;
		ArrayList posibleDirections = new ArrayList();

		currentDirection = RoundVector3 (currentDirection, 0);

		ifCanMoveAndNotOppositeAddToArrayList (up, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (down, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (left, posibleDirections);
		ifCanMoveAndNotOppositeAddToArrayList (right, posibleDirections);

		Vector3 curPosition = RoundVector3 (transform.position, 2);
		Vector3 nextPosition;

		transform.localEulerAngles = up;

		foreach (var angle in posibleDirections) {
			transform.localEulerAngles = (Vector3) angle;
			//TODO: WTF. Check this numbers.
			nextPosition = RoundVector3 (220 * 3 * movementOffset * transform.forward + transform.position, 2);
			if (Physics.Raycast (curPosition, nextPosition, out hit)) {
				if (hit.collider.tag == "Pacman") {
					if (isEatable) {
						ifCanMoveAddToArrayList (BackDirection(currentDirection), posibleDirections);
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

	private bool UpdateEatenDirection() {
		currentDirection = GetNewDirection (RoundVector3 (transform.position, 2));
		transform.localEulerAngles = currentDirection;
		return CanMove (GetBoardPosition (RoundVector3 (transform.position + transform.forward * movementOffset, 2)));
	}

	private Vector3 BackDirection( Vector3 angle) {
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
		if (currentDirection != BackDirection (angle)) {
			ifCanMoveAddToArrayList (angle, posibleDirections);
		}
	}

	private void ifCanMoveAddToArrayList( Vector3 angle, ArrayList posibleDirections) {
		transform.localEulerAngles = angle;
		if (CanMove (GetBoardPosition (RoundVector3 (transform.position + transform.forward * movementOffset, 2)))) {
			posibleDirections.Add (angle);
		}
		transform.localEulerAngles = currentDirection;
	}

	private bool CanMove( int[] toPosition ) {
		return GameManager.instance.board [toPosition[1], toPosition[0]] == 0;
	}

	private Vector3 GetBoardDirection( int[] toPosition ) {
		return GameManager.instance.directions [toPosition[1], toPosition[0]];
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
		SetGhostColor (BodyColorEatable, EyesColorEatable, SolidAlpha);
		transform.gameObject.tag = "EatableGhost";
		isEatable = true;
		if (co != null) {
			StopCoroutine (co);
		}
		co = StartCoroutine(EatableRoutine());
	}

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("Pacman") && isEatable) {
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
			for (int i = 1; i <= 15; i++) {
				SetGhostColor (BodyColor, EyesColor, TransparentAlpha);
				yield return new WaitForSeconds (0.2f);
				SetGhostColor (BodyColor, EyesColor, NotSoTransparentAlpha);
				yield return new WaitForSeconds (0.2f);
			}
			SetGhostColor (BodyColor, EyesColor, SolidAlpha);
			transform.gameObject.tag = "Ghost";
			isAlive = true;
			isEatable = false;
		}

	}

	public IEnumerator EatableRoutine()
	{
		while(isAlive && isEatable) {
			yield return new WaitForSeconds (7.0f);
			for (int i = 1; i <= 15; i++) {
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
