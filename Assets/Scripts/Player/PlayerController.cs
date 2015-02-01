using UnityEngine;
using System.Collections;
using Assets.Scripts;

/*
 * Controls te player's movement
 */
public class PlayerController : MonoBehaviour
{
	//reference to the jumper
	private Jumper _jumper;
	//jump button pressed
	private bool _jump = false;
	//how much control you have in the air
	private float _airControl = 0.3f;

	//reference to the attack
	public PlayerAttack _attack;

	//firection the player is facing
	[HideInInspector]
	public bool _facingRight = true;

	//how quickly player accelerates
	public float _moveForce = 100f;
	//capping the speed of the player
	public float _maxSpeed = 4f;

	//for ghosting through platforms
	private float _ghostTimer = 0f;
	private float _ghostDelay = 0.5f;
	private bool _ghost = false;

	//cheat for tricking Unity when dealing with collision magic
	private Collider2D _playerCollider;
	private bool _colSwitch = false;


	void Awake()
	{
		//find jumper
		_jumper = this.GetComponentInChildren<Jumper>();
		//find collider
		_playerCollider = this.GetComponent<BoxCollider2D>();
	}
	
	void Update () {
		//if our cheat is on, out collider is off, so we need to tun it back on
		if(_colSwitch)
		{
			_colSwitch = false;
			_playerCollider.enabled = true;
		}

		//if jump is pressed
		if(CustomInput.JumpFreshPress && !CustomInput.Down)
		{
			_jump = true;
		}

		//if attack is pressed
		if(CustomInput.AttackFreshPress)
		{
			_attack.gameObject.SetActive(true);
		}

		//if down is pressed
		if(CustomInput.Down && CustomInput.JumpFreshPress)
		{
			//we should be ghosting
			_ghost = true;
			_ghostTimer = 0f;

			_colSwitch = true;
			_playerCollider.enabled = false;
		}
		//if ghosting
		if(_ghost) _ghostTimer += Time.deltaTime;

		//if ghosting is over
		if(_ghostTimer > _ghostDelay)
		{
			_ghost = false;
			_ghostTimer = 0;
		}

		//ignore collision between the player and platforms
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("platform"), _ghost);
	}

	//fixed update runs on a timed cycle (for physics stuff)
	void FixedUpdate ()
	{
		//caching the horizontal input
		float _h;

		//assign value based on left or right input
		if(CustomInput.Left) _h = -1f;
		else if(CustomInput.Right) _h = 1f;
		else _h = 0f;


		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(_h * rigidbody2D.velocity.x < _maxSpeed)
		{
			//account for air control
			if(_jumper.CanJump()) rigidbody2D.AddForce(Vector2.right * _h * _moveForce);
			else rigidbody2D.AddForce(Vector2.right * _h * _moveForce * _airControl);
		}
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > _maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * _maxSpeed, rigidbody2D.velocity.y);

		//flippng based on direction and movement
		if(_h > 0 && !_facingRight)
			Flip();
		else if(_h < 0 && _facingRight)
			Flip();

		//if jump was pressed
		if(_jump)
		{
			//and player can jump
			if(_jumper.CanJump())
			{
				//add upward force
				rigidbody2D.AddForce(new Vector2(0f, _jumper.Force), ForceMode2D.Impulse);
			}
			_jump = false;
		}

	}
	
	//flipping so player faces the correct position
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		//sprites should be separate from the collider to get rid of weird collision stuff (that I understand)
		Transform _sprites = transform.FindChild("Sprites");
		// Multiply the player's x local scale by -1.
		Vector3 _scale = _sprites.localScale;
		_scale.x *= -1;
		_sprites.localScale = _scale;
	}

	//updating the max speed; this will control how fast the player moves
	public void UpdateSpeed(float _maxSpeed)
	{
		this._maxSpeed = _maxSpeed;
	}

	//getter-setter for ghosting
	public bool Ghost
	{
		get{return _ghost;}
		set{
			_ghost = value;
			_ghostTimer = 0f;
		}
	}
}
