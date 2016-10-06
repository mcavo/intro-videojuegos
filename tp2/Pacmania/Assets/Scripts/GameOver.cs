using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	[HideInInspector] public int score;				// Score -> we get them from the GameManager
	[HideInInspector] public int highScore;			// HighScore -> we get them from the GameManager

	private Text scoreText;
	private Text highScoreText;
	private GameObject recordMessageText;

	void Awake ()
	{
		if (GameManager.instance == null)
		{
			// If the GameManager is not assigned we cant get the scores and decide who wins -> so we take them to the menu
			SceneManager.LoadScene ("MainMenu");
		}
			
		getScoresFromManager ();
		initializeComponents ();
	}
		
	private void initializeComponents() 
	{
		scoreText = GameObject.Find ("Score").GetComponent<Text>();
		highScoreText = GameObject.Find ("HighScore").GetComponent<Text>();
		recordMessageText = GameObject.Find ("NewHighScore");
		scoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();

		recordMessageText.SetActive (score > highScore);
	}

	private void getScoresFromManager() 
	{
		score = GameManager.instance.score;
		highScore = GameManager.instance.getHighScore();

		//Destroy GameManager instance
		GameManager.instance.destroyOnLevel = true;
	}
}
