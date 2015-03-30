using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Armor of the player
 * Will probably contain more effects later
 * Just updates the color
 */
namespace Assets.Scripts.Player
{
    public class PlayerArmor : MonoBehaviour
    {

        public void UpdateArmor(ColorElement _color)
        {
            this.GetComponent<Renderer>().material.color = CustomColor.GetColor(_color);
        }
        public void UpdateArmor(Color _color)
        {
            this.GetComponent<Renderer>().material.color = _color;
        }
    }
}
