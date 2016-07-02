using UnityEngine;
using System.Collections;

public class TeleportToClosestTag : MonoBehaviour {

	public int targetTeam = 1;
	private bool foundTarget = false;
	private Tag target = null;

	void Start () {
		target = Spy.Instance.GetClosestFriend (transform.position, targetTeam);

		if (target != null)
		{
			foundTarget = true;
			transform.position = target.transform.position;
		}
		else
			print ("Coundn't find target");
	}

	void Update()
	{
		if (foundTarget == false)
		{
			target = Spy.Instance.GetClosestFriend (transform.position, targetTeam);
			if(target != null)
			{
				print ("Found target");

				foundTarget = true;
				transform.position = target.transform.position;
			}
		}

	}
}

