using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;

/*
 * Meter to display each r, g, b of the players collectec colors
 */
namespace Assets.Scripts.UI
{
    public class ColorMeter : MonoBehaviour
    {
		//color of the bar
		public ColorElement _color;
        //size of the bar
        private float _size = 1f;

        //reference to the color wheel
        private ColorWheel _wheel;

		//ui sphere
		public Image _sphere; 

		void Start ()
		{
			//find wheel
			_wheel = GameObject.Find("ColorWheel").GetComponent<ColorWheel>();

			//init scale at 0
			this.transform.localScale = new Vector3(0f, 1f, 1f);

			//set color
			this.GetComponent<Image>().color = CustomColor.GetColor(_color);
		}

		//update the meter with some amount
		public void UpdateMeter(float _amount)
		{
			//keep meter within bounds
			_amount = Mathf.Clamp(_amount, 0f, _size);
			this.transform.localScale = new Vector3(_amount, 1f, 1f);

			//update tabs that should be revealed or hidden
			_wheel.UpdateTabs();
		}

		//Update the UI sphere
		public void UpdateSphereColor(ColorElement _color)
		{
			_sphere.color = CustomColor.GetColor(_color);
		}
    }
}
