using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Armor of the player
 * Will probably contain more effects later
 * Just updates the color
 */
public class PlayerArmor : MonoBehaviour {

	public void UpdateArmor(ColorElement _color)
	{
		this.renderer.material.color = CustomColor.GetColor(_color);
	}
	public void UpdateArmor(Color _color)
	{
		this.renderer.material.color = _color;
	}
}
