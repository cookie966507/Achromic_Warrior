using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
	/*
	 * Manager for controlling sound. DO NOT REMOVE UNUSED CODE
	 * All sounds/music should come through here
	 */
	public class SoundManager : MonoBehaviour
	{
		//reference to the sound manager
		public static SoundManager _instance;

		//whether sfx or music are enabled - not implemented
		//private static bool _sfxEnabled = true;
		//private static bool _musicEnabled = true;

		//volume of the audio types
		private static float _sfxVol = 1f;
		private static float _musicVol = 1f;

		//lists to keep references to the different AudioSources
		private static List<AudioSource> _sfxSources;
		private static List<AudioSource> _musicSources;

		private static List<AudioClip> _clips;

		//volume parameters - not implemented
		//private const float VOL_UP = 1f;
		//private const float VOL_DOWN = 0f;

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
				_clips = new List<AudioClip>();
                if(PlayerPrefs.HasKey(Menu.MenuHandlers.Audio.audioHash+0))
                {
                    _musicVol = PlayerPrefs.GetFloat(Menu.MenuHandlers.Audio.audioHash + 0);
                    _sfxVol = PlayerPrefs.GetFloat(Menu.MenuHandlers.Audio.audioHash + 1);
                }
                else
                {
                    _musicVol = 1f;
                    _sfxVol = 1f;
                }
			}
			//too many sound managers
			else if(_instance != this)
			{
				Destroy(gameObject);
			}
		}

		void OnLevelWasLoaded(int i)
		{
			_clips.Clear();
			_musicSources.Clear();
			_sfxSources.Clear();
		}

		void Update()
		{
			//List<AudioSource> _tempList = new List<AudioSource>();
			//go through the audio sources
			//foreach(AudioSource _source in _sfxSources)
			for(int i = 0; i < _sfxSources.Count; i++)
			{
				//if the source is finished playing
				if(!_sfxSources[i].isPlaying)
				{
					//set the volume to be on just in case it was off
					//_sfxSources[i].volume = VOL_UP;

					//remove the reference because we are done with it (tempList will remove it)
					_clips.Remove(_sfxSources[i].clip);
					_sfxSources.RemoveAt(i);
					//decrement i
					i--;
				}
			}


			//same for the music
			//foreach(AudioSource _source in _musicSources)
			for(int i = 0; i < _musicSources.Count; i++)
			{
				if(!_musicSources[i].isPlaying)
				{
					//_musicSources[i].volume = VOL_UP;

					_clips.Remove(_musicSources[i].clip);
					_musicSources.RemoveAt(i);
					i--;
				}
			}
		}

		//function for playing sfx
		public static void PlaySFX(AudioSource _audio)
		{
			//if sfx is muted, mute the incoming sound
			//if(!_sfxEnabled) _audio.volume = VOL_DOWN;
			_audio.volume = _sfxVol;

			//play it anyway in case we unmute the sound and store a reference
			_sfxSources.Add(_audio);
			_audio.Play();
		}

		//function for playing music
		public static void PlayMusic(AudioSource _audio)
		{
			//if music is muted, mute the incoming sound
			//if(!_musicEnabled) _audio.volume = VOL_DOWN;

			if(!_clips.Contains(_audio.clip))
			{
				_audio.volume = _musicVol;

				//play it anyway in case we unmute the music and store a reference
				_musicSources.Add(_audio);
				_clips.Add(_audio.clip);
				_audio.Play();
			}
		}

		//update the sfx vol based on slider
		public static void SliderSFX(float _vol)
		{
			_sfxVol = _vol;

			for(int i = 0; i < _sfxSources.Count; i++)
			{
				_sfxSources[i].volume = _sfxVol;
			}
		}

		//update the music vol based on slider
		public static void SliderMusic(float _vol)
		{
			_musicVol = _vol;

			for(int i = 0; i < _musicSources.Count; i++)
			{
				_musicSources[i].volume = _musicVol;
			}
		}
//
//		public static void PauseAudio()
//		{
//			for(int i = 0; i < _musicSources.Count; i++)
//			{
//				_musicSources[i].Pause();
//			}
//			for(int i = 0; i < _sfxSources.Count; i++)
//			{
//				_sfxSources[i].Pause();
//			}
//		}
//
//		public static void UnPauseAudio()
//		{
//			for(int i = 0; i < _musicSources.Count; i++)
//			{
//				_musicSources[i].Play();
//			}
//			for(int i = 0; i < _sfxSources.Count; i++)
//			{
//				_sfxSources[i].Play();
//			}
//			Debug.Log("AudioUnpaused");
//		}

		/* - not implemented
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
		*/
	}
}
