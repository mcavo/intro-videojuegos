using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.
	[HideInInspector] public int score;
	[HideInInspector] public int[,] board;
	private int lives;


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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitGame() {
		score = 0;
		lives = 3;
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
	}
}
