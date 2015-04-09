using UnityEngine;
using System.Collections;



namespace Assets.Scripts.Enemies
{
	public class Patrol : Enemy
	{
		public Transform[] patrolPoints;
		public Transform[] jumpPoints;
		public float changeThreshold = 0.03f;
		public float moveSpeed;
		private Transform player;
		private Transform target;
		private int currentPoint;
		private float damping = 3.0f;
		private float jumpForceX = 30f;
		private float jumpForceY = 100f;
		private bool jumpMode = false;
		private int patLen;
		private int jumpLen;
		private float changeInY = 0.0f; 
		private float lastYPos = 0.0f;
		private float noWait = 0;


		protected override void StartUp()
		{
			//transform.position = patrolPoints [0].position;
			currentPoint = 0;
			target = patrolPoints [currentPoint];
			patLen = patrolPoints.Length;
			jumpLen = jumpPoints.Length;
			player = FindObjectOfType<Player.PlayerController> ().gameObject.transform;
			//this.gameObject.layer = LayerMask.NameToLayer("enemy");
		}
		
		protected override void Run()
		{
			bool value = PatrolAround ();
			//print ("Current point: " + currentPoint);
		}

		protected bool PatrolAround()
		{
			changeInY = transform.position.y - lastYPos;
			lastYPos = transform.position.y;
			// Check if you need to jump to get to your position
			if ((target.position.y - transform.position.y) >= 2 && Mathf.Abs (changeInY) < changeThreshold) // && target.tag == "pat") 
			{
				jumpMode = true;
				Transform current = jumpPoints[0];
				for(int i = 0; i < jumpLen; i++)
				{
					// Find the shortest point from the next patrolto jump to for jump points
					
					if(Mathf.Abs (jumpPoints[i].position.y - transform.position.y) < 2)
					{
						// If no jump point is found, set the current one
						if(!current)
						{
							current = jumpPoints[i];
						}
						
						// If a jump point is found, calculate the one with the closest x coord
						else if(Mathf.Abs(jumpPoints[i].position.x - target.position.x) < 
						        Mathf.Abs(current.position.x - target.position.x))
						{
							current = jumpPoints[i];
						}
					}
				}
				target = current;
			}
			
			if(transform.position == target.position)
			{
				this.ExitGhost ();
				if(jumpMode)
				{
					// Need to calculate jump vector
					Vector2 vector = patrolPoints[currentPoint].position - target.position;
					vector.y *= jumpForceY;					
					vector.x = 0; 
					
					vector.x *= jumpForceX;
					GetComponent<Rigidbody2D>().AddForce(vector); 
					EnterGhost();
					jumpMode = false;
					target = patrolPoints[currentPoint];
				}
				else
				{
					if(noWait > 0.25)
					{
						currentPoint = Random.Range (0, patLen);
						target = patrolPoints[currentPoint];
						noWait = 0;
					}
					noWait += Time.deltaTime;
				}
			}
			
			transform.position = Vector2.MoveTowards (transform.position, target.position, moveSpeed * Time.deltaTime);
			if(transform.rotation.z <= 1 || transform.rotation.z >= -1)
			{
				Quaternion adjust = Quaternion.Euler (0, 0, 0);
				transform.rotation = Quaternion.Slerp(transform.rotation, adjust, Time.deltaTime * damping);	// Is shifting from rotation to player view gradually
			}

			// Return true if the player is close, false if he is not within range
			if(Vector2.Distance (player.position, transform.position) < 3.0)
			{
				return true;
			}
			else
			{
				return false;
			}

		}
	}
}
