using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;

/*
 * Class to control all of the player data relating to color
 */
namespace Assets.Scripts.Player
{
    public class PlayerColorData : MonoBehaviour
    {
        //current color available for the meters (instead of a vector)
        private Color _meterColor;

        //current color
        private ColorElement _color;

        //all of the pieces that can change color
        private PlayerArmor[] _armor;

        //red color meter
        private ColorMeter _r;
        //green color meter
        private ColorMeter _g;
        //blue color meter
        private ColorMeter _b;

        //reference to the controller
        private PlayerController _controller;

        //for decreasing the meters while a color is active
        private float _time = 0f;
        private float _decreaseDelay = 0.7f;
        private float _decreaseAmount = 5f;

        //updating stats based on the type of color
        private float _primary = 1f;
        private float _secondary = 0.5f;
        private float _upperTertiary = 0.75f;
        private float _lowerTertiary = 0.25f;

        //base stats
        private float _baseAtk = 1f;
        private float _baseDef = 0f;
        private float _baseSpd = 4f;

        //changing stats (used as ratio with _primary, _secondary, etc)
        private float _atk = 0f;
        private float _def = 0f;
        private float _spd = 0f;

        //max a stat can be
        private float _maxAtk = 9f;
        private float _maxDef = 9f;
        private float _maxSpd = 11f;

        //player health
        private float _health = 20f;

        void Awake()
        {
            //init meter color
            _meterColor = new Color(0f, 0f, 0f);
            //find controller
            _controller = this.GetComponent<PlayerController>();
        }

        void Start()
        {
            //get all armor pieces
            _armor = GameObject.FindObjectsOfType<PlayerArmor>();

            //find UI meters
            _r = GameObject.Find("R").GetComponent<ColorMeter>();
            _g = GameObject.Find("G").GetComponent<ColorMeter>();
            _b = GameObject.Find("B").GetComponent<ColorMeter>();
        }

        void Update()
        {
            //if color meter should be decreasing
            if (!_color.Equals(ColorElement.White))
                DeductTime();
        }

        //revert all values
        public void ResetToWhite()
        {
            Color = ColorElement.White;
            _atk = 0f;
            _def = 0f;
            _spd = 0f;

            _controller.UpdateSpeed(Speed);

			_r.UpdateSphereColor(ColorElement.White);
			_g.UpdateSphereColor(ColorElement.White);
			_b.UpdateSphereColor(ColorElement.White);
        }

        //updates the possible color of the player
        public void AddColor(Color _newColor, float _amount)
        {
            //different format for getting color in proper form (0-1)
            _newColor = CustomColor.ConvertColor(_newColor.r * _amount, _newColor.g * _amount, _newColor.b * _amount);
            _meterColor += _newColor;

            //bound colors
            _meterColor.r = Mathf.Clamp(_meterColor.r, 0f, 1f);
            _meterColor.g = Mathf.Clamp(_meterColor.g, 0f, 1f);
            _meterColor.b = Mathf.Clamp(_meterColor.b, 0f, 1f);
            _meterColor.a = Mathf.Clamp(_meterColor.a, 0f, 1f);

            _r.UpdateMeter(_meterColor.r);
            _g.UpdateMeter(_meterColor.g);
            _b.UpdateMeter(_meterColor.b);
        }

