using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Video : ButtonHandler
    {
        public GameObject[] cursors;
        public UnityEngine.UI.Slider resolution;
        public UnityEngine.UI.Text resolutionText;
        public UnityEngine.UI.Slider quality;
        public UnityEngine.UI.Text qualityText;
        public UnityEngine.UI.Toggle fullscreen;

        private static UnityEngine.UI.Slider resolutionBar;
        private static UnityEngine.UI.Slider qualityBar;
        private static UnityEngine.UI.Toggle fullscreenButton;
        private static UnityEngine.UI.Text resText;
        private static UnityEngine.UI.Text qualText;

        private VidioStateMachine machine = new VidioStateMachine();
        private delegate void state();
        private state[] doState;
        private VidioStateMachine.video currState;
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
            doState = new state[] { Sleep, Resolutions, Fullscreen, Quality, Accept, Exit };
            resolutionBar = resolution;
            qualityBar = quality;
            fullscreenButton = fullscreen;
            resText = resolutionText;
            qualText = qualityText;
            List<Resolution> temp = new List<Resolution>();
            double num = 0;
            foreach (Resolution r in Screen.resolutions)
            {
                num = r.width / 16.0;
                if ((double)(r.height) / num - 9 < 1 && (double)(r.height) / num - 9 > -1)
                {
                    temp.Add(r);
                }
            }
            if (temp.Count == 0)
                temp.Add(Screen.resolutions[0]);
            res = temp.ToArray();
            touchedResolution = true;
            touchedFullscreen = true;
            touchedQuality = true;
            resolutionBar.minValue = 0;
            resolutionBar.maxValue = res.Length - 1;
            qualityBar.minValue = 0;
            qualityBar.maxValue = QualitySettings.names.Length-1;
            if(PlayerPrefs.HasKey(videoHash+0))
            {
                if (PlayerPrefs.GetInt(videoHash + 0) >= res.Length)
                {
                    resolutionBar.value = 0;
                    PlayerPrefs.SetInt(videoHash + 0, 0);
                }
                else
                    resolutionBar.value = PlayerPrefs.GetInt(videoHash + 0);
                fullscreen.isOn=(PlayerPrefs.GetInt(videoHash+1) == 0) ? false : true;
                qualityBar.value = PlayerPrefs.GetInt(videoHash + 2);
            }
            else
            {
                PlayerPrefs.SetInt(videoHash + 0, 0);
                PlayerPrefs.SetInt(videoHash + 1, 0);
                PlayerPrefs.SetInt(videoHash + 2, 0);
                resolutionBar.value = 0;
                fullscreen.isOn = false;
                qualityBar.value = 0;
            }
            resText.text = res[(int)resolutionBar.value].width + "x" + res[(int)resolutionBar.value].height;
            qualText.text = QualitySettings.names[(int)qualityBar.value];
        }

        void Update()
        {
            VidioStateMachine.video prevState = currState;
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

        private static void Resolutions()
        {
			if (CustomInput.LeftFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                touchedResolution = true;
                int temp = (int)resolutionBar.value;
                temp --;
                if (temp < 0)
                    temp = res.Length-1;
                resolutionBar.value = temp;
                doResolutions(temp);
            }
			if (CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.RightArrow))
            {
                touchedResolution = true;
                int temp = (int)resolutionBar.value;
                temp ++;
                if (temp >= res.Length)
                    temp = 0;
                resolutionBar.value = temp;
                doResolutions(temp);
            }
        }
        private static void doResolutions(int val)
        {
            resText.text = res[val].width + "x" + res[val].height;
        }

        private static void Fullscreen()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
            {
                touchedFullscreen = true;
                fullscreenButton.isOn = !fullscreenButton.isOn;
                doFullscreen();
            }
        }
        private static void doFullscreen()
        {
            
        }

        private static void Quality()
        {
			if (CustomInput.LeftFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                touchedQuality = true;
                int temp = (int)qualityBar.value;
                temp--;
                if (temp < 0)
                    temp = QualitySettings.names.Length-1;
                qualityBar.value = temp;
                doQuality(temp);
            }
			if (CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.RightArrow))
            {
                touchedQuality = true;
                int temp = (int)qualityBar.value;
                temp++;
                if (temp >= QualitySettings.names.Length)
                    temp = 0;
                qualityBar.value = temp;
                doQuality(temp);
            }
        }
        private static void doQuality(int val)
        {
            qualText.text = QualitySettings.names[val];
        }

        private static void Accept()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doAccept();
        }
        private static void doAccept()
        {
            ConformationWindow.getConformation(updateSettings);
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

        public void ResolutionClick()
        {
            if(touchedResolution)
            {
                touchedResolution = false;
                return;
            }
            if (currState == VidioStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VidioStateMachine.video.resolutions);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VidioStateMachine.video.resolutions - 1].SetActive(true);
            doResolutions((int)resolutionBar.value);
        }

        public void FullscreenClick()
        {
            if(touchedFullscreen)
            {
                touchedFullscreen = false;
                return;
            }
            if (currState == VidioStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VidioStateMachine.video.fullscreen);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VidioStateMachine.video.fullscreen - 1].SetActive(true);
            doFullscreen();
        }

        public void QualityClick()
        {
            if(touchedQuality)
            {
                touchedQuality = false;
                return;
            }
            if (currState == VidioStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VidioStateMachine.video.quality);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VidioStateMachine.video.quality - 1].SetActive(true);
            doQuality((int)qualityBar.value);
        }

        public void AcceptClick()
        {
            if (currState == VidioStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VidioStateMachine.video.accept);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)VidioStateMachine.video.accept - 1].SetActive(true);
            doAccept();
        }
        public void ExitClick()
        {
            if (currState == VidioStateMachine.video.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(VidioStateMachine.video.exit);
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
                        res[(int)resolutionBar.value].width,
                        res[(int)resolutionBar.value].height,
                        fullscreenButton.isOn);
                FindObjectOfType<Camera>().ResetAspect();
                QualitySettings.SetQualityLevel((int)qualityBar.value);
                PlayerPrefs.SetInt(videoHash + 0, (int)resolutionBar.value);
                PlayerPrefs.SetInt(videoHash + 1, fullscreenButton.isOn ? 1 : 0);
                PlayerPrefs.SetInt(videoHash + 2, (int)qualityBar.value);
            }
        }
    }
    class VidioStateMachine
    {
        internal enum video { sleep, resolutions, fullscreen, quality, accept, exit };
        private delegate video machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private video currState;
        private video sleepState = video.resolutions;

        internal VidioStateMachine()
        {
            currState = video.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Resolutions, Fullscreen, Quality, Accept, Exit };
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

        private static video Resolutions()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return video.exit;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return video.fullscreen;
            return video.resolutions;
        }
        private static video Fullscreen()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return video.resolutions;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return video.quality;
            return video.fullscreen;
        }

        private static video Quality()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return video.fullscreen;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return video.exit;
            return video.quality;
        }
        private static video Accept()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return video.quality;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return video.resolutions;
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                return video.exit;
            return video.accept;
        }
        private static video Exit()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return video.quality;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return video.resolutions;
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                return video.accept;
            return video.exit;
        }
    }
}
