using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {

	public int SimpleWayModules;
	public int TotalModules;

	public int BonusQuantity;
	public int TrapsQuantity;

	public int Seed;
	public int Size;
	public Vector2 Start;
	public Vector2 End;
	public int Difficulty;

	private float TimeToCrossModule = 5f;

	public void Initialize (int seed, int size, Vector2 start, Vector2 end, int difficulty)
	{
		Seed = seed;
		Size = size;
		Start = start;
		End = end;
		Difficulty = difficulty;
		SimpleWayModules = 0;
		TotalModules = 0;
	}

	public float TimeNeeded()
	{
		float extraTime = Mathf.Log(Size) * ((float) ( Size * TimeToCrossModule ));
		return (SimpleWayModules * TimeToCrossModule + extraTime)/ Mathf.Log(Difficulty * 5) ;
	}

	public void AddSimpleWayModule()
	{
		SimpleWayModules += 1;
		TotalModules += 1;
	}

	public void AddModule()
	{
		TotalModules += 1;
	}

}
