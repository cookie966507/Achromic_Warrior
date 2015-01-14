using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{
	public bool _destroyDelay = false;
	public float _lifeTime = 3f;

	void Awake()
	{
		if(_destroyDelay)
		{
			Destroy (this.gameObject, _lifeTime);
		}
	}

	public void DestroyImmediate()
	{
		Destroy(this.gameObject);
	}
}
