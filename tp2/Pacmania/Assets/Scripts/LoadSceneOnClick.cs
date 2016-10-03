using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}
