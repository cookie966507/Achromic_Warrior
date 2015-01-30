using UnityEngine;
using System.Collections;

/*
 * Class handling the player's attack
 */
namespace Assets.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        //reference to the player
        private PlayerColorData _player;
        //time between attacks
        private float _delay = 0.3f;

        void Start()
        {
            //find player
            _player = GameObject.Find("player").GetComponent<PlayerColorData>();
        }

        //called when enabled
        void OnEnable()
        {
            //ccall hide method after a certain time after enabled
            Invoke("Hide", _delay);
        }

        //hide the attack ring
        void Hide()
        {
            this.gameObject.SetActive(false);
        }

        //something is in the attack trigger
        void OnTriggerStay2D(Collider2D col)
        {
            //if it is an enemy
            if (col.transform.tag.Equals("enemy"))
            {
                //hit the enemy
                col.GetComponent<Enemies.Enemy>().Hit((int)_player.Attack, col.transform.position);
            }
        }
    }
}
