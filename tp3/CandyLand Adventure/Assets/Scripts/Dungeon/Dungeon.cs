using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {

	public int Size;
	public int SimpleWayModules;
	public int TotalModules;

	public void Initialize (int size) {
		Size = size;
		SimpleWayModules = 0;
		TotalModules = 0;
	}

	public float TimeNeeded(int difficulty) {
		return 10f * TotalModules - 5f * SimpleWayModules;
	}

	public void AddSimpleWayModule() {
		SimpleWayModules += 1;
		TotalModules += 1;
	}

	public void AddModule() {
		TotalModules += 1;
	}


}
