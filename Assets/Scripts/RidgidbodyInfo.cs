using UnityEngine;
using System.Collections;

public class RidgidbodyInfo : MonoBehaviour
{
	public bool _kinematic = false; //is rigidbody kinematic?
	public bool _fixedAngle = false; //is rigidbody fixed angle?

	private bool _paused = false; //is rigidbody paused?

	private Vector2 _vel = Vector2.zero; //reference to old velocity
	private float _angVel = 0f; //reference to spinning velocity

	/* Testing
	void Start()
	{
		rigidbody2D.AddTorque(100f);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return) && !_paused)
		{
			PauseMotion();
		}
		if(Input.GetKeyDown(KeyCode.Backspace) && _paused)
		{
			UnpauseMotion();
		}
	}
	*/

	//pause rigidbody
	public void PauseMotion()
	{
		if(!_kinematic && !_fixedAngle)
		{
			_vel = this.rigidbody2D.velocity; //save velocity
			_angVel = this.rigidbody2D.angularVelocity; //save angular velocity

			this.rigidbody2D.fixedAngle = true; //set fixed angle
			this.rigidbody2D.isKinematic = true; //set to kinematic to pause
		}
		else if(!_kinematic)
		{
			_vel = this.rigidbody2D.velocity; //save velocity
			this.rigidbody2D.isKinematic = true; //set to kinematic to pause
		}

		_paused = true; //pause
	}

	public void UnpauseMotion()
	{
		if(!_kinematic && !_fixedAngle)
		{
			this.rigidbody2D.isKinematic = false; //set to not kinematic to unpause

			this.rigidbody2D.fixedAngle = false; //set to not fixed angle
			this.rigidbody2D.angularVelocity = _angVel; //reapply angular velocity
			this.rigidbody2D.velocity = _vel; //reapply velocity

			_angVel = 0f; //reset reference
			_vel = Vector2.zero; //reset reference
		}
		else if(!_kinematic)
		{
			this.rigidbody2D.isKinematic = false; //set to not kinematic to unpause
			this.rigidbody2D.velocity = _vel; //reapply velocity
			_vel = Vector2.zero; //reset reference
		}

		_paused = false; //unpause
	}

	public bool Paused
	{
		get{return _paused;}
	}

	public Vector2 Vel
	{
		get{return _vel;}
	}

	public float AngVel
	{
		get{return _angVel;}
	}
}
