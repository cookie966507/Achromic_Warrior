using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Audio : ButtonHandler
    {
        public GameObject[] cursors;
        public UnityEngine.UI.Scrollbar music;
        public UnityEngine.UI.Scrollbar sfx;

        private static UnityEngine.UI.Scrollbar musicBar;
        private static UnityEngine.UI.Scrollbar sfxBar;

        private AudioStateMachine machine = new AudioStateMachine();
        private delegate void state();
        private state[] doState;
        private AudioStateMachine.audio currState;

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
            doState = new state[] { Sleep, Music, SFX, Exit };
            musicBar = music;
            sfxBar = sfx;
        }

        void Update()
        {
            AudioStateMachine.audio prevState = currState;
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

        private static void Music()
        {
            if (CustomInput.LeftHeld)
            {
                float temp = musicBar.value;
                if (temp > 0)
                    temp -= .1f;
                musicBar.value = temp;
                doMusic(temp);
            }
            if (CustomInput.RightHeld)
            {
                float temp = musicBar.value;
                if (temp < 1)
                    temp += .1f;
                musicBar.value = temp;
                doMusic(temp);
            }
        }
        private static void doMusic(float val)
        {
            Data.SoundManager.SliderMusic(val);
        }

        private static void SFX()
        {
            if (CustomInput.LeftHeld)
            {
                float temp = sfxBar.value;
                if (temp > 0)
                    temp -= .1f;
                sfxBar.value = temp;
                doSFX(temp);
            }
            if (CustomInput.RightHeld)
            {
                float temp = sfxBar.value;
                if (temp < 1)
                    temp += .1f;
                sfxBar.value = temp;
                doSFX(temp);
            }
        }
        private static void doSFX(float val)
        {
            Data.SoundManager.SliderSFX(val);
        }

        private static void Exit()
        {
            if (CustomInput.AcceptFreshPress)
                doExit();
        }
        private static void doExit()
        {
            Kernel.transition(false, isLeft, 0);
        }

        public void MusicClick()
        {
            if (currState == AudioStateMachine.audio.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(AudioStateMachine.audio.music);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)AudioStateMachine.audio.music - 1].SetActive(true);
            doMusic(musicBar.value);
        }

        public void SFXClick()
        {
            if (currState == AudioStateMachine.audio.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(AudioStateMachine.audio.sfx);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)AudioStateMachine.audio.sfx - 1].SetActive(true);
            doSFX(sfxBar.value);
        }

        public void ExitClick()
        {
            if (currState == AudioStateMachine.audio.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(AudioStateMachine.audio.exit);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            //cursors[(int)AudioStateMachine.audio.exit - 1].SetActive(true);
            doExit();
        }
    }
    class AudioStateMachine
    {
        internal enum audio { sleep, music, sfx, exit };
        private delegate audio machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private audio currState;
        private static float hold = 0;//used for delays
        private static bool die = false;
        private static bool doubleJumped = false;
        private audio sleepState = audio.music;

        internal AudioStateMachine()
        {
            currState = audio.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Music, SFX, Exit };
        }

        internal audio update()
        {
            return currState = getNextState[((int)currState)]();//gets te next Enums.PlayerState
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != audio.sleep)
            {
                sleepState = currState;
                currState = audio.sleep;
            }
        }

        internal void goTo(audio state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static audio Sleep()
        {
            return audio.sleep;
        }

        private static audio Music()
        {
            if (CustomInput.UpFreshPress)
                return audio.exit;
            if (CustomInput.DownFreshPress)
                return audio.sfx;
            return audio.music;
        }
        private static audio SFX()
        {
            if (CustomInput.UpFreshPress)
                return audio.music;
            if (CustomInput.DownFreshPress)
                return audio.exit;
            return audio.sfx;
        }
        private static audio Exit()
        {
            if (CustomInput.UpFreshPress)
                return audio.sfx;
            if (CustomInput.DownFreshPress)
                return audio.music;
            return audio.exit;
        }
    }
}
