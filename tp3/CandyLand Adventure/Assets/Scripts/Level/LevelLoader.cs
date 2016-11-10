using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public GameObject gameManager;			//GameManager prefab to instantiate.
	public GameObject soundManager;			//SoundManager prefab to instantiate.

	public GameObject Pause;

	void Awake ()
	{
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager.instance == null) {
			//Instantiate gameManager prefab
			Instantiate (gameManager);
		}
		//Check if a SoundManager has already been assigned to static variable SoundManager.instance or if it's still null
		if (SoundManager.instance == null) {
		//Instantiate SoundManager prefab
			Instantiate(soundManager);
		}
	}

	void Start ()
	{
		GameManager.instance.InitGame ();
		StartCoroutine (PauseRoutine ());
		SoundManager.instance.playBasicMusic ();
	}

	private IEnumerator PauseRoutine() {
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				Time.timeScale = 0;
				Pause.SetActive (true);
			}    
			if (Input.GetKeyDown(KeyCode.R))
			{
				Time.timeScale = 1;
				Pause.SetActive (false);
			}  
			yield return null;    
		}
	}
}
