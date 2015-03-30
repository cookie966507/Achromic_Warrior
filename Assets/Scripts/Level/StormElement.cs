using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;

namespace Assets.Scripts.Level
{
	public class StormElement : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float _flashAmount = 0f;

		private Material _flashMat;

		private bool _flash;
		private bool _reverse;

		private float _vel;
		private float _speed = 0.1f;

		public StormController _controller;

		void Awake()
		{
			_flashMat = this.GetComponent<Renderer>().material;
		}

		void Start()
		{
			if(_controller == null) _controller = GameObject.Find("StormController").GetComponent<StormController>();
			_controller.AddToElementsList(this);
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.P))
			{
				Flash ();
			}
			if(_flash)
			{
				_flashMat.SetFloat("_FlashAmount", Mathf.SmoothDamp(_flashMat.GetFloat("_FlashAmount"), _flashAmount, ref _vel, _speed));
				if(_flashMat.GetFloat("_FlashAmount") >= _flashAmount - 0.01)
				{
					_reverse = true;
					_flash = false;
				}
			}
			else if(_reverse)
			{
				_flashMat.SetFloat("_FlashAmount", Mathf.SmoothDamp(_flashMat.GetFloat("_FlashAmount"), 0f, ref _vel, _speed * 5f));
				if(_flashMat.GetFloat("_FlashAmount") == 0f) _reverse = false;
			}
		}

		public void Flash()
		{
			_flash = true;
			_reverse = false;
		}
	}
}