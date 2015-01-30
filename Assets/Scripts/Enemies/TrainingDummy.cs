using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Training dummy class. An extension of enemy
 */
namespace Assets.Scripts.Enemies
{
    public class TrainingDummy : Enemy
    {
        protected override void StartUp()
        {
        }

        protected override void Run()
        {
            //specific to training dummys and the hingejoint2D components attached to them
            if (_player.transform.position.x - this.transform.position.x < 0 && this.transform.localScale.x > 0)
            {
                //set the dummy direction properly
                Vector3 _scale = transform.localScale;
                _scale.x *= -1;
                transform.localScale = _scale;

                //set the limits nd motor of the joint to be reversed
                JointAngleLimits2D limits = GetComponent<HingeJoint2D>().limits;
                limits.max = 75;
                limits.min = 0;
                GetComponent<HingeJoint2D>().limits = limits;
                JointMotor2D motor = GetComponent<HingeJoint2D>().motor;
                motor.motorSpeed = -50;
                GetComponent<HingeJoint2D>().motor = motor;
            }
            else if (_player.transform.position.x - this.transform.position.x > 0 && this.transform.localScale.x < 0)
            {
                //set the dummy direction properly
                Vector3 _scale = transform.localScale;
                _scale.x *= -1;
                transform.localScale = _scale;

                //set the limits nd motor of the joint to be reversed
                JointAngleLimits2D limits = GetComponent<HingeJoint2D>().limits;
                limits.max = 0;
                limits.min = -75;
                GetComponent<HingeJoint2D>().limits = limits;
                JointMotor2D motor = GetComponent<HingeJoint2D>().motor;
                motor.motorSpeed = 50;
                GetComponent<HingeJoint2D>().motor = motor;
            }
        }
    }
}
