using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverLoader : MonoBehaviour {

	//WinnerText

	public GameObject soundManager;			//SoundManager prefab to instantiate.
	public GameObject[] chickenAnimations;
	[HideInInspector] public int[] scores;


	void Awake ()
	{
		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
		if (SoundManager.instance == null)
			//Instantiate SoundManager prefab
			Instantiate(soundManager);

		scores = GameManager.instance.scores;

		//Destroy Game Manager instance
		GameManager.instance.destroyOnLevel = true;

		Text winnerText = GameObject.Find ("WinnerText").GetComponent<Text>();

		if (scores [0] == scores [1]) {
			winnerText.text = "It's a tie!";
		} else if (scores [0] > scores [1]) {
			winnerText.text = "Player1 wins!";
			Instantiate (chickenAnimations [0]);
		} else {
			winnerText.text = "Player2 wins!";
			Instantiate (chickenAnimations [1]);
		}

	}

	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	private void checkInput() {
		if (Input.GetKey(KeyCode.Space)) {
			SceneManager.LoadScene ("Main");	
		} 
	}



}
