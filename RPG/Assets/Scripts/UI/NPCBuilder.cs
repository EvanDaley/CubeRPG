// NOT IMPLEMENTED. This is the beginning of a Constructor for AI characters. I wanted to build 5-10 
// basic ai units and store them in an array. As the player beats waves of enemies we could
// recycle the enemies in the array and simply make them stronger by editing their baseStat/NPCStat components.

using UnityEngine;
using System.Collections;

public class NPCBuilder : MonoBehaviour {

	public GameObject[] NPCPrefabs;
	public static NPCBuilder Instance;

	void Start () {
		Instance = this;
	}
	
	public GameObject GetNextNPC()
	{
		GameObject instance = GameObject.Instantiate (NPCPrefabs[0]) as GameObject;
		return instance;
	}
}
