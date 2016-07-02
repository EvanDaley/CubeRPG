// After initialization move in a fixed direction every frame. 
// If direction is not set in the inspector we calculate a random direction.
// randomFactor directly affects the speed of the object.

using UnityEngine;
using System.Collections;

public class MoveDirectionOnStart : MonoBehaviour {

	public Vector3 direction;
	public float speed;
	public float randomFactor = 0;

	void Start () 
	{
		if (direction == Vector3.zero)
			direction = Random.onUnitSphere;


		if(randomFactor > 1)
			speed += ((float)Random.Range (1, 11) / 10) * randomFactor;
	}
	
	void Update () 
	{
		transform.Translate (direction * speed * Time.deltaTime);
	}
}
