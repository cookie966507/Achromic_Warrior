using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

/*
 * This class makes sure level music will play through the SoundManager
 */
namespace Assets.Scripts.Util
{
	public class MusicPlayer : MonoBehaviour
	{
		public AudioClip[] _clips;

		private int _clipIndex = 0;
		public bool _loop = false;

		void Awake()
		{
			this.GetComponent<AudioSource>().clip = _clips[_clipIndex];
		}

		void Start()
		{
			if(_clips.Length == 1 && _loop) this.GetComponent<AudioSource>().loop = true;
			SoundManager.PlayMusic(this.GetComponent<AudioSource>());
		}

		void Update()
		{
			if(!this.GetComponent<AudioSource>().isPlaying)
			{
				if(_clipIndex < _clips.Length-1)
				{
					_clipIndex++;
					if(_clipIndex == _clips.Length-1 && _loop) this.GetComponent<AudioSource>().loop = true;
					this.GetComponent<AudioSource>().clip = _clips[_clipIndex];
					SoundManager.PlayMusic(this.GetComponent<AudioSource>());
				}
			}
		}
	}
}
