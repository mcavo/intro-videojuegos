using UnityEngine;
using System.Linq;

public class Module : MonoBehaviour
{
	public string[] Tags;
	public Bonus[] bonuses;
	public Trap[] traps;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}

	public BonusAnchor[] GetBonusAnchors(int level)
	{
		return GetComponentsInChildren<BonusAnchor>().Where (c => c.minorLevel <= level).ToArray ();
	}

	public TrapAnchor[] GetTrapAnchors(int level)
	{
		return GetComponentsInChildren<TrapAnchor>().Where (c => c.minorLevel <= level).ToArray ();
	}

	private int addBonus(int difficulty)
	{
		int count = 0;
		if (traps.Count() == 0) 
		{
			return count;
		}
		BonusAnchor[] bonusAnchors = GetBonusAnchors (difficulty);
		foreach (BonusAnchor bonusAnchor in bonusAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Bonus bonus = getBonus (difficulty);
				bonus.transform.position = bonusAnchor.transform.position;
				bonus.transform.parent = gameObject.transform;	
				count++;
			}
		}
		return count;
	}

	private int addTraps(int difficulty)
	{
		int count = 0;
		TrapAnchor[] trapAnchors = GetTrapAnchors (difficulty);
		if (traps.Count() == 0) 
		{
			return count;
		}
		foreach (TrapAnchor trapAnchor in trapAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Trap trap = getTrap (difficulty);
				trap.transform.position = trapAnchor.transform.position;
				trap.transform.parent = gameObject.transform;	
				count++;
			}
		}
		return count;
	}

	public void Decorate(int level, Dungeon dungeon) 
	{
		int trapsCount = addTraps (level);
		int bonusCount = addBonus (level);
//		dungeon.AddBonuses (bonusCount);
//		dungeon.AddTraps (trapsCount);
	}

	public Bonus getBonus(int difficulty)
	{
		var bonusPrefab = bonuses[Random.Range(0, bonuses.Length)];
		Bonus bonus = (Bonus)Instantiate (bonusPrefab);
		return bonus;
	}

	public Trap getTrap(int difficulty)
	{
		var trapPrefab = traps[Random.Range(0, traps.Length)];
		Trap trap = (Trap)Instantiate (trapPrefab);
		return trap;
	}
}
