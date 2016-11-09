using UnityEngine;
using System.Linq;

public class Module : MonoBehaviour
{
	public string[] Tags;

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
}
