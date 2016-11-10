using UnityEngine;
using System.Collections;

public class SpinBehaviour : MonoBehaviour
{
	public float Speed = 30f;

	void Update ()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * Speed);
	}
}
