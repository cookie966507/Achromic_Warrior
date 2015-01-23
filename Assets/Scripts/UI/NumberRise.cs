using UnityEngine;
using System.Collections;

/*
 * Controls the UI numbers in game
 */
public class NumberRise : MonoBehaviour
{
	//speed at which to move
	private float _speed = 1f;
	//direction to move
	private Vector3 _dir;

	void Awake()
	{
		//init direction
		_dir = new Vector3(Random.Range(-1.5f, 1.5f), 1f, 0f);
	}
	
	void Update ()
	{
		//move object
		transform.Translate(_dir*_speed*Time.deltaTime);
	}
}
