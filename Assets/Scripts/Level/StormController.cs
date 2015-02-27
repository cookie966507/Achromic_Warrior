using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Level;

namespace Assets.Scripts.Level
{
	public class StormController : MonoBehaviour
	{
		private float _lightningDelay;
		private float _time = 0;

		private const float MINTIME = 3f;
		private const float MAXTIME = 10f;

		private const float DOUBLECHANCE = 0.15f;
		private const float DOUBLEDELAY = 0.75f;

		private List<StormElement> _elements;

		void Awake()
		{
			_elements = new List<StormElement>();

			this.SetLightningDelay();
		}
		
		void Update ()
		{
			_time += Time.deltaTime;
			if(_time > _lightningDelay)
			{
				_time = 0;
				this.SetLightningDelay();
				if(_lightningDelay < MINTIME + MINTIME * DOUBLECHANCE) _lightningDelay = DOUBLEDELAY;

				for(int i = 0; i < _elements.Count; i++)
				{
					_elements[i].Flash();
				}
			}
		}

		private void SetLightningDelay()
		{
			_lightningDelay = Random.Range(MINTIME, MAXTIME);
		}

		public void AddToElementsList(StormElement _element)
		{
			this._elements.Add(_element);
		}
	}
}
