// The player's weapon. Left click for machine gun, righ click to dash.

using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public GameObject bulletPrefab;
	public GameObject trailPrefab;

	private CharacterStat stats;
	private Vector3 targetPoint;
	private float weaponCoolTime = .1f;
	private float abilityCoolTime = 1f;

	public float weaponDamage = 2;
	public DamageType damageType;
	public Tag myTag;

	public LayerMask layermask;

	void Start()
	{
		stats = GetComponent<CharacterStat> ();
		myTag = GetComponent<Tag> ();
	}

	void Update () {

		if (Input.GetButton ("Fire1"))
		{
			if (Time.time > weaponCoolTime)
			{
				SetTargetPoint ();
				weaponCoolTime = Time.time + stats.fireRate;
				FireWeapon ();
			}
		}

		if (Input.GetButton ("Fire2"))
		{
			if (Time.time > abilityCoolTime)
			{
				SetTargetPoint ();
				abilityCoolTime = Time.time + stats.abilityCooldownRate;
				FireAbility ();
			}
		}
	}

	void FireWeapon()
	{
		Vector3 relativePos = targetPoint - transform.position;
		Quaternion directionToTarget = Quaternion.LookRotation (relativePos);

		GameObject bulletInstance = GameObject.Instantiate (bulletPrefab, transform.position + relativePos.normalized*2, directionToTarget) as GameObject;
		Rigidbody bulletBody = bulletInstance.GetComponent<Rigidbody> ();
		bulletBody.AddForce (relativePos.normalized * stats.shotForce);

		PassDamageToBullet (bulletInstance);
	}

	void PassDamageToBullet(GameObject bulletInstance)
	{
		DamageTransfer dt = bulletInstance.GetComponent<DamageTransfer> ();
		dt.sender = myTag;
	
		// create base damage based on strength and other factors
		//int randElement = PseudoRandom.getRandomNormal();

		int randElement = PseudoRandom.getRandomNormal();
		float baseDamage = (stats.strength/2) + (weaponDamage * (stats.strength*2 / (1+randElement))) + randElement * (stats.strength);
		dt.type = damageType;

		// add base damage based on this players boosts / damagetype
		baseDamage = baseDamage * (stats.GetDamageAmplifier(damageType));
		dt.damage = baseDamage;

	}

	void FireAbility()
	{
		// if teleport
		Vector3 relativePos = (targetPoint - transform.position) + Vector3.up;
		int trailParticles = (int)Mathf.Min (Mathf.Ceil (relativePos.magnitude), Mathf.Ceil(stats.teleportDist)) - 1;
		CreateTeleportTrail (trailParticles);
		transform.position = Vector3.MoveTowards (transform.position, targetPoint + Vector3.up*2, stats.teleportDist);
	}

	void CreateTeleportTrail(int n)
	{
		for (int i = 0; i < n; i++)
		{
			Vector3 jumpPos = Vector3.MoveTowards (transform.position, targetPoint + Vector3.up*2, stats.teleportDist);
			Vector3 relativePos = (jumpPos - transform.position) + Vector3.up;
			GameObject instance = GameObject.Instantiate(trailPrefab, transform.position, transform.rotation) as GameObject;
			instance.transform.position += i*relativePos / (n);
		}
	}

	void SetTargetPoint()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 100, layermask))
		{
			targetPoint = hit.point;
		}
	}

}
