using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	Image TimeBar;

	Dungeon dungeon;

	// Use this for initialization
	void Start () {
		TimeBar = gameObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		GameManager.instance.TimeLeft += Time.deltaTime;
		TimeBar.fillAmount = GameManager.instance.TimeLeft / GameManager.instance.Time;

		if (TimeBar.fillAmount >= 1f) {
			GameManager.instance.GameOver ();
		}
	}
}
