using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Data;
using Assets.Scripts.Enums;

/*
 * This class will handle all the info regarding the player's life
 */
namespace Assets.Scripts.Player
{
	public class PlayerLifeData : MonoBehaviour
	{
		//health of the player
		private static float _health = 100f;
		private const float _maxHealth = 100f;

		//health bar
		private static Image _bar;

		void Awake()
		{
			_health = 100f;
			//find reference to health bar
			_bar = GameObject.Find("health").GetComponent<Image>();
		}

        public static void damageHealth(int damage)
        {
            _health -=damage;
			if(_health <= 0)
			{
				if(!Application.loadedLevelName.Equals("training"))
				{
					GameManager.State = GameState.Lose;
					GameManager.Pause();
				}
			}
			            
			_health = Mathf.Clamp(_health, 0f, _maxHealth);
			_bar.transform.localScale = new Vector3(_health/_maxHealth, 1f, 1f);//if all health is lost
        }

		// Gets or sets the health.
		public float Health
		{
			get{return _health;}
		}
	}
}
