using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
	public float incrementSpeed = 5f;
	public float incrementTime = 1f;
	public float duration = 1f;
	public AudioClip audio;

	void OnTriggerEnter(Collider col)
	{
		GameManager.instance.TimeLeft -= incrementTime;
		if (audio != null) 
		{
			SoundManager.instance.PlaySingle (audio);
		}
		gameObject.SetActive (false);
	}
}
