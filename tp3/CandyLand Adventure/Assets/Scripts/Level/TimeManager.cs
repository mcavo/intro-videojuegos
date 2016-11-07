using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public Image TimeBar;
	public float TimeLeft;

	Dungeon dungeon;

	// Use this for initialization
	void Start () {
		dungeon = GameObject.Find ("Dungeon").GetComponent<Dungeon> ();
		TimeLeft = dungeon.TimeNeeded(1);
	}
	
	// Update is called once per frame
	void Update () {
		TimeBar.fillAmount += (1/TimeLeft) * Time.deltaTime;

		if (TimeBar.fillAmount >= 1f) {
			
		}

	}
}
