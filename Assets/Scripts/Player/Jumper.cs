using UnityEngine;
using System.Collections;

/*
 * Class controlling a characters ability to jump;
 */
public class Jumper : MonoBehaviour
{
	//force upward
	public float _force = 10;

	//determine if able to jump
	public bool CanJump()
	{
		//on the ground
		bool _grounded = false;

		//layers to collide with
		int _layerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("orb_collector") | (1 << LayerMask.NameToLayer("Default")));
		//compliment to collide with all EXCEPT these layers
		_layerMask = ~_layerMask;

		//raycast to see if we can jump off of something
		RaycastHit2D _hit = Physics2D.Raycast(this.transform.position, Vector3.down, 0.1f, _layerMask);
		//if we hit something
		if(_hit.collider != null)
		   {
			//if falling down
			if(this.transform.parent.rigidbody2D.velocity.y <= 0f)
			{
				//character is able to jump
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
