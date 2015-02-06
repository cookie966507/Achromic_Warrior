using UnityEngine;
using System.Collections;

/*
 * Class handling the player's attack
 */
namespace Assets.Scripts.Util
{
    public class ObjectHider : MonoBehaviour
    {
        void Awake()
        {
            Hide();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
