using UnityEngine;
using System.Collections;

/*
 * Class controlling the player's camera
 */
namespace Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        //target to follow
        private Transform _target;

        //speed of the camera
        private float _speed = 0.1f;

        //refs for smooth damp
        private float _xVel;
        private float _yVel;

        //target position
        private float _x;
        private float _y;

        //current positions
        private float _currX;
        private float _currY;

        void Start()
        {
            //find target
            _target = GameObject.Find("Player Camera Target").transform;
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

        }

        public Transform Target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}
