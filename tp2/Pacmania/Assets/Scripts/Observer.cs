using UnityEngine;
using System.Collections;

namespace ObserverPattern
{
	//Wants to know when another object does something interesting 
	public abstract class Observer : MonoBehaviour
	{
		public abstract void OnNotify();
	}

}
