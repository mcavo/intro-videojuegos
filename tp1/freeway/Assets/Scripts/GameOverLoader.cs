using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverLoader : MonoBehaviour
{
	public GameObject soundManager;				// SoundManager prefab to instantiate.
	public GameObject[] chickenAnimations;		// Chicken animations
	[HideInInspector] public int[] scores;		// Scores -> we get them from the GameManager

	void Awake ()
	{
		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
		if (SoundManager.instance == null)
		{
			//Instantiate SoundManager prefab
			Instantiate (soundManager);
		}
		if (GameManager.instance == null)
		{
			// If the GameManager is not assigned we cant get the scores and decide who wins -> so we take them to the menu
			SceneManager.LoadScene ("Main");
		}

		scores = GameManager.instance.scores;

		//Destroy GameManager instance
		GameManager.instance.destroyOnLevel = true;

		Text winnerText = GameObject.Find ("WinnerText").GetComponent<Text>();

		if (scores [0] == scores [1])
		{
			winnerText.text = "It's a tie!";
			Instantiate (chickenAnimations [0]);
		}
		else if (scores [0] > scores [1])
		{
			winnerText.text = "Player1 wins!";
			Instantiate (chickenAnimations [1]);
		} 
		else
		{
			winnerText.text = "Player2 wins!";
			Instantiate (chickenAnimations [2]);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		checkInput ();
	}

	private void checkInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			SceneManager.LoadScene ("Main");
		} 
	}

}
