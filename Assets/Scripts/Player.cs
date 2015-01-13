using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private Vector3 _colors;

	private PlayerArmor[]_armor;

	void Start ()
	{
		_colors = new Vector3(0f, 0f, 0f);

		_armor = GameObject.FindObjectsOfType<PlayerArmor>();
	}
	
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag.Equals("orb"))
		{

		}

	}

	public Vector3 PlayerColors
	{
		get {return _colors;}
		set{_colors = value;}
	}

	public PlayerArmor[] Armor
	{
		get{return _armor;}
		set{_armor = value;}
	}
}
