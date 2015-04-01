using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
	public class AnimationInfo : MonoBehaviour
	{

		private bool _doOnce = true;
		//private float _time = 0f;
		private float _speed = 0f;

		void Update()
		{
			if(Data.GameManager.Paused && _doOnce)
			{
				_doOnce = false;
				_speed = this.GetComponent<Animator>().speed;
				this.GetComponent<Animator>().speed = 0;
				//_time = this.GetComponent<Animator>().playbackTime;
				//this.GetComponent<Animator>().playbackTime = 0;
			}
			else if (!Data.GameManager.Paused && !_doOnce)
			{
				_doOnce = true;
				this.GetComponent<Animator>().speed = _speed;
				//this.GetComponent<Animator>().playbackTime = _time;
			}
		}
	}
}