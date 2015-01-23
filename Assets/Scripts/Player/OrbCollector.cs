using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Class that collects orbs
 */
public class OrbCollector : MonoBehaviour
{
	//reference to the player
	private Player _player;

	void Start()
	{
		//find player
		_player = GameObject.Find("player").GetComponent<Player>();
	}

	//something enters collector trigger
	void OnTriggerEnter2D(Collider2D col)
	{
		//if it is an orb
		if(col.transform.tag.Equals("orb"))
		{
			//get the color and destroy the orb ----- ADD COLLECTING EFFECT LATER
			ColorPickup _orb = col.transform.GetComponent<ColorPickup>();
			_player.AddColor(CustomColor.GetColor(_orb.ColorType), _orb.Amount);
			_orb.GetComponent<Destroyer>().DestroyImmediate();
		}
	}
}
