using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class DungeonCreator : MonoBehaviour {

	public GameObject DungeonGO;
	public Module startModule;
	public Module endModule;
	public Module[] Modules;
	public Module Wall;

	public int Seed;
	public int MatrixSize = 4;
	public Vector2 start;
	public Vector2 end;

	public string Name;

	private float delta = 2f;
	private float deltaDistance = 40;

	private GameObject Dungeon;

	public void Delete()
	{
		DestroyImmediate(Dungeon);
	}

	private Vector2 GetMatrixPosition (Vector3 position)
	{
		return new Vector2 (Mathf.FloorToInt((position.x + 42.5f) / 40) + start.x, Mathf.FloorToInt((position.z - 2f) / 40) + start.y);
	}

	private Vector2 getNewDirection(Vector2 current) {
		if (current == end) {
			return new Vector2(0,0);
		}
		var posibleDirections = new List<Vector2> ();
		int difX = (int) (end.x - current.x);
		int difY = (int) (end.y - current.y);
		if (difX != 0) {
			posibleDirections.Add (new Vector2 (difX/Mathf.Abs(difX), 0));
		}
		if (difY != 0) {
			posibleDirections.Add (new Vector2 (0, difY/Mathf.Abs(difY)));
		}
		return posibleDirections [Random.Range (0, posibleDirections.Count)];
	}

	private ModuleConnector getNewExitPoint (ModuleConnector oldExit, Vector2 newDirection, List<ModuleConnector> pendingExits, int[,] matrix) {
		var PosiblePrefabs = new List<Module> ();
		foreach(var t in oldExit.Tags) {
			PosiblePrefabs.AddRange(Modules.Where(m => m.Tags.Contains(t)).ToArray());
		}
		//despues de esto tengo todos los prefabs que podrian ser
		ShuffleList(PosiblePrefabs);
		for (int i = 0; i < PosiblePrefabs.Count; i++) {
			var newModule = (Module)Instantiate (PosiblePrefabs [i]);
			newModule.Decorate(1);
			newModule.transform.SetParent (Dungeon.transform);

			var newModuleExits = newModule.GetExits ();

			ShuffleArray (newModuleExits);

			for (int k = 0; k < newModuleExits.Length; k++) {
				var exitToMatch = newModuleExits [k];
				var posibleExits = newModuleExits.Where (e => e != exitToMatch);
				MatchExits (oldExit, exitToMatch);
				foreach (var e in posibleExits) {
					if (System.Math.Round (e.transform.forward.x, 0) == newDirection.x && System.Math.Round (e.transform.forward.z, 0) == newDirection.y) {
						pendingExits.AddRange (newModuleExits.Where (ep => ep != exitToMatch));

						var n = oldExit.transform.position + oldExit.transform.forward * delta;
						Vector2 position = GetMatrixPosition (n);

						matrix [(int)position.x, (int)position.y] = 1;
						Dungeon.GetComponent<Dungeon>().AddSimpleWayModule ();
						return e;
					}
				}
			}

			DestroyImmediate(newModule.gameObject);
		}

		return oldExit;
	}

	public void Generate()
	{
		Random.InitState (Seed);

		Dungeon = Instantiate(DungeonGO) as GameObject;
		Dungeon.name = Name;
		Dungeon.GetComponent<Dungeon>().Initialize (MatrixSize);
		var startsModule = (Module) Instantiate(startModule, transform.position, transform.rotation);
		Dungeon.GetComponent<Dungeon>().AddSimpleWayModule ();
		startsModule.transform.SetParent (Dungeon.transform);
		var pendingExits = new List<ModuleConnector>(startsModule.GetExits());
		var matrix = new int[MatrixSize, MatrixSize];
		var endWay = false;
		matrix [(int)start.x, (int)start.y] = 1;

		var current = start;
		var newD = getNewDirection (current);
		ModuleConnector exitP = pendingExits [0];
		foreach (var e in pendingExits) {
			if (System.Math.Round(e.transform.forward.x, 0) == newD.x
					&& System.Math.Round(e.transform.forward.z, 0) == newD.y) {
				exitP = e;
			}
		}

		while (!endWay) {
			pendingExits.Remove (exitP);
			current = current + newD;
			newD = getNewDirection (current);
			if (current == end)
			{  
				var newModule = (Module)Instantiate (endModule);
				Dungeon.GetComponent<Dungeon>().AddSimpleWayModule ();
				newModule.Decorate (1);
				newModule.transform.SetParent (Dungeon.transform);
				var newModuleExits = newModule.GetExits ();
				var exitToMatch = newModuleExits.FirstOrDefault (x => x.IsDefault) ?? GetRandom (newModuleExits);
				MatchExits (exitP, exitToMatch);
				pendingExits.AddRange (newModuleExits.Where (e => e != exitToMatch));
				var n = exitP.transform.position + exitP.transform.forward * delta;
				Vector2 position = GetMatrixPosition (n);
				matrix [(int)position.x, (int)position.y] = 1;
				endWay = true;
			}
			else
			{
				var newExitP = getNewExitPoint (exitP, newD, pendingExits, matrix);

				if (exitP == newExitP) {
					break;
				}

				exitP = newExitP;
			}

		}
			

		do
		{
			var newExits = new List<ModuleConnector>();
			var duplicatedExits = new List<ModuleConnector>();
			var count = 0;

			for (int i=0 ; i<pendingExits.Count ; i++) {
				
				var pendingExit = pendingExits[i];
				var foundDuplicated = false;


				foreach (var p in duplicatedExits) {
					if (checkEqualPoints(p.transform.position, pendingExit.transform.position)) {
						foundDuplicated = true;
						count = count + 1;
						break;
					}
				}

				if (!foundDuplicated) {
					if (CheckForChunck (matrix, pendingExit)) {
						
						var newTag = GetRandom (pendingExit.Tags);
						var newModulePrefab = GetRandomWithTag (Modules, newTag);

						var newModule = (Module)Instantiate (newModulePrefab);
						Dungeon.GetComponent<Dungeon>().AddModule();
						newModule.Decorate(1);
						newModule.transform.SetParent (Dungeon.transform);

						var newModuleExits = newModule.GetExits ();
						var exitToMatch = newModuleExits.FirstOrDefault (x => x.IsDefault) ?? GetRandom (newModuleExits);
						MatchExits (pendingExit, exitToMatch);

						newExits.AddRange (newModuleExits.Where (e => e != exitToMatch));

						var n = pendingExit.transform.position + pendingExit.transform.forward * delta;
						Vector2 position = GetMatrixPosition (n);

						matrix [(int)position.x, (int)position.y] = 1;

					} else {
						
						ModuleConnector checkDuplicated;
						foundDuplicated = false;
						for(int j = i+1 ; j < pendingExits.Count ; j ++) {
							checkDuplicated = pendingExits[j];
							if(checkEqualPoints(pendingExit.transform.position, checkDuplicated.transform.position)) {
								duplicatedExits.Add(pendingExit);
								foundDuplicated = true;
								break;
							}
						}
						if (!foundDuplicated) {
							for (int j = 0 ; j < newExits.Count ; j ++){
								checkDuplicated = newExits[j];
								if(checkEqualPoints(pendingExit.transform.position, checkDuplicated.transform.position)) {
									duplicatedExits.Add(pendingExit);
									foundDuplicated = true;
									newExits.Add(pendingExit);
									break;
								}
							}
							if (!foundDuplicated) {
								var newModule = (Module)Instantiate (Wall);
								newModule.Decorate(1);
								newModule.transform.SetParent (Dungeon.transform);
								var newModuleExits = newModule.GetExits ();
								var exitToMatch = newModuleExits.FirstOrDefault (x => x.IsDefault) ?? GetRandom (newModuleExits);
								MatchExits (pendingExit, exitToMatch);
							}
						}
					} 
				}
			}
			pendingExits = newExits;


		} while (pendingExits.Count != 0);

	}

	private bool checkEqualPoints(Vector3 p1, Vector3 p2) {
		return Mathf.Approximately (p1.x, p2.x) && Mathf.Approximately (p1.z, p2.z);
	}

	private bool CheckForChunck(int[,] matrix, ModuleConnector pendingExit)
	{
		Vector2 position = GetMatrixPosition (pendingExit.transform.position + pendingExit.transform.forward * delta);
		return position.x >= 0 && position.x < MatrixSize && position.y >= 0 && position.y < MatrixSize 
			&& matrix [(int)position.x, (int)position.y] == 0;

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

	private void ShuffleArray(ModuleConnector[] array)
	{
		int n = array.Count();
		while (n > 1)
		{
			n--;
			int i = Random.Range(0, n+1);
			ModuleConnector temp = array[i];
			array[i] = array[n];
			array[n] = temp;
		}
	}

	private void ShuffleList(List<Module> modules)
	{
		int n = modules.Count();
		while (n > 1)
		{
			n--;
			int i = Random.Range(0, n+1);
			Module temp = modules[i];
			modules[i] = modules[n];
			modules[n] = temp;
		}
	}

}
