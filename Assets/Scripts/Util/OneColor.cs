using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * This class is for assigning one color to an object if it does not
 * have one assigned elsewhere
 */
namespace Assets.Scripts.Util
{
	public class OneColor : MonoBehaviour
	{
		public ColorElement _color = ColorElement.White;
		public bool _ui;
		public bool onlyChildren;

		void Awake()
		{
			if(_ui) this.GetComponent<Image>().color = CustomColor.GetColor(_color);
			else
			{
				if(onlyChildren)
				{
					Renderer[] _r = this.GetComponentsInChildren<Renderer>();
					for(int i = 0; i < _r.Length; i++)
					{
						_r[i].material.color = CustomColor.GetColor(_color);
					}
				}
				else this.renderer.material.color = CustomColor.GetColor(_color);
			}
		}
	}

}