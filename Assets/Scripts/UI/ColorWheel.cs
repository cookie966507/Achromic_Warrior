using UnityEngine;
using System.Collections;
using Assets.Scripts.Util;
using Assets.Scripts.Enums;

/*
 * UI Color wheel to select tabs and color
 */
namespace Assets.Scripts.UI
{
    public class ColorWheel : MonoBehaviour
    {
        //angle degrees separating each tab
        private float _angle = 30f;

        //ref for smooth damp
        private float _zVel;

        //current and target z rotations
        private float _z;
        private float _currZ;

        //speed to spin
        private float _speed = 0.1f;

        //active color on top
        private ColorElement _activeColor;

        //list of tabs in the wheel
        public ColorTab[] _tabs;
        //active tab on top
        private ColorTab _activeTab;
        //index for tabs
        private int _tabCounter = 0;

        //reference to player
        private Player.PlayerColorData _player;

        //make top tab stick out by changing scale
        private Vector3 _normalScale;
        private Vector3 _upScale;

        //reference to meters
        private ColorMeter _r;
        private ColorMeter _g;
        private ColorMeter _b;

        void Awake()
        {
            //assign tab scales
            _normalScale = new Vector3(1f, 1f, 1f);
            _upScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        void Start()
        {
            //find player
            _player = GameObject.Find("player").GetComponent<Player.PlayerColorData>();

            //init active tab
            _activeTab = _tabs[_tabCounter];
            _activeTab.transform.localScale = _upScale;
            _activeTab.Label.gameObject.SetActive(true);

            //find meters
            _r = GameObject.Find("R").GetComponent<ColorMeter>();
            _g = GameObject.Find("G").GetComponent<ColorMeter>();
            _b = GameObject.Find("B").GetComponent<ColorMeter>();
        }

        void Update()
        {
            //if cycling spin the wheel
            if (CustomInput.CycleLeftFreshPress)
            {
                _z -= _angle;
                UpdateWheel(-1);
            }
            if (CustomInput.CycleRightFreshPress)
            {
                _z += _angle;
                UpdateWheel(1);
            }

            //going Achromic ---- PUT IN PLAYER CLASS
            if (CustomInput.CycleLeftFreshPress && CustomInput.CycleRightFreshPress && _r.transform.localScale.x == 1 && _g.transform.localScale.x == 1 && _b.transform.localScale.x == 1)
            {
                _player.Color = ColorElement.Black;
                _player.UpdateStats();
            }

            //changing color based on meter values and current color
            if (CustomInput.ChangeColorFreshPress)
            {
                //player has no color
                if (_player.Color.Equals(ColorElement.White))
                {
                    //active tab is available
                    if (!_activeTab.GreyTab.activeSelf)
                    {
                        _player.Color = _activeTab.Color;
                        _player.UpdateStats();
                    }
                }
                //player has a color
                else
                {
                    //on same tab as color
                    if (_player.Color.Equals(_activeTab.Color))
                    {
                        //cancel color
                        _player.ResetToWhite();
                    }
                    //different color tab
                    else
                    {
                        //if tab is unavailable
                        if (_activeTab.GreyTab.activeSelf)
                        {
                            //cancel color
                            _player.ResetToWhite();
                        }
                        //tab is avaiable
                        else
                        {
                            //change to that color
                            _player.Color = _activeTab.Color;
                            _player.UpdateStats();
                        }
                    }
                }
            }

            //update the wheel spinning
            _currZ = Mathf.SmoothDamp(_currZ, _z, ref _zVel, _speed);

            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _currZ));
        }

        //go through the tabs after spinning
        public void UpdateWheel(int _dir)
        {
            //increase index and reset if necessary
            _tabCounter += _dir;
            if (_tabCounter == _tabs.Length) _tabCounter = 0;
            else if (_tabCounter == -1) _tabCounter = _tabs.Length - 1;

            //change scales
            _activeTab.transform.localScale = _normalScale;
            _activeTab.Label.gameObject.SetActive(false);

            //update active tab
            _activeTab = _tabs[_tabCounter];

            //change scales
            _activeTab.transform.localScale = _upScale;
            _activeTab.Label.gameObject.SetActive(true);

        }

        //setting whether available for use or not based on meter values
        public void UpdateTabs()
        {
            _tabs[(int)ColorElement.Red].GreyTab.SetActive(_r.transform.localScale.x == 0f); //red
            _tabs[(int)ColorElement.Green].GreyTab.SetActive(_g.transform.localScale.x == 0f); //green
            _tabs[(int)ColorElement.Blue].GreyTab.SetActive(_b.transform.localScale.x == 0f); //blue
            _tabs[(int)ColorElement.Yellow].GreyTab.SetActive(_g.transform.localScale.x == 0f || _r.transform.localScale.x == 0f); //yellow
            _tabs[(int)ColorElement.Cyan].GreyTab.SetActive(_b.transform.localScale.x == 0f || _g.transform.localScale.x == 0f); //cyan
            _tabs[(int)ColorElement.Magenta].GreyTab.SetActive(_r.transform.localScale.x == 0f || _b.transform.localScale.x == 0f); //magenta
            _tabs[(int)ColorElement.Orange].GreyTab.SetActive(_g.transform.localScale.x == 0f || _r.transform.localScale.x == 0f); //orange
            _tabs[(int)ColorElement.Chartreuse].GreyTab.SetActive(_g.transform.localScale.x == 0f || _r.transform.localScale.x == 0f); //chartruese
            _tabs[(int)ColorElement.Spring].GreyTab.SetActive(_b.transform.localScale.x == 0f || _g.transform.localScale.x == 0f); //spring
            _tabs[(int)ColorElement.Azure].GreyTab.SetActive(_b.transform.localScale.x == 0f || _g.transform.localScale.x == 0f); //azure
            _tabs[(int)ColorElement.Rose].GreyTab.SetActive(_r.transform.localScale.x == 0f || _b.transform.localScale.x == 0f); //rose
            _tabs[(int)ColorElement.Violet].GreyTab.SetActive(_r.transform.localScale.x == 0f || _b.transform.localScale.x == 0f); //violet
        }
    }
}
