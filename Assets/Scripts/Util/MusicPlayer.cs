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

		void Start()
		{
			SoundManager.PlayMusic(this.GetComponent<AudioSource>());
		}
	}
}
