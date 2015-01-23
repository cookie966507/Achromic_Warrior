using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Class for picking up colors
 */
public class ColorPickup : MonoBehaviour
{
	//amount to add to color
	private float _amount = 50;
	//what kind of color pickup
	private ColorElement _color;


	void Start ()
	{
		//color the orb based on the assign color; stick with rgb for now
		if(_color.Equals(ColorElement.Red)) renderer.material.color = Color.red;
		else if(_color.Equals(ColorElement.Green)) renderer.material.color = Color.green;
		else if(_color.Equals(ColorElement.Blue)) renderer.material.color = Color.blue;

		else Debug.LogError("Colors for orbs should just be rgb");
	}

	//pickup collides with something
	void OnTriggerEnter2D(Collider2D col)
	{
		//if the pickup hits the ground
		if(col.transform.tag.Equals("ground"))
		{
			//if falling down
			if(rigidbody2D.velocity.y <= 0)
			{
				//stop velocity
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.gravityScale = 0;
			}
		}
	}

	public float Amount
	{
		get {return _amount;}
		set {_amount = value;}
	}

	public ColorElement ColorType
	{
		get {return _color;}
		set{_color = value;}
	}
}
