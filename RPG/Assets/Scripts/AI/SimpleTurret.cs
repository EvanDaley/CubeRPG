using UnityEngine;
using System.Collections;

public class SimpleTurret : MonoBehaviour {

	public GameObject bulletPrefab;

	private float weaponCoolTime = .1f;
	private Tag myTag;
	private Tag targetTag;

	private float switchTargetInterval = 4f;
	private float switchTargetCooldown;

	private float canTargetInterval = 1f;
	private float canTargetCooldown;

	private NPCStat stats;
	private Vector3 targetPoint;
	private float width;

	public float damage = 2;
	public DamageType damageType;

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

	void FireWeapon()
	{
		Vector3 relativePos = targetTag.transform.position - transform.position;
		Quaternion directionToTarget = Quaternion.LookRotation (relativePos);

		GameObject bulletInstance = GameObject.Instantiate (bulletPrefab, transform.position + relativePos.normalized*(width), directionToTarget) as GameObject;
		Rigidbody bulletBody = bulletInstance.GetComponent<Rigidbody> ();
		bulletBody.AddForce (relativePos.normalized * stats.shotForce);

		PassDamageToBullet (bulletInstance);
	}

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
