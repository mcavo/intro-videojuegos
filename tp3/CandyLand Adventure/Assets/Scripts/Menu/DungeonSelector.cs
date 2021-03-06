﻿using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DungeonSelector : MonoBehaviour
{

	private int index = 0;

	// Use this for initialization
	void Start ()
	{
		this.GetComponent<Button>().onClick.AddListener (ButtonOnClick);
	}

	public void SetDungeonIndex(int index)
	{
		this.index = index;
	}

	void ButtonOnClick()
	{
		GameManager.instance.DungeonToPlay = index;
	}

}
