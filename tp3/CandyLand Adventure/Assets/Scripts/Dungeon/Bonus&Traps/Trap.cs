using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	
	public float reduceSpeed = 5f;
	public float reduceTime = 1f;
	public float duration = 1f;
	public AudioClip audio;

	void OnTriggerEnter(Collider col)
	{
		GameManager.instance.TimeLeft += reduceTime;
		if (audio != null) 
		{
			SoundManager.instance.PlaySingle (audio);
		}
		gameObject.SetActive (false);
	}
}
