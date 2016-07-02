using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityWell : MonoBehaviour {

	public float force;
	private List<Collider> colliders;
	private List<Rigidbody> rigidbodies;

	void Start()
	{
		colliders = new List<Collider> ();
		rigidbodies = new List<Rigidbody> ();
	}

	void OnTriggerStay(Collider other)
	{
		if (colliders.Contains (other))
		{
			ApplyGravity (rigidbodies [colliders.IndexOf (other)]);
		} else
		{
			Rigidbody body = other.GetComponent<Rigidbody> ();
			if (body != null)
			{
				colliders.Add (other);
				rigidbodies.Add (body);
			}
		}
	}

	void ApplyGravity(Rigidbody body)
	{
		Vector3 direction = transform.position - body.transform.position;

		if(body.velocity.magnitude < 40)
			body.AddForce (direction * force);
	}
}
