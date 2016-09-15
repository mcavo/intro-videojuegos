using UnityEngine;
using System.Collections;

public class PointScript : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals ("Pacman"))
		{
			Destroy(gameObject); 
		}
	}
}
