using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuHanders
{
	public class LevelSelect : MonoBehaviour
	{
		private string _selectedLevel = "attack_level";
		private const int NUMLEVELS = 3;
		private int _levelCounter = 0;

		private float _z = 0f;
		private float _currZ = 0f;
		private float _zVel = 0f;
		private float _speed = 0.1f;

		public GameObject _levelPanel;

		
		void Update ()
		{
			if(CustomInput.LeftFreshPress || CustomInput.CycleLeftFreshPress)
			{
				if(_levelCounter != NUMLEVELS) UpdateSelector(-1);
			}
			if(CustomInput.RightFreshPress || CustomInput.CycleRightFreshPress)
			{
				if(_levelCounter != NUMLEVELS) UpdateSelector(1);
			}
			if(CustomInput.AcceptFreshPress)
			{
				Data.GameManager.GotoLevel(_selectedLevel);
			}
		
			_currZ = Mathf.SmoothDamp(_currZ, _z, ref _zVel, _speed);
			
			_levelPanel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _currZ));
		}

		private void UpdateSelector(int _dir)
		{
			_z += (120*_dir);

			//increase index and reset if necessary
			_levelCounter += _dir;
			if(_levelCounter == NUMLEVELS) _levelCounter = 0;
			else if(_levelCounter == -1) _levelCounter = NUMLEVELS-1;

			switch(_levelCounter)
			{
			case 0:
				_selectedLevel = "attack_level";
				break;
			case 1:
				_selectedLevel = "speed_level";
				break;
			case 2:
				_selectedLevel = "defense_level";
				break;
			case NUMLEVELS:
				_selectedLevel = "boss";
				break;
			}
		}
	}
}
