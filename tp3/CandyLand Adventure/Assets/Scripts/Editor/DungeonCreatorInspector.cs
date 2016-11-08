using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(DungeonCreator))]
public class DungeonCreatorInspector : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		//DungeonCreator myTarget = (DungeonCreator)target;

		//myTarget.seed = EditorGUILayout.IntField("Seed", myTarget.seed);

		EditorGUILayout.HelpBox ("In order to play your dungeon, put the Dungeon prefab at the Assets/Prefabs/Dungeon folder.\nAnd later, add that prefab to the Dungeon list at the GameManager prefab, located at the Assets/Prefabs folder.",MessageType.Info);

		if (GUILayout.Button("Create"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Generate();
		}

		if (GUILayout.Button("Delete"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Delete();
		}

	}
}
