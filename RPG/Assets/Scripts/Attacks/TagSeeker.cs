// Move toward a given target. This behavior makes the bullets automatically track enemies when 
// the player is a high enough level.

using UnityEngine;
using System.Collections;

public class TagSeeker : MonoBehaviour {

	private Tag senderTag;
	private Tag targetTag;
	private Rigidbody rbody;
	private BaseStat stats;

	private bool targetLocked = false;

	void Start()
	{
		rbody = GetComponent<Rigidbody> ();
		senderTag = GetComponent<DamageTransfer> ().sender;

		if(senderTag != null)
			stats = senderTag.GetComponent<BaseStat> ();
	}

	void Update () {

		// this comes first in case we hardcoded a target but don't have a sender tag (delivering xp)
		if (targetTag != null)
		{
			Vector3 relativePos = targetTag.transform.position - transform.position;
			rbody.AddForce (relativePos.normalized * stats.lockingForce);
		}

		if (stats == null)
			return;

		if (stats.lockingForce == 0)
		{
			rbody.velocity = Vector3.ClampMagnitude (rbody.velocity, stats.maxBulletVelocity);
			return;
		}

		if (senderTag != null && targetTag == null)
		{
			targetTag = Spy.Instance.GetClosestEnemy (senderTag);
		}

		// clamp the velocity
		rbody.velocity = Vector3.ClampMagnitude (rbody.velocity, stats.maxBulletVelocity);
	}
}
