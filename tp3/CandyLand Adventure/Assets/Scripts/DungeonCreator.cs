using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class DungeonCreator : MonoBehaviour {

	public Module[] Modules;
	public int MatrixSize = 4;

	public int Seed;
	private int minDonutsPerRoom = 1;
	private int maxDonutsPerRoom = 3;
	private float delta = 2f;
	public float deltaDistance = 40;
	public GameObject sphere;

	private Transform Dungeon;

	public void Clear()
	{
		var modulesToClear = new List<Module>(GameObject.Find ("Dungeon").GetComponentsInChildren<Module>());
		foreach (var m in modulesToClear) {
			DestroyImmediate(m.gameObject);
		}
	}

	private int[] GetMatrixPosition (Vector3 position)
	{
		return new int[]{ (int)((position.x + 20) / 40), (int)((position.z + 20) / 40) };
	}

	public void Generate()
	{
		Random.InitState (Seed);

		Dungeon = GameObject.Find ("Dungeon").GetComponent<Transform>();

		var startModule = (Module) Instantiate(Modules[2], transform.position, transform.rotation);
		startModule.transform.SetParent (Dungeon);
		var pendingExits = new List<ModuleConnector>(startModule.GetExits());
		var matrix = new int[MatrixSize, MatrixSize];
		//int x = Random.Range (0, MatrixSize);
		//int y = Random.Range (0, MatrixSize);
		int xPos = 0;
		int yPos = 0;
		Debug.Log (xPos + " " + yPos);
		matrix[xPos,yPos] = 1;

		do
		{
			var newExits = new List<ModuleConnector>();

			foreach (var pendingExit in pendingExits) {
				if (CheckForChunck (matrix, pendingExit)) {
					var newTag = GetRandom (pendingExit.Tags);
					var newModulePrefab = GetRandomWithTag (Modules, newTag);
					var newModule = (Module)Instantiate (newModulePrefab);
					newModule.transform.SetParent (Dungeon);
					var newModuleExits = newModule.GetExits ();
					var exitToMatch = newModuleExits.FirstOrDefault (x => x.IsDefault) ?? GetRandom (newModuleExits);
					MatchExits (pendingExit, exitToMatch);
					newExits.AddRange (newModuleExits.Where (e => e != exitToMatch));
					var n = pendingExit.transform.position + pendingExit.transform.forward * delta;
					Debug.Log(n.x + " " + n.z);
					int[] position = GetMatrixPosition (n);

					matrix [position [0], position [1]] = 1;

				} else {
					// Agregue la pared
				} 
			}

			pendingExits = newExits;

		} while (pendingExits.Count != 0);

	}

	private bool CheckForChunck(int[,] matrix, ModuleConnector pendingExit)
	{
		int[] position = GetMatrixPosition (pendingExit.transform.position + pendingExit.transform.forward * delta);
		return position [0] >= 0 && position [0] < MatrixSize && position [1] >= 0 && position [1] < MatrixSize 
			&& matrix [position [0], position [1]] == 0;

	}

	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}

	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}

	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}

}
