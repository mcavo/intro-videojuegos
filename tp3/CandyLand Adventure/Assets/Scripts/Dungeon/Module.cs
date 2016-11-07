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

	public void Decorate(int level) 
	{
		addBonus (level);
		addTraps (level);
	}

	public Bonus getBonus(int level)
	{
		var bonusPrefab = bonuses[Random.Range(0, bonuses.Length)];
		Bonus bonus = (Bonus)Instantiate (bonusPrefab);
		bonus.SetLevel (0);
		return bonus;
	}

	public Trap getTrap(int level)
	{
		var trapPrefab = traps[Random.Range(0, traps.Length)];
		Trap trap = (Trap)Instantiate (trapPrefab);
		trap.SetLevel (0);
		return trap;
	}

	public void addBonus(int level)
	{
		BonusAnchor[] bonusAnchors = GetBonusAnchors (level);
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

	public void addTraps(int level)
	{
		TrapAnchor[] trapAnchors = GetTrapAnchors (level);
		foreach (TrapAnchor trapAnchor in trapAnchors) 
		{
			// TODO: condition should be change with level var.
			if (Random.Range (0, 1) == 0) {
				Trap trap = getTrap (level);
				trap.transform.position = trapAnchor.transform.position;
				trap.transform.parent = transform;	
			}
		}
	}
}
