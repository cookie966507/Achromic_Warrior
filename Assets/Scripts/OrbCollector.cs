using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public class OrbCollector : MonoBehaviour
{

	private Player _player;

	void Start()
	{
		_player = GameObject.Find("player").GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag.Equals("orb"))
		{
			ColorPickup _orb = col.transform.GetComponent<ColorPickup>();
			_player.AddColor(CustomColor.GetColor(_orb.ColorType), _orb.Amount);
			Destroy(col.gameObject);
		}
	}
}
