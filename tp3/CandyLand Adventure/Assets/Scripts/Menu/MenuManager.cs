using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Linq;

public class MenuManager : MonoBehaviour {

	public GameObject gameManager;			//GameManager prefab to instantiate.
	public GameObject soundManager;			//SoundManager prefab to instantiate.

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

	public GameObject Difficulty;
	public Button EasyButton;
	public Button ModerateButton;
	public Button HardButton;
	public Button DifficultyBackButton;
	public Button DifficultyMenuButton;

	public GameObject DungeonList;
	public GameObject DungeonGrid;
	public GameObject DungeonSelector;
	public Button DungeonListBackButton;

	void Awake ()
	{
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager.instance == null) {
			//Instantiate gameManager prefab
			Instantiate (gameManager);
		}
		//Check if a SoundManager has already been assigned to static variable SoundManager.instance or if it's still null
		if (SoundManager.instance == null) {
//		Instantiate SoundManager prefab
			Instantiate(soundManager);
		}

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

		EasyButton.onClick.AddListener (EasyButtonOnClick);
		ModerateButton.onClick.AddListener (ModerateButtonOnClick);
		HardButton.onClick.AddListener (HardButtonOnClick);
		DifficultyBackButton.onClick.AddListener (DifficultyBackButtonOnClick);
		DifficultyMenuButton.onClick.AddListener (DifficultyMenuButtonOnClick);

		DungeonListBackButton.onClick.AddListener (DungeonListBackButtonOnClick);

		SoundManager.instance.playBasicMusic ();
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
			dungeon.GetComponent<Button>().onClick.AddListener (DungeonSelectorButtonOnClick);
		}
		
	}

	void RulesButtonOnClick() {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (true);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
		DifficultyMenu (false);
	}

	void RulesBackButtonOnClick () {
		PrincipalMenu (true);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
		DifficultyMenu (false);
	}

	void RulesMoreButtonOnClick () {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (true);
		PlayMenu (false);
		DifficultyMenu (false);
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
		DifficultyMenu (false);
	}

	void ControllsButtonOnClick () {
		PrincipalMenu (false);
		ControllsMenu (true);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
		DifficultyMenu (false);
	}

	void DungeonListBackButtonOnClick() {
		RulesBackButtonOnClick();
	}

	void DungeonSelectorButtonOnClick() {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (false);
		DifficultyMenu (true);
	}

	void EasyButtonOnClick () {
		GameManager.instance.Difficulty = 1;
		SceneManager.LoadScene ("Level");
	}

	void ModerateButtonOnClick () {
		GameManager.instance.Difficulty = 2;
		SceneManager.LoadScene ("Level");
	}

	void HardButtonOnClick () {
		GameManager.instance.Difficulty = 3;
		SceneManager.LoadScene ("Level");
	}

	void DifficultyBackButtonOnClick() {
		PrincipalMenu (false);
		ControllsMenu (false);
		RulesMenu (false);
		BonusAndTrapsMenu (false);
		PlayMenu (true);
		DifficultyMenu (false);
	}

	void DifficultyMenuButtonOnClick () {
		RulesBackButtonOnClick();
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

	void DifficultyMenu (bool show) {
		Difficulty.SetActive (show);
	}

}
