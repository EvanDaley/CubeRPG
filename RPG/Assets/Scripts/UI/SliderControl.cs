using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour {

	public Slider hpSlider;
	public Slider mpSlider;
	public Slider xpSlider;
	public Text xpText;
	public static SliderControl Instance;

	void Start()
	{
		Instance = this;
	}

}
