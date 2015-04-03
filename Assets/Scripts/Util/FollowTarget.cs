using UnityEngine;
using System.Collections;

/*
 * Follows a target exactly
 */
namespace Assets.Scripts.Util
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform _target;

        void Update()
        {
			if(_target)
            this.transform.position = _target.position;
        }
    }
}
