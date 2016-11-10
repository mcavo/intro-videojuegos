using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	Image TimeBar;

	Dungeon dungeon;

	private Color color;

	// Use this for initialization
	void Start () {
		TimeBar = gameObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {

		color = TimeBar.color;

		GameManager.instance.TimeLeft += Time.deltaTime;

		color.r = GameManager.instance.TimeLeft / GameManager.instance.Time;
		TimeBar.fillAmount = 1f - GameManager.instance.TimeLeft / GameManager.instance.Time;
		TimeBar.color = color;

		if (TimeBar.fillAmount <= 0f) {
			GameManager.instance.GameOver ();
		}
	}
}
