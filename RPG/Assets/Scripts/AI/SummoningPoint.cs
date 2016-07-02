// Summoning point. A simple spawnpoint that registers itself with the Summoning stone.
// When the summoning stone wants to it will search through the list for a good place to
// spawn NPCs.

using UnityEngine;
using System.Collections;

public class SummoningPoint : MonoBehaviour {

	public bool isPriority = false;
	private bool isClear = false;
	private int count = 0;

	private float spawnCooldown = 0f;
	private float spawnInterval = 1f;

	void Start()
	{
		SummoningStone.Instance.RegisterPoint (this);
	}

	void OnDeath()
	{
		SummoningStone.Instance.DeRegisterPoint (this);
	}

	public bool IsClear()
	{
		if (count == 0 && Time.time > spawnCooldown)
		{
			return true;
		}

		return false;
	}

	public void SpawnHappened()
	{
		spawnCooldown = Time.time + spawnInterval;
	}

	void OnTriggerEnter(Collider other)
	{
		count++;
	}

	void OnTriggerExit(Collider other)
	{
		count--;
	}
}
