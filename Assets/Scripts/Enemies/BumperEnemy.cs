using UnityEngine;
using System.Collections;
using Assets.Scripts.Enemies;

namespace Assets.Scripts.Enemies
{
	public class BumperEnemy : Enemy
	{
		private int _dir = 1;
		public float _maxSpeed = 4f;

		public Transform _bumper;

		protected override void StartUp ()
		{
		}

		protected override void Run ()
		{
			int _mask = (1 << LayerMask.NameToLayer("player") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("enemy"));
			RaycastHit2D _hit = Physics2D.Raycast(_bumper.position, _bumper.forward, 0.5f, _mask);
			if(_hit.transform != null)
			{
				Transform _sprites = transform.Find("Sprites");
				_dir *= -1;
				_sprites.localScale = new Vector2(_dir, _sprites.localScale.y);
			}
		}

		/*
		void OnCollisionEnter2D(Collision2D col)
		{
			if(!col.transform.tag.Equals("ground"))
			{
				Transform _sprites = transform.FindChild("Sprites");
				_dir *= -1;
				_sprites.localScale = new Vector2(_dir, _sprites.localScale.y);
			}
		}
*/
		void FixedUpdate()
		{
			this.rigidbody2D.AddForce(new Vector2(_force * _dir, 0f));
			if (Mathf.Abs(rigidbody2D.velocity.x) > _maxSpeed)
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * _maxSpeed, rigidbody2D.velocity.y);
		}
	}
}