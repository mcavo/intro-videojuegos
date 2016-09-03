using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkingColorText : MonoBehaviour {

	Text Blinking;

	// Use this for initialization
	void Start () {
		Blinking = GetComponent<Text>();

		StartCoroutine(TextoParpadeo());
	}

	public IEnumerator TextoParpadeo()
	{
		//el parpadeo sera infinito hasta que termine la condicion dependiendo tu eleccion
		while(true)
		{
			//Establecemos nuestro texto en blanco
			Blinking.color = new Color(0.91373f, 0.49020f, 0.67451f, 1f);

			//mostramos el texto en blanco por 0.3 segundos
			yield return new WaitForSeconds(.6f);

			//mostramos nuestro texto en mi caso Depredador1220 por otros 0.5 segundos
			Blinking.color = new Color(0.91373f, 0.078431f, 0.44706f, 1f);
			yield return new WaitForSeconds(.6f);
		}
	}

}
