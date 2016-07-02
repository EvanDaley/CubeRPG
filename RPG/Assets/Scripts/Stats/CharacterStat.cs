using UnityEngine;
using System.Collections;

/// <summary>
/// Character stat. Inherits most of its values (strength, vitality, etc) from Basestat. 
/// Includes stats that shouldn't included in basestat (ability cooldown).
/// </summary>
public class CharacterStat : BaseStat {

	public float abilityCooldownRate = 1f;
	public float teleportDist = 2f;

}
