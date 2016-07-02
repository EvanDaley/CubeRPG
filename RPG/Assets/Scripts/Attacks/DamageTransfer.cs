using UnityEngine;
using System.Collections;

public enum DamageType
{
	ballistic,
	fire,
	water,
	earth,
	lightning,
	wind,
	arcane,
	death,
	life,
	radiation,
	poison,
	light,
	unknown,
	friction,
	magnetic,
	pressure,
	laser,
	psionic,
	emotional,
	explosive
}

public class DamageTransfer : MonoBehaviour 
{
	public bool destroyOnCollision;
	public float damage = 1;
	public DamageType type;
	public Tag sender;
}
