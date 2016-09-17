using UnityEngine;
using System.Collections;

public class ModifyScale : MonoBehaviour {

	Vector3 oldScale;
	public Vector3 newScale = new Vector3 (1.5f, 1.5f, 1.5f);

	// Use this for initialization
	void Start () {
		oldScale = transform.localScale;
		StartCoroutine(ModifyScaleRoutine());
	}

	public IEnumerator ModifyScaleRoutine()
	{
		while(true)
		{
			transform.localScale = newScale;

			// Waits for 0.3 seconds
			yield return new WaitForSeconds(.2f);

			transform.localScale = oldScale;

			// Waits for 0.5 seconds
			yield return new WaitForSeconds(.4f);
		}
	}
}
