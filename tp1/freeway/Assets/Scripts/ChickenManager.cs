using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChickenManager : MonoBehaviour {
	public float speed = 0.05f;
	public static float assetsSize = 0.36f;
	public Vector3 initialPosition;
	public KeyCode upKey;
	public KeyCode downKey;
	public AudioClip crushSound;
	public AudioClip arriveSound;

	private Text scoreText;
	private RectTransform rectTransform;
	private int score;
	private Canvas canvas;
	private float correctionFactor = 0.10f;

	// Use this for initialization
	void Start () {
		initialPosition = assetsSize*initialPosition;
		score = 0;
		initializeText();
		SetScoreText();
		transform.position = initialPosition;
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
			SoundManager.instance.PlaySingle (crushSound);
		}

		if (col2d.tag.Equals("EndRoad")) {
			transform.position = initialPosition;
			score++;
			SoundManager.instance.PlaySingle (arriveSound);
			SetScoreText ();
		}
	}

	private void SetScoreText() {
		scoreText.text = score.ToString ();
	}

	private void initializeText() {
		GameObject scoreT = new GameObject("TextScore");
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

		scoreT.transform.SetParent(canvas.transform, false);

		RectTransform trans = scoreT.AddComponent<RectTransform>();

		Text text = scoreT.AddComponent<Text>();
		scoreText = text;

		settingAnchor (trans);
		addingTextStyle();
		updateTextDimensionAndPosition ();
	}

	private void updateTextDimensionAndPosition () {
		Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		int newSize = Screen.height / 10;
		scoreText.fontSize = newSize;
		Vector3 cameraPosition = camera.WorldToScreenPoint (new Vector3(initialPosition.x, 13f*assetsSize + correctionFactor, 0));
		scoreText.rectTransform.anchoredPosition = new Vector2(cameraPosition.x, cameraPosition.y);
	}

	private void addingTextStyle() {
		//Adding fotmat style to text
		Font BitMapFont = Resources.Load("Fonts/Masaaki-Regular", typeof(Font)) as Font;  
		scoreText.alignment = TextAnchor.MiddleCenter;
		scoreText.color = Color.white;
		scoreText.font = BitMapFont;
		scoreText.horizontalOverflow = HorizontalWrapMode.Overflow;
		scoreText.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void settingAnchor(RectTransform trans) {
		//Set anchor in the bottom left corner
		trans.anchorMax = new Vector2 (0f, 0f);
		trans.anchorMin = new Vector2 (0f, 0f);
		trans.pivot = new Vector2 (0.5f, 0.5f);
	}
}
