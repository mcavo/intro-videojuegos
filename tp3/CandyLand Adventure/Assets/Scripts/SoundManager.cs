using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource[] efxSources;
	public AudioSource musicSource;                 // Drag a reference to the audio source which will play the music.
	public AudioClip musicClip;
	public static SoundManager instance = null;     // Allows other scripts to call functions from SoundManager.             
	public float lowPitchRange = .95f;              // The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            // The highest a sound effect will be randomly pitched.
	private bool silence = false;


	void Awake ()
	{
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
	}

	public void PauseMusic()
	{
		silence = !silence;
		if (silence) {
			foreach(AudioSource source in efxSources)
			{
				source.Pause ();
			}
			musicSource.Pause ();
		} else {
			foreach(AudioSource source in efxSources)
			{
				source.Play ();
			}
			musicSource.Play ();
		}	
 	}

	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		foreach (AudioSource efxSource in efxSources) 
		{
			if (!efxSource.isPlaying) {
				efxSource.clip = clip;

				//Play the clip.
				efxSource.Play ();
			}
		}

	}

	public void playBasicMusic()
	{
		musicSource.clip = musicClip;
		musicSource.Play ();
	}
}