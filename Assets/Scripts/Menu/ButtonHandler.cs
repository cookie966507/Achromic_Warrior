using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu
{
    abstract class ButtonHandler : MonoBehaviour
    {
        protected bool isLeft;
        public abstract void wake();
        public abstract void sleep();

        public void setLeft()
        {
            isLeft = true;
        }

        public void setRight()
        {
            isLeft = false;
        }
    }
}
