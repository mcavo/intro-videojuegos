using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MainLoader : MonoBehaviour
{
	public GameObject soundManager;			//SoundManager prefab to instantiate.

	void Awake ()
	{
		//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
		if (SoundManager.instance == null)
		{
			//Instantiate SoundManager prefab
			Instantiate (soundManager);
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
			SceneManager.LoadScene ("Play");	
		} 
	}
}
