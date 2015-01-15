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
			_player.AddColor(CustomColor.GetColor(col.transform.GetComponent<ColorPickup>().ColorType));
			Destroy(col.gameObject);
		}
	}
}
