using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingText : MonoBehaviour {

	Text Blinking;

	private string BlinkText;
	private string BlankText;

	// Use this for initialization
	void Start () {
		Blinking = GetComponent<Text>();
		BlinkText = Blinking.text;
		BlankText = "";

		StartCoroutine(TextoParpadeo());
	}

	public IEnumerator TextoParpadeo()
	{
		//el parpadeo sera infinito hasta que termine la condicion dependiendo tu eleccion
		while(true)
		{
			//Establecemos nuestro texto en blanco
			Blinking.text = BlankText;

			//mostramos el texto en blanco por 0.3 segundos
			yield return new WaitForSeconds(.3f);

			//mostramos nuestro texto en mi caso Depredador1220 por otros 0.5 segundos
			Blinking.text = BlinkText;
			yield return new WaitForSeconds(.5f);
		}
	}

}
