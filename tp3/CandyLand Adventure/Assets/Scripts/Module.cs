using UnityEngine;
using System.Linq;

public enum Chunks { BeveledRoom, Corridor, Junction, Junction2, Junction3, Room, ThreeExitCorridor, Wall }

public class Module : MonoBehaviour
{
	public string[] Tags;
	public Chunks type;
	public Bonus[] bonuses;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

	public BonusAnchor[] GetAnchors(int level)
	{
		BonusAnchor[] anchors = GetComponentsInChildren<BonusAnchor>();
		BonusAnchor[] filterAnchors = anchors.Where (c => c.minorLevel <= level).ToArray ();
		return filterAnchors;
	}

	public void Decorate(int level) 
	{
		BonusAnchor[] bonusAnchors = GetAnchors (level);
		foreach (BonusAnchor bonusAnchor in bonusAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Bonus bonus = getBonus (level);
				bonus.transform.position = bonusAnchor.transform.position;
				bonus.transform.parent = transform;	
			}
		}
	}

	public Bonus getBonus(int level)
	{
		var bonudPrefab = bonuses[Random.Range(0, bonuses.Length)];
		Bonus bonus = (Bonus)Instantiate (bonudPrefab);
		bonus.SetLevel (0);
		return bonus;
	}
}
