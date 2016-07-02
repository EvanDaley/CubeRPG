using UnityEngine;
using System.Collections;

public class AimAtCamera : MonoBehaviour {

	void Update () 
	{
		transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	}
}
