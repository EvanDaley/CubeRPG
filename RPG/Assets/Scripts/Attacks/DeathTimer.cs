// A simple timer that will destroy a gameobject after lifeSpan (in seconds) + a small random element
// determined by randomFactor. If lifeSpan is 0 and randomFactor is 1 the object will be destroyed in
// 0 to 1 seconds. 

using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour {

	public float randomFactor = 0f;
	public float lifeSpan = 1f;
	private float deathTime;

	// Use this for initialization
	void Start () {
		SetDeathTime ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > deathTime)
			Destroy (gameObject);
	}

	public void SetLifespan(float span, float randomFactor)
	{
		lifeSpan = span;
		this.randomFactor = randomFactor;
	}

	void SetDeathTime()
	{
		float rand = ((float)Random.Range (1, 11) / 10) * randomFactor;
		deathTime = Time.time + lifeSpan + rand;
	}
}
