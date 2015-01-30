using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Class for applying damage in a specific area
 * Can be used ofr hazards
 */
namespace Assets.Scripts
{
    public class DamageArea : MonoBehaviour
    {
        //amount of damage
        public float _damage;

        //frequency of damage
        private float _timer;
        private float _delay = 1.5f;

        //cause damage if within the trigger of the area
        void OnTriggerStay2D(Collider2D col)
        {
            _timer += Time.deltaTime;
            if (_timer > _delay)
            {
                if (col.transform.tag.Equals("Player"))
                {
                    //show damage
                    UI.DamageDisplay.instance.ShowDamage((int)(_damage - col.GetComponent<Player.PlayerColorData>().Defense), col.transform.position, ColorElement.Black);
                    _timer = 0f;
                }
            }
        }
    }
}
