// BaseStat contains the basic values that are inherited by every character. 
// Human players are given CharacterStat which inherits from this class. 
// AI characters are given NPCStat, which also inherits from here.

using UnityEngine;
using System.Collections;

public class BaseStat : MonoBehaviour {

	// the stats that every 'character' inherits:
	public float agility = 1f;
	public float speed = 1f;
	public float fireRate = 1f;
	public float shotForce = 800f;
	public float accuracy = 1;
	public int vitality = 1;
	public int strength = 1;
	public float baseRange = 10f;
	public float healthRegen = 0f;
	public float lockingForce = 0f;
	public float maxBulletVelocity = 15;
	public float deathExplosionForce = 100;
	public int expGivenOnDeath;
	public GameObject xpDropPrefab;
	private int level = 1;

	public int Level
	{
		get 
		{
			return level;
		}

		set 
		{
			level = value;
			print ("Level: " + level + ", Next requirement: " + xpNextLevel);
			SliderControl.Instance.xpText.text = "Lvl " + level;

			vitality++;
			strength++;
			fireRate *= .98f;

			//healthRegen += .1f;
			lockingForce += .5f;

			BroadcastMessage ("LevelUp");

		}
	}

	private int curXP = 0;
	private int xpNextLevel = 14;
	private int prevRequirement = 0;

	public int CurXP
	{
		get 
		{
			return curXP;
		}

		set 
		{
			curXP = value;

			while (curXP > xpNextLevel)
			{
				Level = Level + 1;
				prevRequirement = xpNextLevel;
				xpNextLevel = (int)((Mathf.Pow (level, 2f)) + 100f*Level);
			}


			print ("Current xp: " + curXP + ", req: " + xpNextLevel);


			SliderControl.Instance.xpSlider.value = (float)(curXP - prevRequirement) / (float)xpNextLevel;


			// level up?
		}
	}

	private int bossKills = 0;
	private int standardKills = 0;

	public void SendXP(int xp, ActorType actor)
	{
		if (xp > 0)
			CurXP = CurXP + xp;

		// todo keep track of actors killed
	}

	public float GetDamageAmplifier(DamageType damageType)
	{
		float amplifier = 1;
	
		// now we can add to amplifier based on proficiencies, buffs, handicapps, etc
		if (damageType == DamageType.fire)
			amplifier += 1.2f;

		// TODO: loop through proficiencies. Add bonuses based on ability
		//
		//

		// TODO: loop through effects on this player. If the player has a bonus damage effect, apply it
		// foreach(Effect effect in effectsList)
		// {
		// 		add to amplifier
		// }

		return amplifier;
	}
}
