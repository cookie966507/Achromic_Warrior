using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		//public GameObject _bossPanel;
		private List<GameObject> _children;
		private List<GameObject> _topInstructions;

		//public Text _bossText;

		void Awake()
		{
			_children = new List<GameObject>();
			_topInstructions = new List<GameObject>();

			_children.Add(GameObject.Find("Red_Preview"));
			_children.Add(GameObject.Find("Green_Preview"));
			_children.Add(GameObject.Find("Blue_Preview"));
			
			_children[1].SetActive(false);
			_children[2].SetActive(false);

			_topInstructions.Add(GameObject.Find("Exit_Instruction_Label"));
			_topInstructions.Add(GameObject.Find("Tutorial_Instruction_Label"));
		}
		
		void Update ()
		{
			if(CustomInput.UsePad)
			{
				_topInstructions[0].GetComponent<Text>().text = "Press: " + CustomInput.GamePadCancel + " Button";
				_topInstructions[1].GetComponent<Text>().text = "Press: " + CustomInput.GamePadChangeColor + " Button";
			/*	if(_levelPanel.activeSelf)
					_bossText.text = "Press \"" + CustomInput.GamePadSuper + "\" for the boss level";
				else
					_bossText.text = "Press \"" + CustomInput.GamePadSuper + "\" for the normal levels"; */
			}
			else{
				_topInstructions[0].GetComponent<Text>().text = "Press: " + CustomInput.KeyBoardCancel.ToString() + " Key";
				_topInstructions[1].GetComponent<Text>().text = "Press: T Key";
			/*	if(_levelPanel.activeSelf)
					_bossText.text = "Press \"" + CustomInput.KeyBoardSuper.ToString() + "\" for the boss level";
				else
					_bossText.text = "Press \"" + CustomInput.KeyBoardSuper.ToString() + "\" for the normal levels"; */
			}

			if(_levelPanel.activeSelf)
			{
				if(CustomInput.LeftFreshPress || CustomInput.CycleLeftFreshPress)
				{
					if(_levelCounter != NUMLEVELS) UpdateSelector(-1);
				}
				if(CustomInput.RightFreshPress || CustomInput.CycleRightFreshPress)
				{
					if(_levelCounter != NUMLEVELS) UpdateSelector(1);
				}
			}
			if(CustomInput.AcceptFreshPressDeleteOnRead)
			{
				Data.GameManager.GotoLevel(_selectedLevel);
			}
			if(CustomInput.CancelFreshPressDeleteOnRead)
			{
				Data.GameManager.GotoLevel("Menu");
			}

			if((CustomInput.UsePad && CustomInput.ChangeColorFreshPressDeleteOnRead) || (!CustomInput.UsePad && Input.GetKeyDown(KeyCode.T)))
			{
				Data.GameManager.GotoLevel("training");
			}
			/*
			if(CustomInput.SuperFreshPressDeleteOnRead)
			{
				if(_levelPanel.activeSelf)
				{
					_levelPanel.SetActive(false);
					_bossPanel.SetActive(true);
					UpdateSelector(0);
					_selectedLevel = "boss";
				}
				else
				{
					_bossPanel.SetActive(false);
					_levelPanel.SetActive(true);
				}
			}
			*/
		
			_currZ = Mathf.SmoothDamp(_currZ, _z, ref _zVel, _speed);
			
			_levelPanel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _currZ));
		}

		private void UpdateSelector(int _dir)
		{
			_z += (120*_dir);
			_children[_levelCounter].SetActive(false);
			//increase index and reset if necessary
			_levelCounter += _dir;
			if(_levelCounter == NUMLEVELS) _levelCounter = 0;
			else if(_levelCounter == -1) _levelCounter = NUMLEVELS-1;

			_children[_levelCounter].SetActive(true);

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
			}
		}
	}
}
