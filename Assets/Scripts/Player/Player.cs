using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private Color _color;
	
	private PlayerArmor[]_armor;
	
	void Start ()
	{
		_color = new Color(0f, 0f, 0f);
		
		_armor = GameObject.FindObjectsOfType<PlayerArmor>();
	}
	
	void Update ()
	{
		
	}

	public void AddColor(Color _newColor)
	{
		_color += _newColor;

		_color.r = Mathf.Clamp(_color.r, 0f, 1f);
		_color.g = Mathf.Clamp(_color.g, 0f, 1f);
		_color.b = Mathf.Clamp(_color.b, 0f, 1f);
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