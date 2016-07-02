// The behaviour attached to the cubes that deliver xp from kills. OnCollision, if we hit player, deliver xp.

using UnityEngine;
using System.Collections;

public class ExpTransfer : MonoBehaviour {

	public ActorType killedActor;
	public Tag murderer;
	public int xp;

	public Rigidbody rbody;

	public void SetTarget(ActorType killedActor, Tag murderer, int xp)
	{
		rbody = GetComponent<Rigidbody> ();
		this.murderer = murderer;
		this.killedActor = killedActor;
		this.xp = xp;

		if (murderer == null || rbody == null)
			Destroy (this.gameObject);
	}

	// if we have a character to deliver xp to, move in that direction every fixed frame (roughly .2 seconds)
	void FixedUpdate () {
		if (murderer != null && rbody != null)
		{
			Vector3 relativePos = murderer.transform.position - transform.position;
			rbody.AddForce (relativePos * 5f);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		Tag otherTag = col.gameObject.GetComponent<Tag>();
		if (otherTag != null)
		{
			if (otherTag == murderer)
			{
				BaseStat baseStat = col.gameObject.GetComponent<BaseStat>();
				if (baseStat != null)
				{
					baseStat.SendXP (xp, killedActor);
					Destroy (this.gameObject);
				}
			}
		}
	}
}
