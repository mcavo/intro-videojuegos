using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class chicken : MonoBehaviour {
	public float speed = 0.05f;
	public static float assetsSize = 0.36f;

	private RectTransform rectTransform;
	private int score;
	public Text scoreText;
	public Text scoreText2;
	//TODO: check if it should not be public. 
	//TODO: same problem with dimensions of screen
	private Vector3 initialPosition = new Vector3(4f*assetsSize, 1f*assetsSize, 0);
	private Vector3 initialPosition2 = new Vector3(13f*assetsSize, 1f*assetsSize, 0);

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText = GameObject.Find ("Score").GetComponent<Text>();
		scoreText2 = GameObject.Find ("Score2").GetComponent<Text>(); 
		SetScoreText();
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	private void checkInput() {
		Vector3 currentPosition = transform.position;
		if(this.tag.Equals ("Chicken")) {
			if (Input.GetKey(KeyCode.W)) {
				transform.position = new Vector3 (currentPosition.x, currentPosition.y + speed, currentPosition.z);			
			} else if (Input.GetKey(KeyCode.S)) {
				if (currentPosition.y - speed < 1f * assetsSize) {
					transform.position = initialPosition;
				} else {
					transform.position = new Vector3 (currentPosition.x, currentPosition.y - speed, currentPosition.z);
				}
			}
		} else {
			if (Input.GetKey(KeyCode.UpArrow)) {
				transform.position = new Vector3 (currentPosition.x, currentPosition.y + speed, currentPosition.z);			
			} else if (Input.GetKey(KeyCode.DownArrow)) {
				if (currentPosition.y - speed < 1f * assetsSize) {
					transform.position = initialPosition2;
				} else {
					transform.position = new Vector3 (currentPosition.x, currentPosition.y - speed, currentPosition.z);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col2d) {
		Debug.Log (col2d.tag);
		if (col2d.tag.Equals ("LittlePinkCar")) {
			Debug.Log ("perdí :(");
			if (this.tag.Equals ("Chicken")) {
				transform.position = initialPosition;
			} else {
				transform.position = initialPosition2;
			}
		}

		if (col2d.tag.Equals("EndRoad")) {
			Debug.Log ("Pasé uno");
			if (this.tag.Equals ("Chicken")) {
				transform.position = initialPosition;
			} else {
				transform.position = initialPosition2;
			}
			score++;
			SetScoreText ();
		}
	}

	void SetScoreText() {
		if (this.tag.Equals ("Chicken")) {
			scoreText.text = score.ToString ();
		} else {
			scoreText2.text = score.ToString ();
		}
	}
}
