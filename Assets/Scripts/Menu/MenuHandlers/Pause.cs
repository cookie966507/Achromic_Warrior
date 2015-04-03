using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Pause : MonoBehaviour
    {
        public GameObject[] cursors;
        private static Canvas win;

        private static PauseStateMachine machine = new PauseStateMachine();
        private delegate void state();
        private state[] doState;
        private PauseStateMachine.pause currState;

        void Start()
        {
            win = this.gameObject.GetComponent<Canvas>();
            doState = new state[] { Sleep, Continue, Quit };
            win.enabled = false;
        }

        void Update()
        {
            PauseStateMachine.pause prevState = currState;
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
            if (Data.GameManager.State == Enums.GameState.Pause)
            {
                machine.goTo(PauseStateMachine.pause.resume);
                win.enabled = true;
            }
        }

        private static void Continue()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead || CustomInput.PauseFreshPressDeleteOnRead)
                doContinue();
        }
        private static void doContinue()
        {
            win.enabled = false;
            machine.goTo(PauseStateMachine.pause.sleep);
            Data.GameManager.Unpause();
        }

        private static void Quit()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doQuit();
            if (CustomInput.PauseFreshPressDeleteOnRead)
                doContinue();
        }
        private static void doQuit()
        {
            win.enabled = false;
            machine.goTo(PauseStateMachine.pause.sleep);
            Data.GameManager.GotoLevel("Level_Select");
        }

        public void YesClick()
        {
            machine.goTo(PauseStateMachine.pause.resume);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)PauseStateMachine.pause.resume - 1].SetActive(true);
            doContinue();
        }

        public void NoClick()
        {
            machine.goTo(PauseStateMachine.pause.quit);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)PauseStateMachine.pause.resume - 1].SetActive(true);
            doQuit();
        }
    }
    class PauseStateMachine
    {
        internal enum pause { sleep, resume, quit };
        private delegate pause machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private pause currState;

        internal PauseStateMachine()
        {
            currState = pause.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Yes, No };
        }

        internal pause update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void goTo(pause state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static pause Sleep()
        {
            return pause.sleep;
        }

        private static pause Yes()
        {
            if (CustomInput.UpFreshPressDeleteOnRead || CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                return pause.quit;
            return pause.resume;
        }
        private static pause No()
        {
            if (CustomInput.UpFreshPressDeleteOnRead || CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                return pause.resume;
            return pause.quit;
        }
    }
}
