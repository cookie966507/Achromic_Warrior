using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	private Transform _target; //target to follow

	private float _speed = 0.1f; //speed of the camera

	private float _xVel; //ref for smooth damp
	private float _yVel; //ref for smooth damp

	private float _x; //target x
	private float _y; //target y
	private float _currX; //current x
	private float _currY; //current y

	void Start ()
	{
		_target = GameObject.Find("Player Camera Target").transform; //find target
	}
	
	void Update ()
	{
		_x = _target.position.x;
		_y = _target.position.y;

		_currX = Mathf.SmoothDamp(_currX, _x, ref _xVel, _speed);
		_currY = Mathf.SmoothDamp(_currY, _y, ref _yVel, _speed);

		this.transform.position = new Vector3(_currX, _currY, this.transform.position.z);

	}

	public Transform Target
	{
		get{return _target;}
		set{_target = value;}
	}
}
