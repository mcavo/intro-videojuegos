using UnityEngine;
using System.Collections;
using System.Timers;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
	private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
	public float timeLeft = 25.0F;

	// Use this for initialization
	void Awake () {
		
		//Check if instance already exists
		if (instance == null) {
			//if not, set instance to this
			instance = this;
		}
		//If instance already exists and it's not this:
		else if (instance != this) {

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);	
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	void Update() {
		double oldTime = System.Math.Round(timeLeft,2);
		timeLeft -= Time.deltaTime / 4;
		double newTime = System.Math.Round(timeLeft,2);
		if (oldTime - newTime != 0) {
			boardScript.timeText.text = string.Format ("{0}", newTime.ToString("F2"));

		}	
		validateGameOver ();
	}

	//Initializes the game for each level.
	void InitGame() {
		
		//Call the SetupScene function of the BoardManager script, pass it current difficulty number.
		boardScript.SetupScene(0); //TODO: change difficulty from menu.
	}

	private void validateGameOver() {
		if (timeLeft < 0) {
			Debug.Log ("Game Over");
			// Here we should show the initial menu
		}
	}
}
