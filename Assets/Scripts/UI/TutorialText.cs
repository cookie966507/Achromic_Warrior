using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI
{
	public class TutorialText : MonoBehaviour
	{
		public GameObject _trigger;
		public GameObject _panel;

		void Awake()
		{
			_trigger.SetActive(false);
			_panel.SetActive(false);
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			_trigger.SetActive(true);
			_panel.SetActive(true);
		}

		void OnTriggerExit2D(Collider2D col)
		{
			_trigger.SetActive(false);
			_panel.SetActive(false);
		}
	}
}
