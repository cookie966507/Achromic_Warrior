using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu
{
    class MenuItem : MonoBehaviour
    {
        public MenuItem[] children;
        public Transform[] holdPositions;
        public ButtonHandler handle;

        [SerializeField]
        private RectTransform canvasPos;

        internal int position = 3;

        internal Node[] getChildren(Node parent)
        {
            Node[] arr=new Node[children.Length];
            for(int i=0;i<children.Length;i++)
            {
                arr[i] = new Node();
                arr[i].item = children[i];
                arr[i].parent = parent;
                if (children.Length!=0)
                    arr[i].children = children[i].getChildren(arr[i]);
                else
                    arr[i].children = null;
            }
            return arr;
                
        }

        internal void wake(bool move, bool left)
        {
            handle.wake();
            if (move)
                moveCanvas(left);
        }

        internal void sleep(bool move, bool left)
        {
            handle.sleep();
            if (move)
                moveCanvas(left);
        }

        private void moveCanvas(bool left)
        {
            if (left)
                position--;
            else
                position++;
            if (position == 1)
                handle.setLeft();
            else if (position == 2)
                handle.setRight();
        }

        void Update()
        {
            if (Mathf.Abs(canvasPos.position.x - holdPositions[position].position.x) > .1f)
                canvasPos.position = Vector3.Lerp(canvasPos.position, holdPositions[position].position, Time.deltaTime);
        }
    }
}
