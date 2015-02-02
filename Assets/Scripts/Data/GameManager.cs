using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Util;

namespace Assets.Scripts.Data
{

	public class GameManager : MonoBehaviour
	{

		public static GameManager _instance;
		public GameState _state = GameState.Game;

		void Awake()
		{
			if(_instance == null)
			{
				DontDestroyOnLoad(gameObject);
				_instance = this;
			}
			else if(_instance != this)
			{
				Destroy(gameObject);
			}
		}

		void Start ()
		{
	
		}
	

		void Update ()
		{
			if(CustomInput.PauseFreshPress && _state.Equals(GameState.Game))
			{
				Pause();
			}
			else if(CustomInput.PauseFreshPress && _state.Equals(GameState.Pause))
			{
				Unpause();
			}
		}

		public void Pause()
		{
		
			VelocityInfo[] _bodies = GameObject.FindObjectsOfType<VelocityInfo>();
			for(int i = 0; i < _bodies.Length; i++)
			{
				_bodies[i].PauseMotion();
				_state = GameState.Pause;
			}
		}

		public void Unpause()
		{
			VelocityInfo[] _bodies = GameObject.FindObjectsOfType<VelocityInfo>();
			for(int i = 0; i < _bodies.Length; i++)
			{
				_bodies[i].UnpauseMotion();
				_state = GameState.Game;
			}
		}

		public GameState State
		{
			get{return _state;}
			set{_state = value;}
		}
	}
}
