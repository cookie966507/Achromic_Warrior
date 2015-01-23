using UnityEngine;
using System.Collections;

/*
 * Follows a target exactly
 */
public class FollowTarget : MonoBehaviour
{
	public Transform _target;

	void Update ()
	{
		this.transform.position = _target.position;
	}
}
