using UnityEngine;
using System.Collections;
using System.Timers;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.
	public AudioClip TickTockSound;							// Annoying tick tock audio clip.  
	public float timeLeft = 25.0F;
	[HideInInspector] public bool destroyOnLevel;			// To destroy the instance on update
	[HideInInspector] public int[] scores;

	private BoardManager boardScript;						// Store a reference to our BoardManager.
	private BlinkingText blinkScript;						// Store a reference to our BlinkingText script;
	private GameObject timeText;							// Store a reference to the text that shows the remaining time.
	private bool isBlinking;
	private bool endgame;

	// Use this for initialization
	void Awake ()
	{
		isBlinking = false;
		endgame = false;
		destroyOnLevel = false;

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

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	void Update()
	{
		if (destroyOnLevel)
		{
			Destroy (gameObject);
		}
		// So it doesnt crush when is 
		if (!endgame)
		{
			double oldTime = System.Math.Round (timeLeft, 2);
			timeLeft -= Time.deltaTime / 4;
			double newTime = System.Math.Round (timeLeft, 2);

			if (!isBlinking && newTime < 5)
			{
				// Set a flag so it doesnt add the same script to the time text several times
				isBlinking = true;
				// Changes the time text color, so the users advice the game is about to end
				timeText = GameObject.Find ("TimeText");
				timeText.AddComponent <BlinkingColorText> ();
				// Plays an annoying Tick Tock sound
				SoundManager.instance.PlayLoop (TickTockSound);
			}

			if (oldTime - newTime != 0)
			{
				// Updates the time text
				boardScript.timeText.text = string.Format ("{0}", newTime.ToString ("F2"));
			}

			validateGameOver ();
		}
	}

	//Initializes the game.
	void InitGame()
	{
		//Call the SetupScene function of the BoardManager script, pass it current difficulty number.
		//TODO: Implement differents difficultys.
		boardScript.SetupScene(0);
	}

	// Checks if the time is over, and loads the game over scene.
	private void validateGameOver()
	{
		if (timeLeft < 0)
		{
			endgame = true;
			SceneManager.LoadScene ("GameOver");
		}
	}
}
