using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Data;

namespace Assets.Scripts.UI
{
	public class Tutorial : MonoBehaviour
	{
		private string _path = "Assets/Resources/Files/tutorial.txt";

		private List<string> _text;

		public bool _break = false;

		string _current = "";

		int _index = 0;

		public Text _textObject;

		public float _delay = 2f;
		private float _timer = 0f;


		void Start ()
		{
			_text = new List<string>();

			// Create reader & open file
			StreamReader _file = new StreamReader(_path);
			//string _contents = _file.ReadToEnd();

			string _temp = "";

			while((_temp = _file.ReadLine()) != null)
			{
				_text.Add(_temp);
			}

			//close the stream
			_file.Close();

			//Debug.Log(_contents);

			LoadNextText();
		}
		
		void Update ()
		{
			if(!GameManager.Paused)
			{
				if(_break)
				{
					_timer += Time.deltaTime;
					if(_timer > _delay)
					{
						if(_index < _text.Count - 1)
						_break = false;
						_timer = 0f;
						LoadNextText();
					}
				}
			}
		}

		private void LoadNextText()
		{
			_current = "";
			string _temp = _text[_index];
			while(_temp != "\\break/")
			{
				if(_temp.Equals(""))
				{
					_current += "\n";
				}
				else if(_temp.EndsWith(" \\Input/"))
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
			_textObject.text = _current;
		}
	}
}
