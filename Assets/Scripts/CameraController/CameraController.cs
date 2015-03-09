using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

/*
 * Class controlling the player's camera
 */
namespace Assets.Scripts.CameraController
{
    public class CameraController : MonoBehaviour
    {
        //target to follow
        public Transform _target;

		//reference to player
		private Transform _player;

        //speed of the camera
        private float _speed = 0.1f;
		private float _fovSpeed = 0.5f;

        //refs for smooth damp
        private float _xVel;
        private float _yVel;

        //target position
        private float _x;
        private float _y;

        //current positions
        private float _currX;
        private float _currY;

		//for getting the camera fov
		private float _distance = 0f;
		private float _angleVel;
		private float _angle = 10f;
		private float _minAngle = 10f;
		private float _maxAngle = 14f;
		private float _ceilingHeight = 7f;

        void Start()
        {
            //find player
            _player = GameObject.Find("player").transform;
        }

        void Update()
        {
            //get target position
            _x = _target.position.x;
            _y = _target.position.y;

            //update current values
            _currX = Mathf.SmoothDamp(_currX, _x, ref _xVel, _speed);
            _currY = Mathf.SmoothDamp(_currY, _y, ref _yVel, _speed);

            //apply change
            this.transform.position = new Vector3(_currX, _currY, this.transform.position.z);

			_distance = Mathf.Abs(_player.position.y - this.transform.position.y);
			if(_distance > _ceilingHeight) _angle = Mathf.SmoothDamp(_angle, _maxAngle, ref _angleVel, _fovSpeed);
			else _angle = Mathf.SmoothDamp(_angle, _minAngle, ref _angleVel, _fovSpeed);
			GetComponent<Camera>().orthographicSize = _angle;


        }

        public Transform Target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}
