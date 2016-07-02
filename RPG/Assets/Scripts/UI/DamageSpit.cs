using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageSpit : MonoBehaviour {

	private Text text;

	public void SetColor(Color color)
	{
		if (text == null)
		{
			text = GetComponentInChildren<Text> ();
		}

		text.color = color;
	}

	public void SetDamageAmount(string damage)
	{
		if (text == null)
		{
			text = GetComponentInChildren<Text> ();
		}

		text.text = damage;

	}
} 