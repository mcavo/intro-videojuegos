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

		EditorGUILayout.HelpBox ("Hola",MessageType.Info);

		if (GUILayout.Button("Create"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Generate();
		}

		if (GUILayout.Button("Clear"))
		{
			GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>().Clear();
		}

	}
}
