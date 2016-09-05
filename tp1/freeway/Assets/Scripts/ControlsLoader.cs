using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class ControlsLoader : MonoBehaviour {

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
