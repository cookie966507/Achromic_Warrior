using UnityEngine;
using System.Collections;

/*
 * Backup in case something needs to be destroyed on it own
 */
namespace Assets.Scripts.Util
{
    public class Destroyer : MonoBehaviour
    {
        //wait or not to destroy
        public bool _destroyDelay = false;
        //time to wait to destroy
        public float _lifeTime = 3f;

        void Awake()
        {
            if (_destroyDelay)
            {
                Destroy(this.gameObject, _lifeTime);
            }
        }

        public void DestroyImmediate()
        {
            Destroy(this.gameObject);
        }
    }
}
