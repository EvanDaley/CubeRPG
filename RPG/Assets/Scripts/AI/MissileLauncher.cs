// An AI Script. Automatically fire at the closest Actor of a different team.

using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour {

	public GameObject missilePrefab;

	private float weaponCoolTime = .1f;
	private Tag myTag;
	private Tag targetTag;

	private float switchTargetInterval = 4f;	// how frequently we SHOULD search for a new target
	private float switchTargetCooldown;			

	private float canTargetInterval = 1f;		// the minimum time span bewteen searching for new target
	private float canTargetCooldown;

	private NPCStat stats;						// the base stats of this character (vitality, strength, etc)
	private Vector3 targetPoint;
	private float width;

	public float damage = 2;
	public DamageType damageType;

	/// <summary>
	/// Initialize this instance. Cache relevant components and check how big our character is
	/// </summary>
	void Start()
	{
		stats = GetComponent<NPCStat> ();
		myTag = GetComponent<Tag> ();

		if (GetComponent<Collider> ())
			width = GetComponent<Collider> ().bounds.extents.x;
		else
			width = 2;
		width = width + (width / 4);
	}

	void Update () {
		if (Time.time > weaponCoolTime)
		{
			TryFireWeapon ();
		}
	}

	/// <summary>
	/// Tries to fire weapon. If we have a target and the target is in range we call FireWeapon
	/// </summary>
	void TryFireWeapon()
	{
		if (targetTag == null)
		{
			SetTargetPoint ();
			return;
		}

		if(targetTag == null || Time.time > switchTargetCooldown)
			SetTargetPoint ();

		float distance = Vector3.Distance (transform.position, targetTag.transform.position);
		if (distance < stats.baseRange)
		{
			weaponCoolTime = Time.time + stats.fireRate;
			FireWeapon ();
		}
	}

	/// <summary>
	/// Fires the weapon in a random direction. The missile automatically tracks the target Tag.
	/// </summary>
	void FireWeapon()
	{
		Vector3 relativePos = Random.onUnitSphere;
		Quaternion directionToTarget = Quaternion.LookRotation (relativePos);

		GameObject bulletInstance = GameObject.Instantiate (missilePrefab, transform.position + relativePos.normalized*(width)*2f, directionToTarget) as GameObject;
		Rigidbody bulletBody = bulletInstance.GetComponent<Rigidbody> ();
		bulletBody.AddForce (relativePos.normalized * stats.shotForce);

		PassDamageToBullet (bulletInstance);
	}

	/// <summary>
	/// Calculates the base damage the weapon should do and applies that to the bullet instance.
	/// </summary>
	/// <param name="bulletInstance">Bullet instance.</param>
	void PassDamageToBullet(GameObject bulletInstance)
	{
		DamageTransfer dt = bulletInstance.GetComponent<DamageTransfer> ();
		dt.sender = myTag;

		// create base damage based on strength and other factors
		int rand = PseudoRandom.getRandomNormal();
		float baseDamage = stats.strength * 2 + damage * (stats.strength / 8) + rand;
		dt.type = damageType;


		// add base damage based on this players boosts / damagetype
		baseDamage = baseDamage * (stats.GetDamageAmplifier(damageType));
		dt.damage = baseDamage;

	}

	/// <summary>
	/// Gets the closest Target Tag from Spy and targets it's position. 
	/// </summary>
	void SetTargetPoint()
	{
		switchTargetCooldown = Time.time + switchTargetInterval;

		if (Time.time > canTargetCooldown)
			canTargetCooldown = Time.time + canTargetInterval;
		else
			return;

		targetTag = Spy.Instance.GetClosestEnemy (myTag);
		if (targetTag != null)
		{
			targetPoint = targetTag.transform.position;
		}
	}
}
