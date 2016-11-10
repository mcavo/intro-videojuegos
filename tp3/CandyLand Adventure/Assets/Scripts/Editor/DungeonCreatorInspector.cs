using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonCreator))]
public class DungeonCreatorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.HelpBox ("Create and delete Dungeons until get a satisfying combination.\n" +
			"In order to play that dungeon, press the Clear button, put the Dungeon prefab at the Assets/Prefabs/Dungeon folder.\n" +
			"And later, add that prefab to the Dungeon list at the GameManager prefab, located at the Assets/Prefabs folder.",
			MessageType.Info);

		if (GUILayout.Button("Create"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Generate();
		}

		if (GUILayout.Button("Delete"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Delete();
		}

		if (GUILayout.Button("Clear"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Clear();
		}

	}
}
