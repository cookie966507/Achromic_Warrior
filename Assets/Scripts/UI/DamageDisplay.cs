using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Display the UI for numbers when damage occurs
 */
namespace Assets.Scripts.UI
{
    public class DamageDisplay : MonoBehaviour
    {
        //empty gameobject to put numbers in
        public GameObject _emptyNums;
        public string _numPath = "Prefabs/Numbers/";

        //static instance
        public static DamageDisplay instance;

        //spacing between numbers
        public float _spacing = 0.35f;

        void Awake()
        {
            //init instance
            instance = this;
        }

        //show damage at a point and make it a certain color
        public void ShowDamage(int _damage, Vector2 _pos, ColorElement _color)
        {
            //make new display object
            GameObject _display = (GameObject)GameObject.Instantiate(_emptyNums, _pos, Quaternion.identity);

            //length of the string representation of the value
            int _length = ((int)_damage).ToString().Length;

            //start to the left of the center for odd digit numbers
            float _startX = 0f - _length / 2 * _spacing;

            //if one digit set at origin
            if (_length == 1) _startX = 0;
            //if even digit number adjust spacing
            else if ((_length & 1) == 0) _startX += _spacing / 2;

            //create number
            for (int i = 0; i < _length; i++)
            {
                //find digit
                GameObject num = (GameObject)Resources.Load(_numPath + _damage.ToString().Substring(i, 1));
                GameObject newNum = (GameObject)GameObject.Instantiate(num);
                //set within empty num
                newNum.transform.parent = _display.transform;
                //arrange position based on digit
                newNum.transform.localPosition = new Vector2(_startX + i * _spacing, 0);
                newNum.GetComponent<Renderer>().material.color = CustomColor.GetColor(_color);
            }
        }
    }
}
