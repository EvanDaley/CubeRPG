[System.Serializable]
public class Proficiency
{
	// NOT IMPLEMENTED. The idea here was to give the player certain boosts based on their play-style

	string title = "x Proficiency";
	string description = "Boost the damage of x attacks";
	DamageType damageType;
	// optional add boost type enum
	float multiplier = 1.2f;
}
