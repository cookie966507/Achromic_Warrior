using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class VideoWeb : ButtonHandler
    {
        public GameObject[] cursors;
        public UnityEngine.UI.Slider quality;
        public UnityEngine.UI.Text qualityText;
        public UnityEngine.UI.Toggle fullscreen;

        private static UnityEngine.UI.Slider qualityBar;
        private static UnityEngine.UI.Toggle fullscreenButton;
        private static UnityEngine.UI.Text qualText;

        private VideoWebStateMachine machine = new VideoWebStateMachine();
        private delegate void state();
        private state[] doState;
        private VideoWebStateMachine.video currState;
        private static Resolution[] res;

        private static bool isLeft;
        private static bool touchedResolution;
        private static bool touchedFullscreen;
        private static bool touchedQuality;


        private static string videoHash = "Achromic video";
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
            doState = new state[] { Sleep, Fullscreen, Quality, Accept, Exit };
            qualityBar = quality;
            fullscreenButton = fullscreen;
            qualText = qualityText;
            touchedFullscreen = true;
            touchedQuality = true;
            qualityBar.minValue = 0;
            qualityBar.maxValue = QualitySettings.names.Length - 1;
            if (PlayerPrefs.HasKey(videoHash + 0))
            {
                fullscreen.isOn = (PlayerPrefs.GetInt(videoHash + 1) == 0) ? false : true;
                qualityBar.value = PlayerPrefs.GetInt(videoHash + 2);
            }
            else
            {
                PlayerPrefs.SetInt(videoHash + 0, 0);
                PlayerPrefs.SetInt(videoHash + 1, 0);
                PlayerPrefs.SetInt(videoHash + 2, 0);

                fullscreen.isOn = false;
                qualityBar.value = 0;
            }
            qualText.text = QualitySettings.names[(int)qualityBar.value];
        }

        void Update()
        {
            if (Kernel.enabled)
            {
                VideoWebStateMachine.video prevState = currState;
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
		
        private static void Fullscreen()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
            {
                touchedFullscreen = true;
                fullscreenButton.isOn = !fullscreenButton.isOn;
                doFullscreen();
            }
			if (CustomInput.CancelFreshPressDeleteOnRead)
				doExit();
        }
        private static void doFullscreen()
        {

        }

        private static void Quality()
        {
            if (CustomInput.LeftFreshPressDeleteOnRead)
            {
                touchedQuality = true;
                int temp = (int)qualityBar.value;
                temp--;
                if (temp < 0)
                    temp = QualitySettings.names.Length - 1;
                qualityBar.value = temp;
                doQuality(temp);
            }
            if (CustomInput.RightFreshPressDeleteOnRead)
            {
                touchedQuality = true;
                int temp = (int)qualityBar.value;
                temp++;
                if (temp >= QualitySettings.names.Length)
                    temp = 0;
                qualityBar.value = temp;
                doQuality(temp);
            }
			if (CustomInput.CancelFreshPressDeleteOnRead)
				doExit();
        }
        private static void doQuality(int val)
        {
            qualText.text = QualitySettings.names[val];
        }

        private static void Accept()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doAccept();
			if (CustomInput.CancelFreshPressDeleteOnRead)
				doExit();
        }
        private static void doAccept()
        {
            ConformationWindow.getConformation(updateSettings);
        }

        private static void Exit()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doExit();
			if (CustomInput.CancelFreshPressDeleteOnRead)
				doExit();
        }
        private static void doExit()
        {
            Kernel.transition(false, isLeft, 0);
		}

        public void FullscreenClick()
        {
            if (touchedFullscreen)
            {
                touchedFullscreen = false;
                return;
            }
            if (currState == VideoWebStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VideoWebStateMachine.video.fullscreen);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VideoWebStateMachine.video.fullscreen - 1].SetActive(true);
            doFullscreen();
        }

        public void QualityClick()
        {
            if (touchedQuality)
            {
                touchedQuality = false;
                return;
            }
            if (currState == VideoWebStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VideoWebStateMachine.video.quality);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VideoWebStateMachine.video.quality - 1].SetActive(true);
            doQuality((int)qualityBar.value);
        }

        public void AcceptClick()
        {
            if (currState == VideoWebStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VideoWebStateMachine.video.accept);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VideoWebStateMachine.video.accept - 1].SetActive(true);
            doAccept();
        }
        public void ExitClick()
        {
            if (currState == VideoWebStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VideoWebStateMachine.video.exit);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            //cursors[(int)AudioStateMachine.audio.exit - 1].SetActive(true);
            doExit();
        }

        static void updateSettings(bool update)
        {
            if (update)
            {
				Screen.SetResolution(
					Screen.width,
					Screen.height,
					fullscreenButton.isOn);
				FindObjectOfType<Camera>().ResetAspect();
                QualitySettings.SetQualityLevel((int)qualityBar.value);
                PlayerPrefs.SetInt(videoHash + 1, fullscreenButton.isOn ? 1 : 0);
                PlayerPrefs.SetInt(videoHash + 2, (int)qualityBar.value);
            }
        }
    }
    class VideoWebStateMachine
    {
        internal enum video { sleep, fullscreen, quality, accept, exit };
        private delegate video machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private video currState;
        private video sleepState = video.fullscreen;

        internal VideoWebStateMachine()
        {
            currState = video.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Fullscreen, Quality, Accept, Exit };
        }

        internal video update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != video.sleep)
            {
                sleepState = currState;
                currState = video.sleep;
            }
        }

        internal void goTo(video state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static video Sleep()
        {
            return video.sleep;
        }
	
        private static video Fullscreen()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return video.exit;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return video.quality;
            return video.fullscreen;
        }

        private static video Quality()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return video.fullscreen;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return video.exit;
            return video.quality;
        }
        private static video Accept()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return video.quality;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return video.fullscreen;
            if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
                return video.exit;
            return video.accept;
        }
        private static video Exit()
        {
            if (CustomInput.UpFreshPressDeleteOnRead)
                return video.quality;
            if (CustomInput.DownFreshPressDeleteOnRead)
                return video.fullscreen;
            if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
                return video.accept;
            return video.exit;
        }
    }
}
