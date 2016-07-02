using UnityEngine;
using System.Collections;

public class ControlledRotation : MonoBehaviour {

    public Vector3 rot;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rot*Time.deltaTime);
	}
}
