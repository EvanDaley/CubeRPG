using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform target;
	private Vector3 offset;
	public float moveSpeed = 7f;
	public float allowance = 2f;
	Vector3 targetPoint = Vector3.zero;

	public bool autoTargetParent = false;
	public bool abandonParentOnStart = true;
	public bool dieOnTargetDeath = false;

	// Use this for initialization
	void Start () {
		if (autoTargetParent)
			target = transform.parent;
		
		if (abandonParentOnStart)
			transform.SetParent (null);

		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			targetPoint = target.position + offset;
		} else
		{
			if (dieOnTargetDeath)
				Destroy (gameObject);
		}


		transform.position = Vector3.MoveTowards (transform.position, targetPoint, moveSpeed * Time.deltaTime);
	}
}
