using UnityEngine;
using System.Collections;

public class DestroyOnPlayerExit : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Health> () != null)
		{
			Destroy (this.gameObject);
		}
	}
}
