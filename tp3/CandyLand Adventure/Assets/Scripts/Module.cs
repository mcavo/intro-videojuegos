using UnityEngine;

public enum Chunks { BeveledRoom, Corridor, Junction, Junction2, Junction3, Room, ThreeExitCorridor, Wall }

public class Module : MonoBehaviour
{
	public string[] Tags;
	public Chunks type;
	public Bonus[] bonus;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

	public void Decorate(int level) 
	{
		switch (type) 
		{
		case Chunks.BeveledRoom:
			DecorateBeveledRoom (level);
			break;
		case Chunks.Corridor:
			DecorateCorridor (level);
			break;
		case Chunks.Junction:
			DecorateJunction (level);
			break;
		case Chunks.Junction2:
			DecorateJunction2 (level);
			break;
		case Chunks.Room:
			DecorateRoom (level);
			break;
		case Chunks.ThreeExitCorridor:
			DecorateThreeExitCorridor (level);
			break;
		}
	}

	private void DecorateBeveledRoom(int level) {
		//TODO: is missing to use level parameter
		for (int i = 0; i < 2; i++) {
			Bonus b = getBonus ();
			b.transform.position = new Vector3 (i * 2f + 1, transform.position.y, i * 10f + 3);
			b.transform.parent = transform;
		}
	}

	private void DecorateCorridor(int level) {
		if (level > 4)
		{
			//TODO: set some traps
		}
	}

	private void DecorateJunction(int level) {
		if (level > 3)
		{
			//TODO: set some traps
		}
	}

	private void DecorateJunction2(int level) {
		if (level > 3)
		{
			//TODO: set some traps
		}
	}

	private void DecorateRoom(int level) {
		//TODO: is missing to use level parameter
		for (int i = 0; i < 2; i++) {
			Bonus b = getBonus ();
			b.transform.position = new Vector3 (i * 2f + 1, transform.position.y, i * 10f + 3);
			b.transform.parent = transform;
		}
	}

	private void DecorateThreeExitCorridor(int level) {
		if (level > 3)
		{
			//TODO: set some traps
		}
	}

	public Bonus getBonus()
	{
		var bonudPrefab = bonus[Random.Range(0, bonus.Length)];
		return (Bonus)Instantiate (bonudPrefab);
	}
}
