using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	public GameObject gameManager;			//GameManager prefab to instantiate.
	//public GameObject soundManager;			//SoundManager prefab to instantiate.

	public Text FeedBack;

	public GameObject WinLessThanHard;
	public GameObject LooseGreatThanEasy;
	public GameObject WinHardOrLooseEasy;

	public Button[] MenuButtons;
	public Button[] PlayEasier;
	public Button[] PlayHarder;
	public Button[] PlayAgain;

	void Awake() {
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager.instance == null) {
			//Instantiate gameManager prefab
			Instantiate (gameManager);
		}
		//Check if a SoundManager has already been assigned to static variable SoundManager.instance or if it's still null
		//if (SoundManager.instance == null) {
		//Instantiate SoundManager prefab
		//Instantiate(soundManager);
		//}
		Fill();
	}
		
	// Use this for initialization
	void Start () {
		foreach (var b in MenuButtons) {
			b.onClick.AddListener (MenuButtonOnClick);
		}
		foreach (var b in PlayEasier) {
			b.onClick.AddListener (PlayEasierButtonOnClick);
		}
		foreach (var b in PlayHarder) {
			b.onClick.AddListener (PlayHarderButtonOnClick);
		}
		foreach (var b in PlayAgain) {
			b.onClick.AddListener (PlayAgainButtonOnClick);
		}
		// PlayButton.onClick.AddListener (PlayButtonOnClick);
	}

	void Fill() {
		if (GameManager.instance.Win) {
			FeedBack.text = "Delicious!!";
			if (GameManager.instance.Difficulty < 3) {
				WinLessThanHard.SetActive (true);
			} else {
				WinHardOrLooseEasy.SetActive (true);
			}
		} else {
			FeedBack.text = "So sad!!";
			if (GameManager.instance.Difficulty > 1) {
				LooseGreatThanEasy.SetActive (true);
			} else {
				WinHardOrLooseEasy.SetActive (true);
			}
		}
	}

	void PlayAgainButtonOnClick() {
		SceneManager.LoadScene ("Level");
	}

	void PlayEasierButtonOnClick() {
		GameManager.instance.Difficulty -= 1;
		SceneManager.LoadScene ("Level");
	}

	void PlayHarderButtonOnClick() {
		GameManager.instance.Difficulty += 1;
		SceneManager.LoadScene ("Level");
	}

	void MenuButtonOnClick() {
		SceneManager.LoadScene ("Menu");
	}
}
