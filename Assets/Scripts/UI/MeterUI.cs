using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Enums;

namespace Assets.Scripts.UI
{
	/*
	 * This class will be for looks
	 * It will update the meter's color when appropriate
	 */
	public class MeterUI : MonoBehaviour
	{
		//color of the frame
		public ColorElement _color = ColorElement.White;

		//alt color of the frame
		private ColorElement _alt = ColorElement.Black;

		//reference to meters
		private ColorMeter _r;
		private ColorMeter _g;
		private ColorMeter _b;

		//time to lerp if flashing
		private bool _full = false;

		private bool _increase = true;
		private float _timer = 0f;

		//stopping the flashing
		public bool _achromic = false;

		void Start ()
		{
			//find meters
			_r = GameObject.Find("R").GetComponent<ColorMeter>();
			_g = GameObject.Find("G").GetComponent<ColorMeter>();
			_b = GameObject.Find("B").GetComponent<ColorMeter>();
		}
		
		void Update ()
		{
			//meters are full so we can flash
			if(_r.transform.localScale.x == 1 && _g.transform.localScale.x == 1 && _b.transform.localScale.x == 1 ) Full = true;
			else Full = false;

			//if we can flash
			if(_full && !_achromic)
			{
				//going to black
				if(_increase)
				{
					_timer += Time.deltaTime;
					this.GetComponent<Image>().color = Color.Lerp(CustomColor.GetColor(_color), CustomColor.GetColor(_alt), _timer * 1.5f);

					if(_timer * 1.5f >= 1) _increase = !_increase;
				}
				//then back to white
				else{
					_timer -= Time.deltaTime;
					this.GetComponent<Image>().color = Color.Lerp(CustomColor.GetColor(_color), CustomColor.GetColor(_alt), _timer * 1.5f);
					if(_timer * 1.5 <= 0) _increase = !_increase;
				}
			}

			//stop flashing if activated
			if(_achromic)
			{
				if(_r.transform.localScale.x == 0) Full = !_full;
			}
		}

		//color of the frame
		public ColorElement FrameColor
		{
			get{return _color;}
			set
			{
				_color = value;
				this.GetComponent<Image>().color = CustomColor.GetColor(_color);
			}
		}

		//flashing bool
		public bool Full
		{
			get{return _full;}
			set
			{
				//reset everything
				_full = value;
				if(!_full)
				{
					_color = ColorElement.White;
					this.GetComponent<Image>().color = CustomColor.GetColor(_color);
					_achromic = false;
					_timer = 0f;
					_increase = true;
				}
			}
		}
	}
}