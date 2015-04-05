using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
	public class AnimationInfo : MonoBehaviour
	{

		private bool _doOnce = true;
		//private float _time = 0f;
		private float _speed = 0f;

		public bool _playAtEnd = false;

		void Update()
		{
			if(Data.GameManager.Paused && _doOnce)
			{
				_doOnce = false;
				_speed = this.GetComponent<Animator>().speed;
				this.GetComponent<Animator>().speed = 0;
			}
			else if (!Data.GameManager.Paused && !_doOnce)
			{
				_doOnce = true;
				this.GetComponent<Animator>().speed = _speed;
			}

			if(Data.GameManager.End && _doOnce && !_playAtEnd)
			{
				_doOnce = false;
				_speed = this.GetComponent<Animator>().speed;
				this.GetComponent<Animator>().speed = 0;
			}
		}
	}
}