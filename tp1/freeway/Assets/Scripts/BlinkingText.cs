using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingText : MonoBehaviour
{

	public bool isBlinking;

	private Text Blinking;
	private string BlinkText;
	private string BlankText;

	// Use this for initialization
	void Start ()
	{
		Blinking = GetComponent<Text>();
		BlinkText = Blinking.text;
		BlankText = "";
		isBlinking = true;
		StartCoroutine(MakeItBlink());
	}

	public IEnumerator MakeItBlink()
	{
		while(isBlinking)
		{
			// Shows the blank text
			Blinking.text = BlankText;

			// Waits for 0.3 seconds
			yield return new WaitForSeconds(.3f);

			// Shows the original text
			Blinking.text = BlinkText;

			// Waits for 0.5 seconds
			yield return new WaitForSeconds(.5f);
		}
	}

}
