using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;

public class MenuManager : MonoBehaviour {

	public GameObject gameManager;			//GameManager prefab to instantiate.
	//public GameObject soundManager;			//SoundManager prefab to instantiate.

	public Button RulesButton;
	public Button ControllsButton;
	public Button PlayButton;

	public GameObject Controlls;
	public Button ControllsBackButton;

	public GameObject Rules;
	public Button RulesBackButton;
	public Button RulesMoreButton;

	public GameObject Bonus;
	public Button BonusBackButton;
	public Button BonusPrincipalButton;

	public GameObject DungeonList;
	public GameObject DungeonGrid;
	public GameObject DungeonSelector;

	void Awake ()
	{
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager.instance == null) {
			//Instantiate gameManager prefab
			Instantiate (gameManager);
		}
		//Check if a SoundManager has already been assigned to static variable SoundManager.instance or if it's still null
		//if (SoundManager.instance == null) {
		//Instantiate SoundManager prefab
		//Instantiate(soundManager);
		//}

		FillDungeonList ();
	}

	// Use this for initialization
	void Start () {
		PlayButton.onClick.AddListener (PlayButtonOnClick);
		ControllsButton.onClick.AddListener (ControllsButtonOnClick);
		RulesButton.onClick.AddListener (RulesButtonOnClick);

		ControllsBackButton.onClick.AddListener (ControllsBackButtonOnClick);

		RulesBackButton.onClick.AddListener (RulesBackButtonOnClick);
		RulesMoreButton.onClick.AddListener (RulesMoreButtonOnClick);

		BonusBackButton.onClick.AddListener (BonusBackButtonOnClick);
		BonusPrincipalButton.onClick.AddListener (BonusPrincipalButtonOnClick);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FillDungeonList () {

		for (int i = 0 ; i < GameManager.instance.Dungeons.Length ; i++) {
			var dungeon = Instantiate (DungeonSelector) as GameObject;
			dungeon.transform.SetParent (DungeonGrid.transform);
			dungeon.GetComponent<DungeonSelector> ().SetDungeonIndex (i);
			dungeon.GetComponentInChildren<Text> ().text = GameManager.instance.Dungeons [i].name;
		}
		
	}

	void RulesButtonOnClick() {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (true);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
	}

	void RulesBackButtonOnClick () {
		PrincipalMenu (true);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
	}

	void RulesMoreButtonOnClick () {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (true);
		PlayMenu (false);
	}

	void BonusBackButtonOnClick () {
		RulesButtonOnClick ();
	}

	void BonusPrincipalButtonOnClick () {
		RulesBackButtonOnClick();
	}

	void PlayButtonOnClick () {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (true);
	}

	void ControllsButtonOnClick () {
		PrincipalMenu (false);
		ControllsMenu (true);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
	}

	void ControllsBackButtonOnClick () {
		RulesBackButtonOnClick ();
	}

	void PrincipalMenu (bool show) {
		RulesButton.gameObject.SetActive(show);
		ControllsButton.gameObject.SetActive(show);
		PlayButton.gameObject.SetActive(show);
	}

	void ControllsMenu (bool show) {
		Controlls.SetActive(show);
	}

	void PlayMenu (bool show) {
		DungeonList.SetActive (show);
	}

	void RulesMenu (bool show) {
		Rules.SetActive (show);
	}

	void BonusAndTrapsMenu (bool show) {
		Bonus.SetActive (show);
	}

}
