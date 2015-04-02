using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

/*
 * This class makes sure level music will play through the SoundManager
 */
namespace Assets.Scripts.Util
{
	public class SoundPlayer : MonoBehaviour
	{
		public AudioClip _clip;
		
		public bool _loop = false;
		
		void Awake()
		{
			this.GetComponent<AudioSource>().clip = _clip;
		}
		
		void Start()
		{
			if(_loop) this.GetComponent<AudioSource>().loop = true;
			SoundManager.PlaySFX(this.GetComponent<AudioSource>());
		}
	}
}
