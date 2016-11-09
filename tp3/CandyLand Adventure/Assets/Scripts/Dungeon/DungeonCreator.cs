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
	public Bonus[] bonuses;
	public Trap[] traps;

	public string Name;

	public int Seed;
	public int MatrixSize = 4;
	public Vector2 Start;
	public Vector2 End;

	public int Difficulty;


	private float delta = 2f;

	private GameObject Dungeon;

	public void Clear ()
	{
		var modulesToClear = new List<Module>(Dungeon.GetComponentsInChildren<Module>());
		foreach (var m in modulesToClear) {
			DestroyImmediate(m.gameObject);
		}
	}

	public void Delete ()
	{
		DestroyImmediate(Dungeon);
	}

	private Vector2 GetMatrixPosition (Vector3 position)
	{
		return new Vector2 (Mathf.FloorToInt((position.x + 42.5f) / 40) + Start.x, Mathf.FloorToInt((position.z - 2f) / 40) + Start.y);
	}

	private Vector2 getNewDirection(Vector2 current) {
		if (current == End) {
			return new Vector2(0,0);
		}
		var posibleDirections = new List<Vector2> ();
		int difX = (int) (End.x - current.x);
		int difY = (int) (End.y - current.y);
		if (difX != 0) {
			posibleDirections.Add (new Vector2 (difX/Mathf.Abs(difX), 0));
		}
		if (difY != 0) {
			posibleDirections.Add (new Vector2 (0, difY/Mathf.Abs(difY)));
		}
		return posibleDirections [Random.Range (0, posibleDirections.Count)];
	}

	public void Decorate(Module m, int level) 
	{
		addBonus (m, level);
		addTraps (m, level);
	}

	public Bonus getBonus(int difficulty)
	{
		var bonusPrefab = bonuses[Random.Range(0, bonuses.Length)];
		Bonus bonus = (Bonus)Instantiate (bonusPrefab);
		Dungeon.GetComponent<Dungeon> ().AddBonus ();
		return bonus;
	}

	public Trap getTrap(int difficulty)
	{
		var trapPrefab = traps[Random.Range(0, traps.Length)];
		Trap trap = (Trap)Instantiate (trapPrefab);
		Dungeon.GetComponent<Dungeon> ().AddTrap ();
		return trap;
	}

	public void addBonus(Module m, int difficulty)
	{
		BonusAnchor[] bonusAnchors = m.GetBonusAnchors (difficulty);
		foreach (BonusAnchor bonusAnchor in bonusAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Bonus bonus = getBonus (difficulty);
				bonus.transform.position = bonusAnchor.transform.position;
				bonus.transform.parent = m.gameObject.transform;	
			}
		}
	}

	public void addTraps(Module m, int difficulty)
	{
		TrapAnchor[] trapAnchors = m.GetTrapAnchors (difficulty);
		foreach (TrapAnchor trapAnchor in trapAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Trap trap = getTrap (difficulty);
				trap.transform.position = trapAnchor.transform.position;
				trap.transform.parent = m.gameObject.transform;	
			}
		}
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
			Decorate(newModule, Difficulty);
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
		Dungeon dungeonScript = Dungeon.GetComponent<Dungeon> ();
		dungeonScript.Initialize (Seed, MatrixSize, Start, End, Difficulty);
		var startsModule = (Module) Instantiate(startModule, transform.position, transform.rotation);
		Dungeon.GetComponent<Dungeon>().AddSimpleWayModule ();
		startsModule.transform.SetParent (Dungeon.transform);
		var pendingExits = new List<ModuleConnector>(startsModule.GetExits());
		var matrix = new int[MatrixSize, MatrixSize];
		var endWay = false;
		matrix [(int)Start.x, (int)Start.y] = 1;

		var current = Start;
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
			if (current == End)
			{  
				var newModule = (Module)Instantiate (endModule);
				Dungeon.GetComponent<Dungeon>().AddSimpleWayModule ();
				Decorate (newModule, Difficulty);
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
						Decorate(newModule, Difficulty);
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
								Decorate(newModule, Difficulty);
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
