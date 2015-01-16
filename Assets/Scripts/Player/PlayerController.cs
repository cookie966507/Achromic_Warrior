using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	//private Player _player;
	private Jumper _jumper;

	void Start ()
	{
		//_player = GameObject.Find("player").GetComponent<Player>();
		_jumper = this.GetComponentInChildren<Jumper>();
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.RightArrow))
		{
			rigidbody2D.AddRelativeForce(new Vector3(5, 0, 0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			rigidbody2D.AddRelativeForce(new Vector3(-5, 0, 0));
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(_jumper.CanJump())
			{
				rigidbody2D.AddForce(new Vector2(0f, _jumper.Force), ForceMode2D.Impulse);
			}
		}

		rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -50, 50), rigidbody2D.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.transform.tag.Equals("enemy"))
		{
			col.transform.GetComponent<Enemy>().ProduceOrbs(1);
			col.transform.rigidbody2D.AddRelativeForce(new Vector2(1000f, 0f));
		}
	}
}
