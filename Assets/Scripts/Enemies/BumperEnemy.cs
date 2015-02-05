using UnityEngine;
using System.Collections;
using Assets.Scripts.Enemies;

namespace Assets.Scripts.Enemies
{
	public class BumperEnemy : Enemy
	{
		private int _dir = 1;
		private float _moveForce = 100f;
		public float _maxSpeed = 4f;

		protected override void StartUp ()
		{

		}

		protected override void Run ()
		{

		}

		void OnCollisionEnter2D(Collision2D col)
		{
			if(!col.transform.tag.Equals("ground"))
			{
				Transform _sprites = transform.FindChild("Sprites");
				_dir *= -1;
				_sprites.localScale = new Vector2(_dir, _sprites.localScale.y);
			}
		}

		void FixedUpdate()
		{
			this.rigidbody2D.AddForce(new Vector2(_force * _dir, 0f));
			if (Mathf.Abs(rigidbody2D.velocity.x) > _maxSpeed)
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * _maxSpeed, rigidbody2D.velocity.y);
		}
	}
}