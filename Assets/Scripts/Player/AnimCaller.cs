using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player
{
    class AnimCaller : MonoBehaviour
    {
        private PlayerController player;
        void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        public void AnimDetector()
        {
            player.AnimDetector();
        }
    }
}
