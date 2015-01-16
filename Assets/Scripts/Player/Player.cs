using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private Color _color;
	
	private PlayerArmor[]_armor;

	private ColorMeter _r;
	private ColorMeter _g;
	private ColorMeter _b;
	
	void Start ()
	{
		_color = new Color(0f, 0f, 0f);
		
		_armor = GameObject.FindObjectsOfType<PlayerArmor>();

		_r = GameObject.Find("R").GetComponent<ColorMeter>();
		_g = GameObject.Find("G").GetComponent<ColorMeter>();
		_b = GameObject.Find("B").GetComponent<ColorMeter>();
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			Debug.Log(_color);
		}
	}

	public void AddColor(Color _newColor, float _amount)
	{
		_newColor = CustomColor.ConvertColor(_newColor.r*_amount, _newColor.g*_amount, _newColor.b*_amount);
		_color += _newColor;

		_color.r = Mathf.Clamp(_color.r, 0f, 1f);
		_color.g = Mathf.Clamp(_color.g, 0f, 1f);
		_color.b = Mathf.Clamp(_color.b, 0f, 1f);
		_color.a = Mathf.Clamp(_color.a, 0f, 1f);

		_r.UpdateMeter(_color.r);
		_g.UpdateMeter(_color.g);
		_b.UpdateMeter(_color.b);
	}

	public Color PlayerColor
	{
		get {return _color;}
		set{_color = value;}
	}
	
	public PlayerArmor[] Armor
	{
		get{return _armor;}
		set{_armor = value;}
	}
}