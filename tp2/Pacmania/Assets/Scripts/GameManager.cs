using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.
	public GhostController[] ghosts;
	public GameObject CherryImage;
	public GameObject LiveImage;

	[HideInInspector] public int score;
	[HideInInspector] public int[,] board;
	[HideInInspector] public Vector3[,] directions;
	private int lives;
	private int cherries;
	private bool pause;
	public Point Cherry;

	private Text fruitTargetText;
	private Text fruitTargetBorderText;

	private int pointsToSpawnCherry = 1000;

	private Vector3 u = Vector3.zero,
					d = new Vector3(0,180,0),
					r = new Vector3(0,90,0),
					l = new Vector3(0,270,0),
					n = Vector3.zero;


	void Awake() {
		//Check if instance already exists
		if (instance == null)
		{
			//if not, set instance to this
			instance = this;
		}
		//If instance already exists and it's not this:
		else if (instance != this)
		{
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);	
		}
		// We need the scores at the next scene, so we store them here.
		DontDestroyOnLoad(gameObject);

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	// Use this for initialization
	void Start () {
		score = 0;
		lives = 3;
		cherries = 0;
		InitializeGhosts ();
		fruitTargetText = GameObject.Find("FruitTarget").GetComponent<Text>();
		fruitTargetBorderText = GameObject.Find("FruitTargetBorder").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.P)) {
			pause = !pause;
		}

		if (pause) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		if (score >= pointsToSpawnCherry) {
			if (GameObject.Find ("Cherry") == null) {
				Cherry.gameObject.SetActive (true);
				StartCoroutine (ShowFruitTargetText ());
			}
			pointsToSpawnCherry *= 2;
		}
	}

	public IEnumerator ShowFruitTargetText() {
		Color ftc = fruitTargetText.color;
		ftc.a = 0.6f;
		fruitTargetText.color = ftc;
		ftc = fruitTargetBorderText.color;
		ftc.a = 0.6f;
		fruitTargetBorderText.color = ftc;
		yield return new WaitForSeconds (1.0f);
		ftc = fruitTargetText.color;
		ftc.a = 0.0f;
		fruitTargetText.color = ftc;
		ftc = fruitTargetBorderText.color;
		ftc.a = 0.0f;
		fruitTargetBorderText.color = ftc;
	}

	public void InitializeGhosts()
	{
		Transform ghostsContainer = GameObject.Find ("Ghosts").GetComponent<Transform> ();
		for (int i = 0; i < ghosts.Length; i++) {
//			yield return new WaitForSeconds (1.0f);
			Instantiate (ghosts [i], ghostsContainer);
		}

	}

	void InitGame() {
		// TODO : Load raycasting
		// TODO : Desbloquear el loop y hacerlo 

		pause = false;

		board = new int[22, 19]
			{ {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
			, {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1}
			, {1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,1}
			, {1,1,0,1,0,1,0,2,2,2,2,2,0,1,0,1,0,1,1}
			, {1,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,0,2,2,2,2,2,0,1,0,1,1,0,1}
			, {1,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,1}
			, {1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1}
			, {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1}
			, {1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1}
			, {1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1}
			, {1,0,1,1,1,1,1,1,0,1,0,1,1,1,1,1,1,0,1}
			, {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
			, {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

		directions = new Vector3[22, 19] 
			{ {n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n}
			, {n,r,r,r,d,l,l,r,d,n,d,l,r,r,d,l,l,l,n}
			, {n,d,n,n,d,n,n,n,d,n,d,n,n,n,d,n,n,d,n}
			, {n,d,n,n,d,n,n,n,d,n,d,n,n,n,d,n,n,d,n}
			, {n,r,r,r,d,r,d,l,l,l,r,r,d,l,d,l,l,l,n}
			, {n,d,n,n,d,n,d,n,n,n,n,n,d,n,d,n,n,d,n}
			, {n,r,r,r,d,n,r,r,d,n,d,l,l,n,d,l,l,l,n}
			, {n,d,n,n,d,n,n,n,d,n,d,n,n,n,d,n,n,d,n}
			, {n,r,d,n,d,n,r,r,r,d,l,l,l,n,d,n,d,l,n}
			, {n,n,d,n,d,n,u,r,r,u,l,l,u,n,d,n,d,n,n}
			, {n,r,r,r,r,r,u,r,r,u,l,l,u,l,l,l,l,l,n}
			, {n,u,n,n,u,n,u,r,r,u,l,l,u,n,u,n,n,u,n}
			, {n,u,d,n,u,n,u,l,l,r,r,r,u,n,u,n,d,u,n}
			, {n,n,d,n,u,n,u,n,n,n,n,n,u,n,u,n,d,n,n}
			, {n,r,r,r,r,r,u,l,l,n,r,r,u,l,u,l,l,l,n}
			, {n,u,n,n,u,n,n,n,u,n,u,n,n,n,u,n,n,u,n}
			, {n,u,l,n,u,r,r,r,u,r,u,l,l,l,u,n,r,u,n}
			, {n,n,u,n,u,n,u,n,n,n,n,n,u,n,u,n,u,n,n}
			, {n,r,r,r,u,n,u,l,l,n,r,r,u,n,u,l,l,l,n}
			, {n,u,n,n,n,n,n,n,u,n,u,n,n,n,n,n,n,u,n}
			, {n,u,r,r,r,r,r,r,u,r,u,l,l,l,l,l,l,u,n}
			, {n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n}};

	}

	public void RemoveLive() {
		if (lives > 0) {
			lives--;
			GameObject.Find ("Lives").GetComponentsInChildren<SpriteRenderer> ()[lives].gameObject.SetActive (false);
		}
	}

	public void AddLive() {
		GameObject LivesGO = GameObject.Find ("Lives");
		SpriteRenderer[] LivesSR = LivesGO.GetComponentsInChildren<SpriteRenderer> ();
		if (LivesSR.Length >= lives + 1) {
			LivesSR [lives].gameObject.SetActive (true);
		} else {
			GameObject liveInstance = Instantiate (CherryImage, LivesGO.GetComponent<Transform> ()) as GameObject;
			liveInstance.transform.localPosition = new Vector3 (0, 0, 0);
			liveInstance.transform.localScale = new Vector3 (15, 15, 15);
		}
		lives++;
	}

	public void AddCherry() {
		Transform CherryT = GameObject.Find ("Cherries").GetComponent<Transform>();
		GameObject cherryInstance = Instantiate (CherryImage, CherryT) as GameObject;
		cherryInstance.transform.localPosition = new Vector3 (cherries*50,0,0);
		cherryInstance.transform.localRotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
		cherryInstance.transform.localScale = new Vector3 (18, 18, 18);
		cherries++;
	}
}
