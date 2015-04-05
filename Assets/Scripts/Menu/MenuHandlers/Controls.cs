using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Controls : ButtonHandler
    {
        public GameObject[] cursors;

        private ControlsStateMachine machine = new ControlsStateMachine();
        private delegate void state();
        private state[] doState;
        private ControlsStateMachine.control currState;

        private static bool isLeft;
        public override void setLeft()
        {
            isLeft = true;
        }

        public override void setRight()
        {
            isLeft = false;
        }

        void Start()
        {
            doState = new state[] { Sleep, KeyBoard, GamePad, Exit };
        }

        void Update()
        {
            if (Kernel.enabled)
            {
                ControlsStateMachine.control prevState = currState;
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
        }

        public override void wake()
        {
            machine.wake();
            foreach (GameObject g in cursors)
                g.SetActive(false);
            int cursor = (int)currState - 1;
            if (cursor >= 0)
                cursors[cursor].SetActive(true);
        }

        public override void sleep()
        {
            machine.sleep();
        }

        private static void Sleep()
        {
        }

        private static void KeyBoard()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doKeyBoard();
        }
        private static void doKeyBoard()
        {
            Kernel.controlsEnter(0);
        }

        private static void GamePad()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doGamePad();
        }
        private static void doGamePad()
        {
            Kernel.controlsEnter(1);
        }

        private static void Exit()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doExit();
        }
        private static void doExit()
        {
            Kernel.transition(false, isLeft, 0);
        }

        public void KeyBoardClick()
        {
            if (currState == ControlsStateMachine.control.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(ControlsStateMachine.control.keyBoard);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)ControlsStateMachine.control.keyBoard - 1].SetActive(true);
            doKeyBoard();
        }

        public void GamePadClick()
        {
            if (currState == ControlsStateMachine.control.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(ControlsStateMachine.control.gamePad);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)ControlsStateMachine.control.gamePad - 1].SetActive(true);
            doGamePad();
        }

        public void ExitClick()
        {
            if (currState == ControlsStateMachine.control.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(ControlsStateMachine.control.exit);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            //cursors[(int)ControlsStateMachine.control.exit - 1].SetActive(true);
            doExit();
        }
    }
    class ControlsStateMachine
    {
        internal enum control { sleep, keyBoard, gamePad, exit };
        private delegate control machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private control currState;
        private control sleepState = control.keyBoard;

        internal ControlsStateMachine()
        {
            currState = control.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, KeyBoard, GamePad, Exit };
        }

        internal control update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != control.sleep)
            {
                sleepState = currState;
                currState = control.sleep;
            }
        }

        internal void goTo(control state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static control Sleep()
        {
            return control.sleep;
        }

        private static control KeyBoard()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return control.exit;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return control.gamePad;
            return control.keyBoard;
        }
        private static control GamePad()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return control.keyBoard;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return control.exit;
            return control.gamePad;
        }
        private static control Exit()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return control.gamePad;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return control.keyBoard;
            return control.exit;
        }
    }
}
