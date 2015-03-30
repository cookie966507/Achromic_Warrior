using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
	public class LevelLoader : MonoBehaviour
	{
		public string _level = "Menu";

		public void LoadLevel()
		{
			Data.GameManager.GotoLevel(_level);
		}
	}
}
