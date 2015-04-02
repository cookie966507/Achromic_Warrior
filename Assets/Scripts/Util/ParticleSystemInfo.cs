using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
	public class ParticleSystemInfo : MonoBehaviour
	{
		
		private bool _doOnce = true;
		private bool _isPlaying = false;
		
		void Update()
		{
			if(Data.GameManager.Paused && _doOnce)
			{
				_doOnce = false;
				_isPlaying = this.GetComponent<ParticleSystem>().isPlaying;
				if(_isPlaying) this.GetComponent<ParticleSystem>().Pause();
			}
			else if (!Data.GameManager.Paused && !_doOnce)
			{
				_doOnce = true;
				if(_isPlaying) this.GetComponent<ParticleSystem>().Play();
			}
		}
	}
}