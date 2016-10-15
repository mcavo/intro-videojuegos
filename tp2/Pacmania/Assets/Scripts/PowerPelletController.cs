using UnityEngine;
using System.Collections;

public class PowerPelletController : MonoBehaviour {

	Vector3 oldScale;
	public Vector3 newScale = new Vector3 (1.5f, 1.5f, 1.5f);
	private Vector3 currentScale;

	// Use this for initialization
	void Start () {
		oldScale = transform.localScale;
		StartCoroutine(ModifyScaleRoutine());
	}

	public IEnumerator ModifyScaleRoutine()
	{
		while(true)
		{
			while (transform.localScale != newScale) {
				// Waits for 0.2 seconds
				yield return new WaitForSeconds (.05f);
				transform.localScale = transform.localScale - new Vector3 (0.1f, 0.1f, 0.1f);
			}
			yield return new WaitForSeconds (.025f);
			while (transform.localScale != oldScale) {
				// Waits for 0.2 seconds
				yield return new WaitForSeconds (.05f);
				transform.localScale = transform.localScale + new Vector3 (0.1f, 0.1f, 0.1f);
			}
			yield return new WaitForSeconds (.025f);
		}
	}
	void OnTriggerEnter(Collider col) {
		Debug.Log ("Hola1");
		if (col.CompareTag("Pacman")) {
			Debug.Log ("Hola2");
			ObserverPattern.Subject.getInstance().Notify (); //Notify ghosts when pacman eat it
		}
	}
}
