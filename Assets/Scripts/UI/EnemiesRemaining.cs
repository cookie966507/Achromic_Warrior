using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class EnemiesRemaining : MonoBehaviour
	{
		public GameObject _parent;
		public GameObject _emptyUI;

		List<GameObject> _ui;

		public Sprite[] _sprites;

		//spacing between numbers
		public float _spacing = 0.35f;

		void Awake()
		{
			_ui = new List<GameObject>();
		}

		//show damage at a point and make it a certain color
		public void UpdateEnemiesRemaining(int _enemies)
		{
			if(_ui.Count > 0)
			{
				for(int i = 0; i < _ui.Count; i++)
				{
					Destroy(_ui[i]);
				}
			}
			_ui.Clear();

			//length of the string representation of the value
			int _length = ((int)_enemies).ToString().Length;
			
			//start to the left of the center for odd digit numbers
			float _startX = 0f - _length / 2 * _spacing;
			
			//if one digit set at origin
			if (_length == 1) _startX = 0;
			//if even digit number adjust spacing
			else if ((_length & 1) == 0) _startX += _spacing / 2;
			
			//create number
			for (int i = 0; i < _length; i++)
			{
				//find digit
				Sprite num = _sprites[int.Parse(_enemies.ToString().Substring(i, 1))];
				GameObject newNum = (GameObject)GameObject.Instantiate(_emptyUI);
				newNum.GetComponent<Image>().sprite = num;
				//set within empty num
				newNum.transform.SetParent(_parent.transform);
				//arrange position based on digit
				newNum.transform.localPosition = new Vector2(_startX + i * _spacing, 0);
				newNum.GetComponent<Image>().color = CustomColor.GetColor(Assets.Scripts.Enums.ColorElement.Black);
				newNum.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
				_ui.Add(newNum);
			}
		}
	}
}
