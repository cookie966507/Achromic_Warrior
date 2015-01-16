using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public class ColorMeter : MonoBehaviour
{
	private float _size = 1f;

	void Start ()
	{
		this.transform.localScale = new Vector3(0f, 1f, 1f);
	}

	public void UpdateMeter(float _amount)
	{
		_amount = Mathf.Clamp(_amount, 0f, _size);
		this.transform.localScale = new Vector3(_amount, 1f, 1f);
	}
}
