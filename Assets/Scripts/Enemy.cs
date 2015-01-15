using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public class Enemy : MonoBehaviour
{

	public ColorElement _color;

	public float _amount = 10f;

	[HideInInspector]
	public GameObject _orb;
	
	void Start () {
		Init();
	}
	
	public void ProduceOrbs(int _amount)
	{
		for(int i = 0; i < _amount; i++)
		{
			GameObject _newOrb = (GameObject)GameObject.Instantiate(_orb, transform.position, Quaternion.identity);
			_newOrb.GetComponent<ColorPickup>().ColorType = _color;
			_newOrb.GetComponent<ColorPickup>().Amount = _amount;
			_newOrb.rigidbody2D.AddRelativeForce(new Vector2(Random.Range(-1f, 1f), Random.Range(5, 10)), ForceMode2D.Impulse);
		}
	}

	public void Init()
	{
		renderer.material.color = CustomColor.GetColor(_color);
		
		_orb = (GameObject)Resources.Load("Prefabs/orb");
	}
}