        //reducing a color meter base on the color
        private void DeductTime()
        {
            _time += Time.deltaTime;
            if (_time > _decreaseDelay)
            {
                _time = 0f;
                switch (_color)
                {
                    case ColorElement.Red:
                        AddColor(new Color(-_primary, 0f, 0f), _decreaseAmount);
                        if (_meterColor.r == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Green:
                        AddColor(new Color(0f, -_primary, 0f), _decreaseAmount);
                        if (_meterColor.g == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Blue:
                        AddColor(new Color(0f, 0f, -_primary), _decreaseAmount);
                        if (_meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Yellow:
                        AddColor(new Color(-_secondary, -_secondary, 0f), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.g == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Cyan:
                        AddColor(new Color(0f, -_secondary, -_secondary), _decreaseAmount);
                        if (_meterColor.g == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Magenta:
                        AddColor(new Color(-_secondary, 0f, -_secondary), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Orange:
                        AddColor(new Color(-_upperTertiary, -_lowerTertiary, 0f), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.g == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Chartreuse:
                        AddColor(new Color(-_lowerTertiary, -_upperTertiary, 0f), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.g == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Spring:
                        AddColor(new Color(0f, -_upperTertiary, -_lowerTertiary), _decreaseAmount);
                        if (_meterColor.g == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Azure:
                        AddColor(new Color(0f, -_lowerTertiary, -_upperTertiary), _decreaseAmount);
                        if (_meterColor.g == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Rose:
                        AddColor(new Color(-_upperTertiary, 0f, -_lowerTertiary), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Violet:
                        AddColor(new Color(-_lowerTertiary, 0f, -_upperTertiary), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;

                    case ColorElement.Black:
                        AddColor(new Color(-_primary * 2, -_primary * 2, -_primary * 2), _decreaseAmount);
                        if (_meterColor.r == 0 || _meterColor.g == 0 || _meterColor.b == 0)
                        {
                            ResetToWhite();
                        }
                        break;
                }
            }
        }

        //updates the stats of the player base on the current color
        public void UpdateStats()
        {
            switch (_color)
            {
                case ColorElement.Red:
                    _atk = _primary;
                    _spd = 0f;
                    _def = 0f;
                    break;

                case ColorElement.Green:
                    _atk = 0f;
                    _spd = _primary;
                    _def = 0f;
                    break;

                case ColorElement.Blue:
                    _atk = 0f;
                    _spd = 0f;
                    _def = _primary;
                    break;

                case ColorElement.Yellow:
                    _atk = _secondary;
                    _spd = _secondary;
                    _def = 0f;
                    break;

                case ColorElement.Cyan:
                    _atk = 0f;
                    _spd = _secondary;
                    _def = _secondary;
                    break;

                case ColorElement.Magenta:
                    _atk = _secondary;
                    _spd = 0f;
                    _def = _secondary;
                    break;

                case ColorElement.Orange:
                    _atk = _upperTertiary;
                    _spd = _lowerTertiary;
                    _def = 0f;
                    break;

                case ColorElement.Chartreuse:
                    _atk = _lowerTertiary;
                    _spd = _upperTertiary;
                    _def = 0f;
                    break;

                case ColorElement.Spring:
                    _atk = 0f;
                    _spd = _upperTertiary;
                    _def = _lowerTertiary;
                    break;

                case ColorElement.Azure:
                    _atk = 0f;
                    _spd = _lowerTertiary;
                    _def = _upperTertiary;
                    break;

                case ColorElement.Rose:
                    _atk = _upperTertiary;
                    _spd = 0f;
                    _def = _lowerTertiary;
                    break;

                case ColorElement.Violet:
                    AddColor(new Color(-_lowerTertiary, 0f, -_upperTertiary), _decreaseAmount);
                    _atk = _lowerTertiary;
                    _spd = 0f;
                    _def = _upperTertiary;
                    break;

                case ColorElement.Black:
                    _atk = _primary;
                    _spd = _primary;
                    _def = _primary;
                    break;
            }
            _controller.UpdateSpeed(Speed);
        }

        //gets the color of all the meters
        public Color MeterColor
        {
            get { return _meterColor; }
            //set{_meterColor = value;}
        }

        //color of the player
        public ColorElement Color
        {
            get { return _color; }
            set
            {
                _color = value;
                for (int i = 0; i < _armor.Length; i++)
                {
                    //update the armor on color change
                    _armor[i].UpdateArmor(_color);
                }
            }
        }

        //all the armor pieces
        public PlayerArmor[] Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        //stats that return modified values based on color
        public float Attack
        {
            get { return _baseAtk + _atk * _maxAtk; }
        }

        public float Defense
        {
            get { return _baseDef + _def * _maxDef; }
        }

        public float Speed
        {
            get { return _baseSpd + _spd * _maxSpd; }
        }

        //health of the player
        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }
    }
}
