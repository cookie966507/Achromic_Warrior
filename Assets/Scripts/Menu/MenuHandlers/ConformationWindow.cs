using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class ConformationWindow : MonoBehaviour
    {
        public GameObject[] cursors;

        public delegate void func(bool yesNo);
        private static func function;
        private static Canvas win;

        private static ConformationStateMachine machine = new ConformationStateMachine();
        private delegate void state();
        private state[] doState;
        private ConformationStateMachine.confirm currState;

        public static void getConformation(func function)
        {
            ConformationWindow.function = function;
            Kernel.disalble();
            win.enabled=true;
            machine.goTo(ConformationStateMachine.confirm.yes);
        }

        void Start()
        {
            win = this.gameObject.GetComponent<Canvas>();
            doState = new state[] { Sleep, Yes, No };
            win.enabled = false;
        }

        void Update()
        {
            ConformationStateMachine.confirm prevState = currState;
            currState = machine.update();
            if (prevState != currState)
            {
                foreach (GameObject g in cursors)
                    g.SetActive(false);
                int cursor = (int)currState - 1;
                if (cursor >= 0)
                    cursors[cursor].SetActive(true);
            }
            doState[(int)currState]();
        }
        private static void Sleep()
        {
            
        }

        private static void Yes()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doAnswer(true);
        }
        private static void No()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doAnswer(false);
        }
        private static void doAnswer(bool yesNo)
        {
            function(yesNo);
            Kernel.enalble();
            win.enabled = false;
            machine.goTo(ConformationStateMachine.confirm.sleep);
        }
        
        public void YesClick()
        {
            machine.goTo(ConformationStateMachine.confirm.yes);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)ConformationStateMachine.confirm.yes-1].SetActive(true);
            doAnswer(true);
        }

        public void NoClick()
        {
            machine.goTo(ConformationStateMachine.confirm.no);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)ConformationStateMachine.confirm.yes-1].SetActive(true);
            doAnswer(false);
        }
    }
    class ConformationStateMachine
    {
        internal enum confirm { sleep, yes, no };
        private delegate confirm machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private confirm currState;

        internal ConformationStateMachine()
        {
            currState = confirm.yes;
            //fill array with functions
            getNextState = new machine[] { Sleep, Yes, No };
        }

        internal confirm update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void goTo(confirm state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static confirm Sleep()
        {
            return confirm.sleep;
        }

        private static confirm Yes()
        {
            if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
                return confirm.no;
            return confirm.yes;
        }
        private static confirm No()
        {
            if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
                return confirm.yes;
            return confirm.no;
        }
    }
}
