using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
	public class LevelLoader : MonoBehaviour
	{
		public string _level = "Menu";

		public void LoadLevel()
		{
			if(_level.Equals("Menu"))
			{
				#if UNITY_WEBPLAYER
				Data.GameManager.GotoLevel("Menu_Web");
				#else
				Data.GameManager.GotoLevel("Menu");
				#endif
			}
			else Data.GameManager.GotoLevel(_level);
		}
	}
}
