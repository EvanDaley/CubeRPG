using UnityEngine;
using System.Collections;

public class ArmorPlating : Health 
{
	public float startingHealth = 100;
	private Tag parentTag;
	public bool survivesParentDeath = false;
	private int teamId;
	public Transform myOwner;

	void Start()
	{
		if (myOwner != null)
			parentTag = myOwner.GetComponent<Tag> ();
		else
			parentTag = GetComponentInParent<Tag> ();

		curHealth = startingHealth;
		teamId = parentTag.teamId;
	}

	void OnCollisionEnter(Collision collision)
	{
		DamageTransfer damageTransfer = collision.gameObject.GetComponent<DamageTransfer> ();
		if (damageTransfer != null)
		{
			if (damageTransfer.sender != null)
			{
				if (damageTransfer.sender.teamId != teamId)
				{
					curHealth -= damageTransfer.damage;
					SpitArmorDamage (damageTransfer.damage);
					if (damageTransfer.destroyOnCollision)
						Destroy (collision.gameObject);

					if (curHealth < 0)
					{
						GameObject explosion = GameObject.Instantiate (explosionPrefab, transform.position, transform.rotation) as GameObject;
						Destroy (gameObject);
					}
				}
			} else
			{
				print ("The object that fired the bullet did not pass 'sender' parameter to DamageTransfer component or it was destroyed after firing.");
			}
		}
	}

	public void SpitArmorDamage(float damage)
	{
		GameObject damageSpit = GameObject.Instantiate (damageSpitPrefab, transform.position, Quaternion.identity) as GameObject;
		DamageSpit spit = damageSpit.GetComponent<DamageSpit> ();
		spit.SetDamageAmount (damage.ToString ());

		spit.SetColor (new Color(10,10,10));
	}
}
