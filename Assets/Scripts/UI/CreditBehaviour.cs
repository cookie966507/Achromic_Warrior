using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

/*
 * This class handles the behaviour for individual lines of text
 */
namespace Assets.Scripts.UI
{
	public class CreditBehaviour : MonoBehaviour
	{
		//reference to the handler
		private Credits _credits;

		void OnTriggerEnter2D(Collider2D col)
		{
			//appropriate trigger
			if(col.transform.name.Equals("DestroyCreditText"))
			{
				//if this is the last line in the credits, start over
				if(_credits.TextObjects.Count == 1) _credits.CreateCredits();

				//remove the object from the list and destroy it
				_credits.RemoveFirstObject();
				Destroy (this.gameObject);
			}
		}

		//assign reference to the handler
		public void AssignHandler(Credits _credits)
		{
			this._credits = _credits;
		}
	}
}
