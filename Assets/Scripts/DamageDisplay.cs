using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public class DamageDisplay : MonoBehaviour
{
	public GameObject _emptyNums;
	public string _numPath = "Prefabs/Numbers/";

	public float _spacing = 0.35f;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			ShowDamage(Random.Range(0, 150), Vector3.zero, ((ColorElement)Random.Range(0, (float)ColorElement.NumTypes)));
		}
	}

	public void ShowDamage(int _damage, Vector3 _pos, ColorElement _color)
	{
		GameObject _display = (GameObject)GameObject.Instantiate(_emptyNums, _pos, Quaternion.identity);

		int _length = ((int)_damage).ToString().Length;
		float _startX = 0f - _length/2*_spacing;
		if((_length & 1) == 0) _startX += _spacing/2;
		if(_length == 1) _startX = 0;
		for(int i = 0; i < _length; i++)
		{
			GameObject num = (GameObject)Resources.Load(_numPath + _damage.ToString().Substring(i, 1));
			GameObject newNum = (GameObject)GameObject.Instantiate(num);
			newNum.transform.parent = _display.transform;
			newNum.transform.localPosition = new Vector2(_startX + i*_spacing, 0);
			newNum.renderer.material.color = CustomColor.GetColor(_color);
		}
	}
}
