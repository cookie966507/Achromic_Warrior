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
			//this.gameObject.layer = LayerMask.NameToLayer("enemy");
		}
		
		protected override void Run()
		{
			changeInY = transform.position.y - lastYPos;
			//print ("Change: " + changeInY);
			lastYPos = transform.position.y;
			// Check if you need to jump to get to your position
			if ((target.position.y - transform.position.y) >= 2 && Mathf.Abs (changeInY) < changeThreshold) // && target.tag == "pat") 
			{
				jumpMode = true;
				//print ("Trueeeeeee: " + changeInY);
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
				if(target != null)
					print (target.tag + " " + target.localPosition);
			}
			/*
			else if((target.position.y - transform.position.y) < 2 && jumpMode == true)
			{
				jumpMode = false;
			}
			*/
			
			if(transform.position == target.position)
			{
				this.ExitGhost ();
				if(jumpMode)
				{
					// Need to calculate jump vector
					Vector2 vector = patrolPoints[currentPoint].position - target.position;
					vector.y *= jumpForceY;					/*
					vector.x = 0; //vector.x *= jumpForceX;
                    rigidbody2D.AddForce(vector); 
					vector.x *= jumpForceX;
                    GetComponent<Rigidbody2D>().AddForce(vector); 
                    */
					jumpMode = false;
					target = patrolPoints[currentPoint];
				}
				else
				{
					if(noWait > 0.25)
					{
						currentPoint++;
						if(currentPoint >= patLen)
							currentPoint = 0;
						target = patrolPoints[currentPoint];
						noWait = 0;
					}
					//if(noWait == 0)
						//this.ExitGhost();
					noWait += Time.deltaTime;
				}
				if(target != null)
					print (target.tag + " " + target.localPosition);
			}
			
			transform.position = Vector2.MoveTowards (transform.position, target.position, moveSpeed * Time.deltaTime);
			//Quaternion rotation = Quaternion.LookRotation(patrolPoints[currentPoint].position - transform.position); 
			if(transform.rotation.z <= 1 || transform.rotation.z >= -1)
			{
				Quaternion adjust = Quaternion.Euler (0, 0, 0);
				transform.rotation = Quaternion.Slerp(transform.rotation, adjust, Time.deltaTime * damping);	// Is shifting from rotation to player view gradually
			}
		}
	}
}
