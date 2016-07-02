using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private Rigidbody body;
	private CharacterStat stats;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		stats = GetComponent<CharacterStat> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float horSpeed = Input.GetAxis ("Horizontal");
		float verSpeed = Input.GetAxis ("Vertical");
		float amp = stats.speed + stats.agility;

		body.velocity = new Vector3 (Mathf.Lerp (0, horSpeed * amp, .8f), body.velocity.y, Mathf.Lerp (0, verSpeed * amp, .8f)); 
	}
}
