using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;				// Static instance of GameManager which allows it to be accessed by any other script.

	public Dungeon[] Dungeons;
	public int DungeonToPlay;

	public int Difficulty;
	public float Time;
	public float TimeLeft;
	public bool Win;
	public AudioClip endLevel;

	void Awake()
	{
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
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		DungeonToPlay = 0;
	}

	public void InitGame()
	{
		InitDungeon ();
		SetCamera ();
		SetTimer ();
	}

	private void SetCamera()
	{
		var InitDungeon = GameObject.Find ("StartPoint").GetComponent<Transform> ();
		var camera = GameObject.Find ("Main Camera");
		camera.transform.position = InitDungeon.transform.position;
		camera.transform.rotation = InitDungeon.transform.rotation;
	}

	private void InitDungeon()
	{
		DungeonCreator dc = GameObject.Find ("DungeonCreator").GetComponent<DungeonCreator> ();
		Dungeon d = Dungeons [DungeonToPlay];

		dc.Seed = d.Seed;
		dc.Name = d.name;
		dc.Start = d.Start;
		dc.End = d.End;
		dc.MatrixSize = d.Size;
		dc.Difficulty = Difficulty;
		dc.Generate();
		TimeLeft = 0;
		Time = d.TimeNeeded();
	}

	private void SetTimer()
	{
		StartCoroutine (StartRoutine ());
	}

	private IEnumerator StartRoutine()
	{
		yield return new WaitForSeconds (1.0f);
		GameObject.Find("Fill").GetComponent<TimeManager> ().enabled = true;
	}

	private IEnumerator WinRoutine()
	{
		SoundManager.instance.PlaySingle (endLevel);
		GameObject.Find ("Main Camera").GetComponent<Bloom> ().enabled = true;
		Destroy (GameObject.Find("Cake"));
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("GameOver");
	}

	public void WinGame()
	{
		Win = true;
		StartCoroutine (WinRoutine());
	}
		
	public void GameOver()
	{
		Win = false;
		SceneManager.LoadScene ("GameOver");
	}
}
