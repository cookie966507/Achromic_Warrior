using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Used for fading a sponsor texture
 */
namespace Assets.Scripts.UI
{
	public class SponsorFade : MonoBehaviour {
		public Image[] _images;
		//public float fadeTime;
		private float _time;
		private float _alpha = 0;
		public float _alphaStep;
		private int _imageIndex = 0;
		private Color _color;

		void Start()
		{
			_color = _images[_imageIndex].color;
		}

		void Update(){
			if(CustomInput.AcceptFreshPressDeleteOnRead){
				NextImage();
			}
			_alpha += _alphaStep;
			_alpha = Mathf.Clamp(_alpha, 0f, 1f);
			if(_alpha == 1f || _alpha == 0f)
			{
				_alphaStep = -_alphaStep;
				if(_alpha == 0)
				{
					NextImage();
				}
			}
			if(_imageIndex < _images.Length)
				_images[_imageIndex].color = new Color(_color.r, _color.g, _color.b, _alpha);
		}

		private void NextImage(){
			_time = 0;
			_images[_imageIndex].color = new Color(_color.r, _color.g, _color.b, 0);
			_imageIndex++;
			if(_imageIndex < _images.Length)
			{
				_color = _images[_imageIndex].color;
				_images[_imageIndex].color = new Color(_color.r, _color.g, _color.b, 0);
				_alpha = 0;
				if(_alphaStep < 0) _alphaStep =-_alphaStep;
			}
			else Data.GameManager.GotoLevel("Menu_Web");
		}
	}
}