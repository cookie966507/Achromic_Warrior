using UnityEngine;
using System.Collections;
using Assets.Scripts.Enemies;
/*
 * Enemy that bumps around the level randomly
 */
namespace Assets.Scripts.Enemies
{
	public class BumperEnemy : Enemy
	{
		//direction facing
		private int _dir = 1;
		//speed
		public float _maxSpeed = 4f;

		//for detecting when to turn around
		public Transform _bumper;

		//how high to jump
		public float _jumpForce = 10f;
		//delay for jumping
		private float _jumpDelay;
		private float _jumpTimer = 0f;
		//min and max delays
		private float _minDelay = 2f;
		private float _maxDelay = 4f;

		protected override void StartUp ()
		{
			//give a random delay
			_jumpDelay = Random.Range(_minDelay, _maxDelay);
		}

		protected override void Run ()
		{
			//update timer
			_jumpTimer += Time.deltaTime;
			//if it's time to jump
			if(_jumpTimer >= _jumpDelay)
			{
				//reset timer
				_jumpTimer = 0f;
				//enter ghosting mode
				EnterGhost();
				//zero out the y so it doesn't look weird
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
				//jump
				rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
				//exit ghosting after 2 seconds
				Invoke("ExitGhost", 2f);
				//give a different random delay
				_jumpDelay = Random.Range(_minDelay, _maxDelay);

			}

			//mask for layers to collide with and turn around
			int _mask = (1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("enemy"));
			//cast a ray to see if should turn around
			RaycastHit2D _hit = Physics2D.Raycast(_bumper.position, _bumper.forward, 0.5f, _mask);
			if(_hit.transform != null)
			{
				Flip();
			}
		}

		void FixedUpdate()
		{
			//move in a direction
			this.rigidbody2D.AddForce(new Vector2(_force * _dir, 0f));
			//limit speed
			if (Mathf.Abs(rigidbody2D.velocity.x) > _maxSpeed)
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * _maxSpeed, rigidbody2D.velocity.y);
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.Equals(_player))
			{
				if(_dir > 0 && _player.transform.position.x > this.transform.position.x) Flip();
				else if(_dir < 0 && _player.transform.position.x < this.transform.position.x) Flip();
			}
		}

		public void Flip()
		{
			//turn sprites around, not actual gameobject
			Transform _sprites = transform.Find("Sprites");
			//change dir
			_dir *= -1;
			_sprites.localScale = new Vector2(_dir, _sprites.localScale.y);
		}
	}
}