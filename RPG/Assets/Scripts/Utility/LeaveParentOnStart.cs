using UnityEngine;
using System.Collections;

public class LeaveParentOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.SetParent (null);
	}

}
