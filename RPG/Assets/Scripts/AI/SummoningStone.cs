using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class SummoningStone: MonoBehaviour
{
	public string prefix = "simple_";
	public GameObject damageSpitPrefab;
	private Tag myTag;
	private int wave = 0;
	private float damageDone = 0;
	public GameObject xpDropPrefab;
	private Tag lastAttacker;
	public Text labelWaveCounter;

	public GameObject waveInstance;

	public static SummoningStone Instance;
	private ScreenOverlay screenOverlay;

	private float waveCooldown;
	private float waveMinInterval = 1f;

	public bool autoStart = false;

	private float checkDeadCooldown;
	private float checkDeadInterval = 1f;

	void Awake()
	{
		Instance = this;
		myTag = GetComponent<Tag> ();
		screenOverlay = Camera.main.GetComponent<ScreenOverlay> ();

		if(autoStart && waveInstance != null)
			waveInstance.SetActive (true);
	}

	void ResetWaves()
	{
		Spy.Instance.DestroyNonPlayerActors ();
		wave = 0;

		damageDone = 0;
		labelWaveCounter.text = wave.ToString();

		if (waveInstance != null)
		{
			Destroy (waveInstance);
		}

		try
		{
			GameObject newWave = Instantiate(Resources.Load(prefix + "wave" + wave, typeof(GameObject))) as GameObject;
			if (waveInstance != null)
				waveInstance = newWave;
		}
		catch
		{
			print("Failed to reset waves");
		}

		Spy.Instance.ActivatePlayersOnTeam (1);
	}

	void Update()
	{
		if (Time.time > waveCooldown)
		{
			if ((damageDone > ((wave * wave) + 3) * 5) && Spy.Instance.GetNumActorsNotOnSpecTeam(1) < 25)
				SummonWave ();
		}

		if (Time.time > checkDeadCooldown)
		{
			if (Spy.Instance.GetNumActorsOnSpecTeam (1) < 1)
			{
				Invoke ("ResetWaves", 1f);
			}
		}
	}

	void SummonWave()
	{
		waveCooldown = Time.time + waveMinInterval;

		//screenOverlay.intensity = .4f;
		//Invoke ("ResetOverlay", 1.4f);

		damageDone = 0;
		wave++;
		labelWaveCounter.text = wave.ToString();
		DropXP ();

		Destroy(waveInstance);

		try
		{
			// create wave
			GameObject newWave = Instantiate(Resources.Load(prefix + "wave" + wave, typeof(GameObject))) as GameObject;
			if (waveInstance != null)
				waveInstance = newWave;
		}
		catch
		{
			// summon endgame wave with increasing difficulties
		}
	}

	void ResetOverlay()
	{
		screenOverlay.intensity = 0f;
	}

	void OnCollisionEnter(Collision collision) 
	{
		DamageTransfer damageTransfer = collision.gameObject.GetComponent<DamageTransfer> ();
		if (damageTransfer != null)
		{
			lastAttacker = damageTransfer.sender;
			damageDone += damageTransfer.damage;
			Damage (damageTransfer.damage, damageTransfer.type);
			//if (damageTransfer.destroyOnCollision)
			//	Destroy (collision.gameObject);
		}
	}

	public void Damage(float damage, DamageType damageType)
	{
			SpitDamage (damage);
	}

	public void SpitDamage(float damage)
	{
		GameObject damageSpit = GameObject.Instantiate (damageSpitPrefab, transform.position, Quaternion.identity) as GameObject;
		DamageSpit spit = damageSpit.GetComponent<DamageSpit> ();
		spit.SetDamageAmount (damage.ToString ());

		spit.SetColor (Color.red);
	}

	/// <summary>
	/// Drop XP. If the character is dropping 10 xp we drop a few small xp blocks. If we are dropping
	/// 1 million xp we drop a few larger xp blocks and a few small ones. The big blocks are necessary
	/// so that if a boss dies and drops 10 million xp we can avoid the instantiation of like 3 million xp 
	/// blocks and crashing the game.
	/// </summary>
	void DropXP()
	{
		// TODO: Simplify this method. Using some basic math you could make this a lot shorter.

		int i = Spy.Instance.GetNumActorsNotOnSpecTeam(1);
		i = (i ^ 3) * wave;

		while (i > 1000000)
		{
			int xpToGive = 900000;
			if (i < 900000)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
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

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
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

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
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

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
			xpInstance.transform.localScale = new Vector3 (.7f, .7f, .7f);
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		
			print (xpToGive);

			i -= 500;
		}

		while (i > 200)
		{
			int xpToGive = 190;
			if (i < 190)
				xpToGive = i;

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
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

			GameObject xpInstance = GameObject.Instantiate (xpDropPrefab, transform.position, transform.rotation) as GameObject;
			ExpTransfer exp = xpInstance.GetComponent<ExpTransfer> ();
			exp.SetTarget (myTag.actorType, lastAttacker, xpToGive);		

			i -= 33;
		}
	}
		
	public void RegisterPoint(SummoningPoint point)
	{
		//summoningPoints.Add (point);
	}

	public void DeRegisterPoint(SummoningPoint point)
	{
		//summoningPoints.Remove (point);
	}
}
