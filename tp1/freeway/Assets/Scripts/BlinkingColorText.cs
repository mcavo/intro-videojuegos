using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingColorText : MonoBehaviour
{
	public bool isBlinking;

	private Text Blinking;
	private Color lightPink;
	private Color darkPink;
	private Color originalColor;

	// Use this for initialization
	void Start ()
	{
		Blinking = GetComponent<Text>();
		originalColor = Blinking.color;
		isBlinking = true;
		lightPink = new Color(0.91373f, 0.49020f, 0.67451f, 1f);
		darkPink = new Color(0.91373f, 0.078431f, 0.44706f, 1f);
		StartCoroutine(MakeItBlink());
		Blinking.color = originalColor;
	}

	public IEnumerator MakeItBlink()
	{
		while(isBlinking)
		{
			// Changes the color to light pink
			Blinking.color = lightPink;

			// Waits for 0.6 seconds
			yield return new WaitForSeconds(.6f);

			// Changes the color to dark pink
			Blinking.color = darkPink;

			// Waits for 0.6 seconds
			yield return new WaitForSeconds(.6f);
		}
	}

}
