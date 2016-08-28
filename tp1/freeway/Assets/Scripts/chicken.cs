using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class chicken : MonoBehaviour {
	public float speed = 0.05f;
	public static float assetsSize = 0.36f;

	private RectTransform rectTransform;
	private int score;
	public Text scoreText;
	public Canvas canvas;
	public Vector3 initialPosition;
	public KeyCode upKey;
	public KeyCode downKey;

	// Use this for initialization
	void Start () {
		score = 0;
		canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();
		scoreText.transform.SetParent (canvas);
		SetScoreText();
	}

	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	private void checkInput() {
		Vector3 currentPosition = transform.position;
		if (Input.GetKey(upKey)) {
			transform.position = new Vector3 (currentPosition.x, currentPosition.y + speed, currentPosition.z);			
		} else if (Input.GetKey(downKey)) {
			if (currentPosition.y - speed < 1f * assetsSize) {
				transform.position = initialPosition;
			} else {
				transform.position = new Vector3 (currentPosition.x, currentPosition.y - speed, currentPosition.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col2d) {
		if (col2d.tag.Equals ("LittlePinkCar")) {
			transform.position = initialPosition;
		}

		if (col2d.tag.Equals("EndRoad")) {
			transform.position = initialPosition;
			score++;
			SetScoreText ();
		}
	}

	void SetScoreText() {
		scoreText.text = score.ToString ();
	}
}
