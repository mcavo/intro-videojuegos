using UnityEngine;
using System.Collections;

public class SpinBehaviour : MonoBehaviour {

	public float Speed = 30f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * Speed);
	}
}
