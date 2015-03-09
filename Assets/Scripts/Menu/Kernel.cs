using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu
{
    class Kernel : MonoBehaviour
    {
        public MenuItem item;

        private static Node root;
        private static Node left;
        private static Node right;

        void Awake()
        {
            root = new Node();
            root.item = item;
            root.parent = null;
            root.children = item.getChildren(root);
            right = root;
            left = null;
            root.item.wake(false,false);
            root.item.position = 2;
            root.item.handle.setRight();
        }

        internal static void transition(bool isdown, bool isleft, int child)
        {
            Node next=null;
            if(isdown)
            {
                if (isleft)
                {
                    if(left==null)
                    {
                        Debug.Log("Attack!!!! left shouldn't be null!!!!");
                        return;
                    }
                    if (child >= 0 && child < left.children.Length)
                        next = left.children[child];
                    else
                    {
                        Debug.Log("Menu Item doesn't exist in children");
                        return;
                    }
                    //left stays
                    left.item.sleep(false, false);
                    //right exits right
                    right.item.sleep(true, false);
                    //next enters from right
                    next.item.wake(true, true);
                    right = next;
                }
                else
                {
                    if (child >= 0 && child < right.children.Length)
                        next = right.children[child];
                    else
                    {
                        Debug.Log("Menu Item doesn't exist in children");
                        return;
                    }
                    if (left != null)
                    {
                        //left exits left
                        left.item.sleep(true, true);
                    }
                    //right moves left
                    right.item.sleep(true, true);
                    //next enters from right
                    next.item.wake(true, true);
                    left = right;
                    right = next;
                }
            }
            else 
            {
                if (isleft)
                {
                    if(left==null)
                    {
                        Debug.Log("AAAAAAAAAAAAAAAAAAA");
                        return;
                    }
                    if (left.parent == null)
                    {
                        //at root
                        //right exits right
                        right.item.sleep(true, false);
                        //left moves right
                        left.item.wake(true, false);
                        right = left;
                        left = null;
                    }
                    else
                    {
                        next = left.parent;
                        //right exits right
                        right.item.sleep(true, false);
                        //left moves right
                        left.item.sleep(true, false);
                        //next enters from left
                        next.item.wake(true, false);
                        right = left;
                        left = next;
                    }
                }
                else
                {
                    if (right.parent == null)
                    {
                        Debug.Log("Logic Erro tree incorrect");
                        return;
                    }
                    if (left.parent == null)
                    {
                        //at root
                        //right exits right
                        right.item.sleep(true, false);
                        //left moves right
                        left.item.wake(true, false);
                        right = left;
                        left = null;
                    }
                    else
                    {
                        next = left.parent;
                        //right exits right
                        right.item.sleep(true, false);
                        //left moves right
                        left.item.wake(true, false);
                        //next enters from left
                        next.item.sleep(true, false);
                        right = left;
                        left = next;
                    }
                }
            }
        }

        internal static void interrupt(bool isleft)
        {
            //no movement
            if(isleft)
            {
                left.item.wake(false, false);
                right.item.sleep(false, false);
            }
            else
            {
                left.item.sleep(false, false);
                right.item.wake(false, false);
            }
        }

        internal static void disalble()
        {
            if(left!=null)
                left.item.gameObject.GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
            if (right != null)
                right.item.gameObject.GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
        }

        internal static void enalble()
        {
            if (left != null)
                left.item.gameObject.GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = true;
            if (right != null)
                right.item.gameObject.GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = true;
        }
    }

    class Node
    {
        public MenuItem item;
        public Node parent;
        public Node[] children;
    }
}
