using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
	/*
	 * Manager for controlling sound.
	 * All sounds/music should come through here
	 */
	public class SoundManager : MonoBehaviour
	{
		//reference to the sound manager
		public static SoundManager _instance;

		//whether sfx or music are enabled
		private static bool _sfxEnabled = true;
		private static bool _musicEnabled = true;

		//lists to keep references to the different AudioSources
		private static List<AudioSource> _sfxSources;
		private static List<AudioSource> _musicSources;

		//volume parameters
		private const float VOL_UP = 1f;
		private const float VOL_DOWN = 0f;


		void Awake()
		{
			//if the manager is null
			if(_instance == null)
			{
				//init the manager
				DontDestroyOnLoad(gameObject);
				_instance = this;
				_sfxSources = new List<AudioSource>();
				_musicSources = new List<AudioSource>();

			}
			//too many sound managers
			else if(_instance != this)
			{
				Destroy(gameObject);
			}
		}

		void Update()
		{
			List<AudioSource> _tempList = new List<AudioSource>();
			//go through the audio sources
			foreach(AudioSource _source in _sfxSources)
			{
				//if the source is finished playing
				if(!_source.isPlaying)
				{
					//set the volume to be on just in case it was off
					_source.volume = VOL_UP;
					//remove the reference because we are done with it (tempList will remove it)
					_tempList.Add(_source);
				}
			}
			//avoiding collection errors related to foreach loops
			foreach (AudioSource _source in _tempList)
			{
				_sfxSources.Remove(_source);
			}
			_tempList.Clear();


			//same for the music
			foreach(AudioSource _source in _musicSources)
			{
				if(!_source.isPlaying)
				{
					_source.volume = VOL_UP;
					_tempList.Add(_source);
				}
			}

			foreach (AudioSource _source in _tempList)
			{
				_musicSources.Remove(_source);
			}
			_tempList.Clear();
		}

		//function for playing sfx
		public static void PlaySFX(AudioSource _audio)
		{
			//if sfx is muted, mute the incoming sound
			if(!_sfxEnabled) _audio.volume = VOL_DOWN;

			//play it anyway in case we unmute the sound and store a reference
			_sfxSources.Add(_audio);
			_audio.Play();
		}

		//function for playing music
		public static void PlayMusic(AudioSource _audio)
		{
			//if music is muted, mute the incoming sound
			if(!_musicEnabled) _audio.volume = VOL_DOWN;

			//play it anyway in case we unmute the music and store a reference
			_musicSources.Add(_audio);
			_audio.Play();
		}

		//muting/unmuting sfx
		public static void ToggleSFX()
		{
			//mute/unmute
			_sfxEnabled = !_sfxEnabled;

			//set volume accordingly
			if(_sfxEnabled)
			{
				SoundManager.ToggleCurrentSFX(VOL_UP);
			}
			else
			{
				SoundManager.ToggleCurrentSFX(VOL_DOWN);
			}
		}

		//muting/unmuting the music
		public static void ToggleMusic()
		{
			//mute/unmute
			_musicEnabled = !_musicEnabled;

			//set volume accordingly
			if(_musicEnabled)
			{
				SoundManager.ToggleCurrentMusic(VOL_UP);
			}
			else
			{
				SoundManager.ToggleCurrentMusic(VOL_DOWN);
			}
		}

		//update the volume for currently playing sounds
		private static void ToggleCurrentSFX(float _volume)
		{
			foreach(AudioSource _source in _sfxSources)
			{
				_source.volume = _volume;
			}
		}

		//update the volume for currently playing music
		private static void ToggleCurrentMusic(float _volume)
		{
			foreach(AudioSource _source in _musicSources)
			{
				_source.volume = _volume;
			}
		}
	}
}
