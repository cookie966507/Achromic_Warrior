using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public class ColorPickup : MonoBehaviour
{

	private float _amount;
	private ColorElement _color;


	void Start ()
	{
		if(_color.Equals(ColorElement.Red)) renderer.material.color = Color.red;
		else if(_color.Equals(ColorElement.Green)) renderer.material.color = Color.green;
		else if(_color.Equals(ColorElement.Blue)) renderer.material.color = Color.blue;

		else Debug.LogError("Colors for orbs should just be rgb");

		Destroy (this.gameObject, 10f);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		/*    Make Player check for this
		if(col.transform.tag.Equals("Player"))
		{
			Destroy(gameObject);
		}
		*/
		if(col.transform.tag.Equals("ground"))
		{
			if(rigidbody2D.velocity.y <= 0)
			{
				rigidbody2D.velocity = Vector2.zero;
				Destroy (rigidbody2D);
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
