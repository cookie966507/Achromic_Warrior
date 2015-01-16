using UnityEngine;
using System.Collections;

public class Jumper : MonoBehaviour
{
	private float _force = 10;

	public bool CanJump()
	{
		bool _grounded = false;

		RaycastHit2D _hit = Physics2D.Raycast(this.transform.position, Vector3.down, 0.1f, (1 << LayerMask.NameToLayer("terrain")));
		if(_hit.collider != null)
		   {
			if(this.transform.parent.rigidbody2D.velocity.y <= 0f)
			{
				_grounded = true;
			}
		}

		return _grounded;
	}

	public float Force
	{
		get{return _force;}
		set{_force = value;}
	}
}
