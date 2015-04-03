using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Main : ButtonHandler
    {
        public GameObject[] cursors;

        private MainStateMachine machine = new MainStateMachine();
        private delegate void state();
        private state[] doState;
        private MainStateMachine.main currState;

		private static bool _trainingPlayed = false;

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
            doState = new state[] { Sleep, Play, Settings, Credits };
        }

        void Update()
        {
            MainStateMachine.main prevState = currState;
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

        private static void Play()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doPlay();
        }
        private static void doPlay()
        {
			if(!_trainingPlayed)
			{
				_trainingPlayed = true;
				Data.GameManager.GotoLevel("training");
			}
			else
			{
				Data.GameManager.GotoLevel("Level_Select");
			}
        }

        private static void Settings()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doSettings();
        }
        private static void doSettings()
        {
            Kernel.transition(true, isLeft, 0);
        }

        private static void Credits()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doCredits();
        }
        private static void doCredits()
        {
            Data.GameManager.GotoLevel("credits");
        }

        public void PlayClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.play);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.play - 1].SetActive(true);
            doPlay();
        }

        public void SettingsClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.settings);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.settings - 1].SetActive(true);
            doSettings();
        }

        public void CreditsClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.credits);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.credits - 1].SetActive(true);
            doCredits();
        }
    }
    class MainStateMachine
    {
        internal enum main { sleep, play, settings, credits };
        private delegate main machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private main currState;
        private main sleepState = main.play;

        internal MainStateMachine()
        {
            currState = main.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Play, Settings, Credits };
        }

        internal main update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != main.sleep)
            {
                sleepState = currState;
                currState = main.sleep;
            }
        }

        internal void goTo(main state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static main Sleep()
        {
            return main.sleep;
        }

        private static main Play()
        {
            if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return main.credits;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return main.settings;
            return main.play;
        }
        private static main Settings()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return main.play;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return main.credits;
            return main.settings;
        }
        private static main Credits()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return main.settings;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return main.play;
            return main.credits;
        }
    }
}