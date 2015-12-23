using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Data;
using Assets.Scripts.Player;

namespace Assets.Scripts.UI
{
	public class Tutorial : MonoBehaviour
	{
		public TextAsset _file;

		private static List<string> _text;

		public static bool _break = false;
		public static bool _transition = false;
		public static bool _attack = false;
		public static bool _block = false;
		public static bool _super = false;
		public static bool _equip = false;
		public static bool _exit = false;
		private static int _equipCounter;

		private static float _exitDelay = 5f;

		private static string _current = "";

		private static int _index = 0;

		public Text _textObject;

		public static float _delay = 7f;
		private static float _timer = 0f;

		private static PlayerColorData _data;

		private static TutorialStateMachine machine = new TutorialStateMachine();
		private delegate void state();
		private state[] doState;
		private TutorialStateMachine.tutorial currState;

		void Start ()
		{
			_data = GameObject.Find("player").GetComponent<PlayerColorData>();
			_equipCounter = 0;
			_break = false;
			_transition = false;
			_attack = false;
			_block = false;
			_super = false;
			_equip = false;
			_exit = false;

			_current = "";
			_index = 0;
			_timer = 0;

			doState = new state[] { Move, Jump, Fall, Attack, Block, Cycle, Equip, Super, Exit };
			_text = new List<string>();

            string[] _arr = _file.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
            for (int i = 0; i < _arr.Length; i++)
			{
				_text.Add(_arr[i]);
			}

			LoadNextText();
		}
		
		void Update ()
		{
			if(!GameManager.Paused)
			{
				//TutorialStateMachine.tutorial prevState = currState;
				currState = machine.update();
				doState[(int)currState]();

				if(_break && _transition)
				{
					_timer += Time.deltaTime;
					if(_timer > _delay)
					{
						_transition = false;
						if(_index < _text.Count - 1)
						{
							_break = false;
							_timer = 0f;
							LoadNextText();
						}
					}
				}
				_textObject.text = _current;

				if(_exit)
				{
					_timer += Time.deltaTime;
					_textObject.text += Mathf.CeilToInt(_exitDelay - _timer).ToString();
					if(_timer > _exitDelay)
					{
						machine.goTo(TutorialStateMachine.tutorial.move);
						Data.GameManager.GotoLevel("Level_Select");
					}
				}
			}
		}

		private static void LoadNextText()
		{
			_current = "";
			string _temp = _text[_index];
			while(_temp != "<break>")
			{
				if(_temp.Equals(""))
				{
					_current += "\n";
				}
				else if(_temp.EndsWith(" <Input>"))
				{
					_current += CustomInput.GetText(_temp);
				}
				else _current += _temp;
				_index++;
				if(_index < _text.Count) _temp = _text[_index];
			}
			if(_index < _text.Count-1)
			{
				_index++;
				_break = true;
			}
		}

		private static void Move()
		{
			if (CustomInput.Left || CustomInput.Right)
			{
				machine.goTo(TutorialStateMachine.tutorial.jump);
				LoadNextText();
			}
		}


		private static void Jump()
		{
			if (CustomInput.JumpFreshPress)
			{
				machine.goTo(TutorialStateMachine.tutorial.fall);
				LoadNextText();
			}
		}


		private static void Fall()
		{
			if (CustomInput.Down && CustomInput.JumpFreshPress)
			{
				machine.goTo(TutorialStateMachine.tutorial.attack);
				LoadNextText();
			}
		}


		private static void Attack()
		{
			if (CustomInput.AttackFreshPress && !_attack)
			{
				LoadNextText();
				_transition = true;
				_attack = true;
			}
			if(_attack)
			{
				doAttack();
			}
		}
		private static void doAttack()
		{
			if(!_transition)
			{
				machine.goTo(TutorialStateMachine.tutorial.block);
			}
		}

		private static void Block()
		{
			if (CustomInput.BlockFreshPress && !_block)
			{
				LoadNextText();
				_transition = true;
				_block = true;
			}
			if(_block)
				doBlock();
		}
		private static void doBlock()
		{
			if(!_transition)
			{
				machine.goTo(TutorialStateMachine.tutorial.cycle);
			}
		}


		private static void Cycle()
		{
			if (CustomInput.CycleLeftFreshPress || CustomInput.CycleRightFreshPress)
			{
				machine.goTo(TutorialStateMachine.tutorial.equip);
				LoadNextText();
			}
		}


		private static void Equip()
		{
			if (CustomInput.ChangeColorFreshPress && !_transition)
			{
				LoadNextText();
				_transition = true;
				_equip = true;
			}
			if(_equip)
			{
				doEquip();
			}
		}
		private static void doEquip()
		{
			if(!_transition)
			{
				if(_equipCounter < 1)
				{
					_equipCounter++;
					_transition = true;
				}
				else
				{
					machine.goTo(TutorialStateMachine.tutorial.super);
				}
			}
		}

		private static void Super()
		{
			if (CustomInput.SuperFreshPress && !_super)
			{
				LoadNextText();
				_transition = true;
				_super = true;
			}
			if(_super)
			{
				doSuper();
			}
			else
			{
				PlayerColorData.MakeSuper(_data);
			}
		}
		private static void doSuper()
		{
			if(!_transition)
			{
				machine.goTo(TutorialStateMachine.tutorial.exit);
			}
		}


		private static void Exit()
		{
			_exit = true;
		}
	}

	class TutorialStateMachine
	{
		internal enum tutorial { move, jump, fall, attack, block, cycle, equip, super, exit };
		private delegate tutorial machine();//function pointer
		private machine[] getNextState;//array of function pointers
		private tutorial currState;
		
		internal TutorialStateMachine()
		{
			currState = tutorial.move;
			//fill array with functions
			getNextState = new machine[] { Move, Jump, Fall, Attack, Block, Cycle, Equip, Super, Exit };
		}
		
		internal tutorial update()
		{
			return currState = getNextState[((int)currState)]();
		}
		
		internal void goTo(tutorial state)
		{
			currState = state;
		}
		
		//The following methods control when and how you can transition between states
		private static tutorial Move()
		{
			return tutorial.move;
		}
		
		private static tutorial Jump()
		{
			return tutorial.jump;
		}
		private static tutorial Fall()
		{
			return tutorial.fall;
		}
		private static tutorial Attack()
		{
			return tutorial.attack;
		}
		private static tutorial Block()
		{
			return tutorial.block;
		}
		private static tutorial Cycle()
		{
			return tutorial.cycle;
		}
		private static tutorial Equip()
		{
			return tutorial.equip;
		}
		private static tutorial Super()
		{
			return tutorial.super;
		}
		private static tutorial Exit()
		{
			return tutorial.exit;
		}
	}
}
