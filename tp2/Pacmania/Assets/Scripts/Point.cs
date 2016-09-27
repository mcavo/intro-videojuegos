using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals ("Pacman"))
		{
			transform.gameObject.SetActive (false);
		}
	}
}
