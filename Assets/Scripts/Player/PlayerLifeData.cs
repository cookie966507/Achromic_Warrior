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
		//how many lives the player has
		//private static int _lives = 3;

		//health bar
		private static Image _bar;
		//lives counter
		//private static Image _lifeCounter;

		private static Sprite[] _nums;

		void Awake()
		{
			_health = 100f;
			//find reference to health bar
			_bar = GameObject.Find("health").GetComponent<Image>();
			//find reference to lives counter
			//_lifeCounter = GameObject.Find("LivesUI").GetComponent<Image>();
			//load numbers
			_nums = (Sprite[])Resources.LoadAll<Sprite>("Sprites/UI/Numbers");
		}
		
        //void Update ()
        //{
        //    if(Input.GetKeyDown(KeyCode.DownArrow)){
        //        Lives--;
        //    }
        //    if(Input.GetKeyDown(KeyCode.UpArrow)){
        //        Lives++;
        //    }
        //    if(Input.GetKeyDown(KeyCode.LeftArrow)){
        //        Health -= 10;
        //    }
        //    if(Input.GetKeyDown(KeyCode.RightArrow)){
        //        Health += 10;
        //    }
        //}

        public static void damageHealth(int damage)
        {
            _health -=damage;
			if(_health <= 0)
			{
				//decrement lives and reset health
				//_lives--;
                //_lives = Mathf.Clamp(_lives, 0, 9);
				//_lifeCounter.sprite = _nums[_lives];
				//_health = 100f;
                // transfer to death state?
				GameManager.State = GameState.Lose;
			}

			//if all lives are lost enter Lose state
			//if(_lives <= 0)
			//{
			//	GameManager.State = GameState.Lose;
			//}
            
			_health = Mathf.Clamp(_health, 0f, _maxHealth);
			_bar.transform.localScale = new Vector3(_health/_maxHealth, 1f, 1f);//if all health is lost
        }

		// Gets or sets the health.
		public float Health
		{
			get{return _health;}
		}

		// Gets or sets the lives.
		//public int Lives
		//{
		//	get{return _lives;}
		//}
	}
}
