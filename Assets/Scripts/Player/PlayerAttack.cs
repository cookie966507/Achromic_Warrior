using UnityEngine;
using System.Collections;

/*
 * Class handling the player's attack
 */
namespace Assets.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        void Start()
        {
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        //hide the attack ring
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
