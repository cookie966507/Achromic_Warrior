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
		private float _health = 100f;
		private const float _maxHealth = 100f;
		//how many lives the player has
		private int _lives = 3;

		//health bar
		private Image _bar;
		//lives counter
		private Image _lifeCounter;

		private Sprite[] _nums;

		void Awake()
		{
			//find reference to health bar
			_bar = GameObject.Find("health").GetComponent<Image>();
			//find reference to lives counter
			_lifeCounter = GameObject.Find("LivesUI").GetComponent<Image>();
			//load numbers
			_nums = (Sprite[])Resources.LoadAll<Sprite>("Sprites/UI/Numbers");
		}
		
		void Update ()
		{

			//if all health is lost
			if(_health <= 0)
			{
				//decrement lives and reset health
				Lives--;
				Health = 100f;
			}

			//if all lives are lost enter Lose state
			if(_lives <= 0)
			{
				GameManager.State = GameState.Lose;
			}

			if(Input.GetKeyDown(KeyCode.DownArrow)){
				Lives--;
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				Lives++;
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				Health -= 10;
			}
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				Health += 10;
			}
		}

		// Gets or sets the health.
		public float Health
		{
			get{return _health;}
			set
			{
				_health = value;
				_health = Mathf.Clamp(_health, 0f, _maxHealth);

				_bar.transform.localScale = new Vector3(_health/_maxHealth, 1f, 1f);
			}
		}

		// Gets or sets the lives.
		public int Lives
		{
			get{return _lives;}
			set
			{
				_lives = value;
				_lives = Mathf.Clamp(_lives, 0, 9);
				_lifeCounter.sprite = _nums[_lives];
			}
		}
	}
}
