using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Meter to display each r, g, b of the players collectec colors
 */
public class ColorMeter : MonoBehaviour
{
	//size of the bar
	private float _size = 1f;

	//reference to the color wheel
	private ColorWheel _wheel;

	void Start ()
	{
		//find wheel
		_wheel = GameObject.Find("ColorWheel").GetComponent<ColorWheel>();

		//init scale at 0
		this.transform.localScale = new Vector3(0f, 1f, 1f);
	}

	//update the meter with some amount
	public void UpdateMeter(float _amount)
	{
		//keep meter within bounds
		_amount = Mathf.Clamp(_amount, 0f, _size);
		this.transform.localScale = new Vector3(_amount, 1f, 1f);
		//update tabs that should be revealed or hidden
		_wheel.UpdateTabs();
	}
}
