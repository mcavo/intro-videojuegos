using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.
	[HideInInspector] public int score;
	[HideInInspector] public int[,] board;
	[HideInInspector] public Vector3[,] directions;
	private int lives;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitGame() {
		// TODO : Load raycasting
		// TODO : Desbloquear el loop y hacerlo 
		board = new int[22, 19]
			{ {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
			, {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1}
			, {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
			, {1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1}
			, {1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1}
			, {1,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,1}
			, {1,1,1,1,0,1,0,0,0,0,0,0,0,1,0,1,1,1,1}
			, {1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1}
			, {0,0,0,1,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0}
			, {1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1}
			, {1,1,1,1,0,1,0,0,0,0,0,0,0,1,0,1,1,1,1}
			, {1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,1,1}
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
			, {n,r,r,r,d,n,d,r,r,n,d,l,l,n,d,l,l,l,n}
			, {n,n,n,n,d,n,n,n,d,n,d,n,n,n,d,n,n,n,n}
			, {n,n,n,n,d,n,r,r,r,d,l,l,l,n,d,n,n,n,n}
			, {n,n,n,n,d,n,u,n,n,n,n,n,u,n,d,n,n,n,n}
			, {n,n,n,n,r,r,u,n,n,n,n,n,u,l,l,n,n,n,n}
			, {n,n,n,n,u,n,u,n,n,n,n,n,u,n,u,n,n,n,n}
			, {n,n,n,n,u,n,u,l,l,r,r,r,u,n,u,n,n,n,n}
			, {n,n,n,n,u,n,u,n,n,n,n,n,u,n,u,n,n,n,n}
			, {n,r,r,r,r,r,u,l,l,n,r,r,u,l,l,l,l,l,n}
			, {n,u,n,n,u,n,n,n,u,n,u,n,n,n,u,n,n,u,n}
			, {n,u,l,n,u,r,r,r,u,r,u,l,l,l,u,n,r,u,n}
			, {n,n,u,n,u,n,u,n,n,n,n,n,u,n,u,n,u,n,n}
			, {n,r,r,r,u,n,u,l,l,n,r,r,u,n,u,l,l,l,n}
			, {n,u,n,n,n,n,n,n,u,n,u,n,n,n,n,n,n,u,n}
			, {n,u,r,r,r,r,r,r,u,r,u,l,l,l,l,l,l,u,n}
			, {n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n,n}};

	}

	public void RemoveLive() {
		lives--;
		if (lives >= 0) {
			GameObject.Find ("Lives").GetComponentsInChildren<SpriteRenderer> ()[lives].gameObject.SetActive (false);
		}
	}
}
