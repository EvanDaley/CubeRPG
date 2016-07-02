using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public GameObject explosionPrefab;
	BaseStat stats;
	protected float curHealth;
	protected float maxHealth;

	public GameObject damageSpitPrefab;
	private Tag myTag;
	private Tag lastAttacker = null;
	private float regenCooldown = .2f;
	private float regenInterval = .2f;

	public bool dieOutOfBounds = true;
	public bool surviveAfterDeath = false;
	public bool disableAfterDeath = false;

	bool dead = false;

	void Start () 
	{
		myTag = GetComponent<Tag> ();
		stats = GetComponent<BaseStat> ();
		InitializeHealth ();
	}

	void LevelUp()
	{
		InitializeHealth ();
	}

	void InitializeHealth()
	{
		dead = false;
		curHealth = stats.vitality * 11 + (stats.strength * 6); // plus any other relevant stats
		maxHealth = curHealth;
	}

	void FixedUpdate()
	{
		if (curHealth < maxHealth && Time.time > regenCooldown)
		{
			curHealth += stats.healthRegen;
			regenCooldown = Time.time + regenInterval;
		}

		if (tag.Equals ("Player"))
		{
			SliderControl.Instance.hpSlider.value = (float)(curHealth / maxHealth);
		}

		if (transform.position.y < -20 && dieOutOfBounds)
		{
			Damage (curHealth/20, DamageType.death);
		}
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Boundary")
		{
			if (dieOutOfBounds)
			{
				Damage (curHealth/50, DamageType.death);
			}
		}
		
		DamageTransfer damageTransfer = collision.gameObject.GetComponent<DamageTransfer> ();
		if (damageTransfer != null)
		{
			if (damageTransfer.sender != null)
			{
				if (damageTransfer.sender.teamId != myTag.teamId)
				{
					lastAttacker = damageTransfer.sender;
					Damage (damageTransfer.damage, damageTransfer.type);
					if (damageTransfer.destroyOnCollision)
						Destroy (collision.gameObject);
				}
			} else
			{
				print ("The object that fired the bullet did not pass 'sender' parameter to DamageTransfer component or it was destroyed after firing.");
			}

		}
	}

	public void Damage(float damage, DamageType damageType)
	{
		// check if character is resistant or susceptible to damageType
		// if so, amplify or weaken the damage
		//print ("Damaging: [" + name + "] for (" + damage + ") damage points" );

		if (curHealth >= damage)
		{
			curHealth -= damage;
			SpitDamage (damage);
		} else
		{
			SpitDamage (curHealth);
			curHealth = 0;
		}

		if (tag.Equals ("Player"))
		{
			SliderControl.Instance.hpSlider.value = (float)(curHealth / maxHealth);
		}

		if (curHealth < 1)
		{
			if(dead == false)	
				Die ();
		}
	}

	public void SpitDamage(float damage)
	{
		GameObject damageSpit = GameObject.Instantiate (damageSpitPrefab, transform.position, Quaternion.identity) as GameObject;
		DamageSpit spit = damageSpit.GetComponent<DamageSpit> ();
		spit.SetDamageAmount (damage.ToString ());

		if(myTag.actorType == ActorType.player)
			spit.SetColor (Color.red);
	}

	protected void Die()
	{
		dead = true;
		DropArmor ();
		DropXP ();

		GameObject explosion = GameObject.Instantiate (explosionPrefab, transform.position, transform.rotation) as GameObject;

		if(surviveAfterDeath == false)
			Destroy (gameObject);

		if (disableAfterDeath)
			gameObject.SetActive (false);
	}

	void DropArmor()
	{
		ArmorPlating[] armors = GetComponentsInChildren<ArmorPlating> ();

		foreach (ArmorPlating armor in armors)
		{
			if (armor.survivesParentDeath)
			{
				armor.transform.SetParent (null);
				Rigidbody arbody = armor.GetComponent<Rigidbody> ();

				if (arbody == null)
				{
					armor.gameObject.AddComponent<Rigidbody> ();
				} else
				{
					arbody.isKinematic = false;
				}

				if(arbody != null)
					arbody.AddForceAtPosition ((armor.transform.position - transform.position) * stats.deathExplosionForce, transform.position);
				//arbody.AddForce((armor.transform.position - transform.position) * 100);

				DeathTimer timer = armor.gameObject.AddComponent<DeathTimer> ();
				timer.SetLifespan (4f, 3f);
			}
		}	
	}

	void DropXP()
	{
		int i = stats.expGivenOnDeath;

		while (i > 1000000)
		{
			int xpToGive = 900000;
			if (i < 900000)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (2f, 2f, 2f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			print (xpToGive);

			i -= 900000;
		}
		while (i > 100000)
		{
			int xpToGive = 90000;
			if (i < 90000)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			print (xpToGive);

			i -= 90000;
		}
		while (i > 11000)
		{
			int xpToGive = 8000;
			if (i < 8000)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (1.1f, 1.1f, 1.1f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			print (xpToGive);

			i -= 8000;
		}

		while (i > 1000)
		{
			int xpToGive = 500;
			if (i < 500)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (.7f, .7f, .7f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			//print (xpToGive);

			i -= 500;
		}

		while (i > 200)
		{
			int xpToGive = 190;
			if (i < 190)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (.7f, .7f, .7f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			print (xpToGive);

			i -= 190;
		}

		while (i > 0)
		{
			int xpToGive = 33;
			if (i < 33)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (stats.xpDropPrefab, transform.position, transform.rotation) as GameObject;
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		

			i -= 33;
		}
	}


}
