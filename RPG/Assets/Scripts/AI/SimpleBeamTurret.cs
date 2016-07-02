// SimpleBeamTurret. This inherits from DamageTransfer, so anything that collides with this object
// will take damage! When the player is in range the beam will activate and linearly interpolate
// toward the characters position. If the character touches the beam they take damage every frame
// in the OnTriggerStay method.

using UnityEngine;
using System.Collections;

public class SimpleBeamTurret : DamageTransfer {

	public Vector3 targetPoint;
	private Tag myTag;
	private BaseStat stat;
	private bool activated;
	private Tag targetTag;
	public float targetInterval = 2f;

	private float targetCooldown = 0;
	public float rotationSpeed = 1f;

	public Health damageTarget = null;
	public LayerMask mask;
	private BoxCollider col;

	void Start () {
		myTag = GetComponentInParent<Tag> ();
		stat = GetComponentInParent<NPCStat> ();
		col = GetComponent<BoxCollider> ();
		base.sender = myTag;

		Activated = false;
	}

	public bool Activated
	{
		set 
		{
			activated = value;
			if (activated)
			{
				// activate particle weapon
				Transform child = transform.GetChild(0);
				child.gameObject.SetActive (true);
				col.enabled = true;
			} else
			{
				Transform child = transform.GetChild(0);
				child.gameObject.SetActive (false);
				col.enabled = false;
			}
		}

		get 
		{
			return activated;
		}
	}

	void OnTriggerStay(Collider other)
	{
		Tag otherTag = other.GetComponent<Tag> ();
		if (otherTag != null)
		{
			if (otherTag.teamId != myTag.teamId)
			{
				damageTarget = other.GetComponent<Health> ();
				if(damageTarget != null)
					damageTarget.Damage (damage + PseudoRandom.getRandomNormal(), type);
			}
		}
	}
	
	void Update () 
	{
		if (Time.time > targetCooldown)
		{
			targetCooldown = Time.time + targetInterval;
			targetTag = Spy.Instance.GetClosestEnemy (myTag);
		}

		if (targetTag != null)
		{
			Ray ray = new Ray (transform.position, targetTag.transform.position - transform.position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, stat.baseRange, mask))
			{
				if (hit.transform.gameObject.GetComponent<Tag> () == targetTag)
				{
					Activated = true;
				} else
				{
					Activated = false;
				}
			}
					
			//transform.LookAt (targetTag.transform);
			Vector3 direction = targetTag.transform.position - transform.position;
			Quaternion toRotation = Quaternion.LookRotation (direction);
			transform.rotation = Quaternion.Lerp (transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
		}
	}
}
