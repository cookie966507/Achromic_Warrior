using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu
{
    abstract class ButtonHandler : MonoBehaviour
    {
        public abstract void wake();
        public abstract void sleep();
        public abstract void setLeft();
        public abstract void setRight();
    }
}
