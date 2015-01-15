using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
	public Transform _target;

	void Update ()
	{
		this.transform.position = _target.position;
	}
}
