// A simple script that inherits from Damage Transfer. Anything that collides with this object
// will take damage based on the damage inherited from the base class. 

using UnityEngine;
using System.Collections;

public class Crusher : DamageTransfer {


	// Use this for initialization
	void Start () 
	{
		sender = GetComponent<Tag> ();
	}

}
