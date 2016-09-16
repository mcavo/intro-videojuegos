using UnityEngine;
using System.Collections;

public class Pacman : MovingObject {
	private int[] dirCurrent;
	private int[] dirNext;

	private int[] UP = new int[2] {0,-1};
	private int[] DOWN = new int[2] {0,1};
	private int[] RIGHT = new int[2] {1,0};
	private int[] LEFT = new int[2] {-1,0};
	//private int[] NO_DIRECTION = new int[2] {0,0};

	private int movementOffset = 4;
	private int[] matrixOffset = new int[2] {-35,41};

	// Use this for initialization

	void Awake() {
		dirCurrent = RIGHT;
		dirNext = RIGHT;
	}

	void Start () {
		
		//Call the Start function of the MovingObject base class.
		base.Start ();
	}

	// Update is called once per frame
	void Update () {
		CheckInput ();
		UpdateDirection ();
		AttemptMove (movementOffset * dirCurrent [0], 0, movementOffset * dirCurrent [1]);
	}

	void CheckInput() {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			dirNext = UP;
			Debug.Log ("up");
			Debug.Log (CanMove (dirNext));
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			dirNext = DOWN;
			Debug.Log ("down");
			Debug.Log (CanMove (dirNext));
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			dirNext = RIGHT;
			Debug.Log ("right");
			Debug.Log (CanMove (dirNext));
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			dirNext = LEFT;
			Debug.Log ("left");
			Debug.Log (CanMove (dirNext));
		}
	}

	void UpdateDirection() {
		if (dirCurrent != dirNext) {
			if (CanMove (dirNext)) {
				dirCurrent = dirNext;
			}
		}
	}

	int[] CurrentPosition () {
		Vector3 pos = transform.position;
		return new int[2] { (int) ((pos.x - matrixOffset [0]) / movementOffset), (int) ((matrixOffset [1] - pos.z) / 4) };
	}

	bool CanMove (int[] direction) {
		int[] current = CurrentPosition ();
		Debug.Log ("current: " + current [0] + " " + current [1]);
		Debug.Log ("direction: " + direction [0] + " " + direction [1]);
		return (GameManager.instance.board[current[1]+direction[1],current[0]+direction[0]] == 0);
	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove (int xDir, int yDir, int zDir)
	{
		Debug.Log (CanMove(dirCurrent));
		if(CanMove(dirCurrent))
			Debug.Log("From: " + transform.position.x + " " + transform.position.y + " " + transform.position.z + " - To: " + xDir + " " + yDir + " " + zDir);
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		if (CanMove (dirCurrent)) {
			base.AttemptMove (xDir, yDir, zDir);
		}
	}
}
