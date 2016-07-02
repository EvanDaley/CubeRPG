// Simple roller. This script finds the closest Actor of a different team and pushes itself 
// toward that object with a force based on stats.speed

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SimpleRoller : MonoBehaviour {

	private NPCStat stats;
	private Tag targetTag;
	private Tag myTag;
	private Rigidbody rbody;
	private Renderer rend;
	private float switchTargetCooldown;
	public float switchTargetInterval = 1f;
	public float stoppingDistance = 0f;

	void Start()
	{
		stats = GetComponent<NPCStat> ();
		myTag = GetComponent<Tag> ();
		rbody = GetComponent<Rigidbody> ();
		rend = gameObject.GetComponent<MeshRenderer> ();
	}

	/// <summary>
	/// Every frame this Actor pushes itself toward the closest enemy.
	/// </summary>
	void Update () 
	{
		if (targetTag == null || Time.time > switchTargetCooldown)
		{
			switchTargetCooldown = Time.time + switchTargetInterval;
			targetTag = Spy.Instance.GetClosestEnemy (myTag);
			return;
		}
		float distance = Vector3.Distance (transform.position, targetTag.transform.position);
		if (distance < stats.baseRange + 2 && distance > stoppingDistance)
		{
			Vector3 relativePos = targetTag.transform.position - transform.position;
			rbody.AddForce (relativePos.normalized * stats.speed);
		} 

	}
}
