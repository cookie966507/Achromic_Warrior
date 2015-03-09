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

		//delay for attacking
		private float _attackDelay = 0f;
		private float _attackTimer = 0f;
		//min and max delays
		private float _attackMinDelay = 10f;
		private float _attackMaxDelay = 20f;
		//how long to take to rise into the air
		private float _risingSpeed = 1f;
		private float _risingVel;
		//how much higher to rise
		private float _heightOffset = 5f;
		private float _targetHeight = 0;
		//booleans for tracking current state
		private bool _attacking = false;
		private bool _launch = false;
		private bool _launched = false;
		//how fast to move
		private float _attackForce = 20f;

		//for turning gravity on and off
		private float _gravity;

		//reference to the particle system when attacking
		private ParticleSystem _particles;
		private float _emissionRate = 40f;

		protected override void StartUp ()
		{
			//give a random delay for jumping
			_jumpDelay = Random.Range(_minDelay, _maxDelay);

			//give a random delay for attacking
			_attackDelay = Random.Range(_attackMinDelay, _attackMaxDelay);

			//store initial gravity
			_gravity = this.GetComponent<Rigidbody2D>().gravityScale;

			//find the particle system
			_particles = transform.Find("blade_particles").GetComponent<ParticleSystem>();
			_particles.startColor = CustomColor.GetColor(_color);
		}

		protected override void Run ()
		{

			if(Input.GetKeyDown(KeyCode.P)) this.Attack();
			//if not attacking
			if(!_attacking)
			{
				//update timers
				_jumpTimer += Time.deltaTime;
				_attackTimer += Time.deltaTime;

				//if it's time to jump
				if(_jumpTimer >= _jumpDelay)
				{
					//reset timer
					_jumpTimer = 0f;
					//enter ghosting mode
					EnterGhost();
					//zero out the y so it doesn't look weird
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
					//jump
					GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
					//exit ghosting after 2 seconds
					Invoke("ExitGhost", 2f);
					//give a different random delay
					_jumpDelay = Random.Range(_minDelay, _maxDelay);
				}

				//if it is time to attack
				if(_attackTimer >= _attackDelay)
				{
					//attack
					this.Attack();
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

			//if we are rising into the air but have not launched the attack yet
			if(_attacking && !_launched)
			{
				//do not keep rising if already launched attack
				if(_launch) return;

				//flip based on the player position
				if(_player.transform.position.x > this.transform.position.x)
				{
					if(_dir < 0) this.Flip();
				}
				else if(_player.transform.position.x < this.transform.position.x)
				{
					if(_dir > 0) this.Flip();
				}

				//get the current height
				float _y = this.transform.position.y;
				//if we are not at the current height yet
				if(_y < _targetHeight - 0.3f)
				{
					//smooth damp to the height
					_y = Mathf.SmoothDamp(_y, _targetHeight, ref _risingVel, _risingSpeed);
					this.transform.position = new Vector2(this.transform.position.x, _y);
				}
				//if we are at the desired height
				else
				{
					//launch body into the player
					_launch = true;
				}
			}
		}

		void FixedUpdate()
		{
			//if not attacking
			if(!_attacking)
            {
                Rigidbody2D rgb2d = GetComponent<Rigidbody2D>();
				//move in a direction
                rgb2d.AddForce(new Vector2(_force * _dir, 0f));
				//limit speed
                if (Mathf.Abs(rgb2d.velocity.x) > _maxSpeed)
                    rgb2d.velocity = new Vector2(Mathf.Sign(rgb2d.velocity.x) * _maxSpeed, rgb2d.velocity.y);
			}

			//if it is time to launch into the player
			if(_launch)
			{
				//set states properly so we don't keep launching
				_launch = false;
				_launched = true;
				//remove kinematic so we can move
				this.GetComponent<Rigidbody2D>().isKinematic = false;
				//find the direction to travel to the player
				Vector3 _rayToPlayer = Vector3.zero;
				_rayToPlayer = _player.transform.position - this.transform.position;
				//fly into the player
				GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.Normalize(_rayToPlayer) * _attackForce, ForceMode2D.Impulse);
			}
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			if(col.gameObject.Equals(_player))
			{
				//stop attacking
				StopAttack();
				//flip direction if necessary
				if(_dir > 0 && _player.transform.position.x > this.transform.position.x) Flip();
				else if(_dir < 0 && _player.transform.position.x < this.transform.position.x) Flip();
			}
			else if(col.transform.tag.Equals("ground") | col.transform.tag.Equals("wall"))
			{
				//stop attacking if the player was not hit
				StopAttack();
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

		//setting up attaking the player
		private void Attack()
		{
			//set boolean
			_attacking = true;
			//update timers
			_attackTimer = 0f;
			_jumpTimer = 0;

			//turn on attacking particles
			_particles.emissionRate = _emissionRate;

			//remove gravity for flying into a straight line
			this.GetComponent<Rigidbody2D>().gravityScale = 0;
			//set to kinematic so other enemies do not knock this enemy away
			this.GetComponent<Rigidbody2D>().isKinematic = true;
			_targetHeight = this.transform.position.y + _heightOffset;

			//go through platforms
			this.EnterGhost();

			//stop after a few seconds just in case
			Invoke("StopAttack", 3f);
		}

		//set everything back after attack is complete
		private void StopAttack()
		{
			//set boolean
			_attacking = false;
			_launched = false;

			//turn off attacking particles
			_particles.emissionRate = 0f;

			//reset the new attack time
			_attackDelay = Random.Range(_attackMinDelay, _attackMaxDelay);

			//return gravity to normal
			this.GetComponent<Rigidbody2D>().gravityScale = _gravity;

			//remove kinematic to move again
			this.GetComponent<Rigidbody2D>().isKinematic = false;

			//stop going through platforms
			this.ExitGhost();
		}

		//Enemy ghost exited by leaving platform trigger
		//overrided because Enemy ExitGhost() stopped attack too early
		public override void ExitGhost()
		{
			//if not attacking
			if(!_attacking)
			{
				//exit ghost
				if (this.gameObject.layer != ENEMY)
					this.gameObject.layer = ENEMY;
				_ghostTimer = _ghostDelay+1;
			}
		}
	}
}