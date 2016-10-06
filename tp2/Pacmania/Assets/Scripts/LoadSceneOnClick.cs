using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int scene)
	{
		SceneManager.LoadScene (scene);
	}
}
