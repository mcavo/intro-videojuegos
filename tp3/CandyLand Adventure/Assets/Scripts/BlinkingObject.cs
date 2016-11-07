using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingObject : MonoBehaviour
{
	private bool shouldBlink;
	private int level = 1; //Only the default value. It should be changeq

	void Start ()
	{
		shouldBlink = true;
		//StartCoroutine(MakeItBlink());
	}

	public void StopBlinking() 
	{
		shouldBlink = false;
	}

	public void SetLevel(int level)
	{
		this.level = level;
	}

	public IEnumerator MakeItBlink()
	{
		while(shouldBlink)
		{
			Debug.Log ("BlinkingObject corrutine");
			// Shows the blank text
			gameObject.SetActive(true);

			// Waits for 0.3 seconds
			yield return new WaitForSeconds(.3f - .001f*level);

			// Shows the original text
			gameObject.SetActive(false);
				
			// Waits for 0.5 seconds
			yield return new WaitForSeconds(.1f + 0.01f*level);
		}
	}

}