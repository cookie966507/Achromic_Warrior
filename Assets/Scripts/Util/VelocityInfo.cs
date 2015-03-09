using UnityEngine;
using System.Collections;

/*
 * Information about an object's rigidbody
 * Used for pausing and unpausing
 */
namespace Assets.Scripts.Util
{
public class VelocityInfo : MonoBehaviour
{
        //is rigidbody kinematic?
        public bool _kinematic = false;

        //is rigidbody fixed angle?
        public bool _fixedAngle = false;

        //is rigidbody paused?
        private bool _paused = false;

        //reference to old velocity
        private Vector2 _vel = Vector2.zero;
        //reference to spinning velocity
        private float _angVel = 0f;

        //pause rigidbody
        public void PauseMotion()
        {
            if (!_kinematic && !_fixedAngle)
            {
                //save velocity
                _vel = this.GetComponent<Rigidbody2D>().velocity;
                //save angular velocity
                _angVel = this.GetComponent<Rigidbody2D>().angularVelocity;

                //set fixed angle
                this.GetComponent<Rigidbody2D>().fixedAngle = true;
                //set to kinematic to pause
                this.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else if (!_kinematic)
            {
                //save velocity
                _vel = this.GetComponent<Rigidbody2D>().velocity;
                //set to kinematic to pause
                this.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            //pause
            _paused = true;
        }

        public void UnpauseMotion()
        {
            if (!_kinematic && !_fixedAngle)
            {
                //set to not kinematic to unpause
                this.GetComponent<Rigidbody2D>().isKinematic = false;

                //set to not fixed angle
                this.GetComponent<Rigidbody2D>().fixedAngle = false;
                //reapply angular velocity
                this.GetComponent<Rigidbody2D>().angularVelocity = _angVel;
                //reapply velocity
                this.GetComponent<Rigidbody2D>().velocity = _vel;

                //reset reference
                _angVel = 0f;
                //reset reference
                _vel = Vector2.zero;
            }
            else if (!_kinematic)
            {
                //set to not kinematic to unpause
                this.GetComponent<Rigidbody2D>().isKinematic = false;
                //reapply velocity
                this.GetComponent<Rigidbody2D>().velocity = _vel;
                //reset reference
                _vel = Vector2.zero;
            }

            //unpause
            _paused = false;
        }

        public bool Paused
        {
            get { return _paused; }
        }

        public Vector2 Vel
        {
            get { return _vel; }
        }

        public float AngVel
        {
            get { return _angVel; }
        }
    }
}

