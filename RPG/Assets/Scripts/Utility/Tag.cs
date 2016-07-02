using UnityEngine;
using System.Collections;

public enum ActorType
{
	player,
	npc,
	boss,
	projectile,
	effect,
}

public class Tag: MonoBehaviour
{
	[HideInInspector]
	public GameObject playerIcon;

	[HideInInspector]
	public GameObject npcIcon;

	[HideInInspector]
	public GameObject bossIcon;

	[HideInInspector]
	public GameObject projectileIcon;

	[HideInInspector]
	public GameObject effectIcon;

	public int teamId;
	public ActorType actorType;

	void Start()
	{
		Spy.Instance.RegisterActor (this);

		GameObject [] mapIconPrefabs = new GameObject[]{playerIcon, npcIcon, bossIcon, projectileIcon, effectIcon};
		GameObject icon = mapIconPrefabs[(int)actorType];

		if (icon != null)
		{
			GameObject iconInstance = GameObject.Instantiate (icon, transform.position, transform.rotation) as GameObject;
			iconInstance.transform.SetParent (transform);
		} else
			print ("icon null");
	}

	void OnDeath()
	{
		Spy.Instance.DeRegisterActor (this);
	}
}
