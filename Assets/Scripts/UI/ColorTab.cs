using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Enums;

/*
 * A tab in the color wheel for selecting current color
 */
namespace Assets.Scripts.UI
{
    public class ColorTab : MonoBehaviour
    {
        //color of the tab
        public ColorElement _color;

        //name of the colpr
        private Text _name;
        //grey cover if disabled
        private GameObject _greyTab;

        void Awake()
        {
            //set color
            this.GetComponent<Image>().color = CustomColor.GetColor(_color);

            //assign name and color of text
            _name = this.transform.FindChild(this.transform.name + " Text").GetComponent<Text>();
            _name.text = this.transform.name;
            _name.color = CustomColor.GetColor(_color);

            _greyTab = this.transform.FindChild(this.transform.name + " Cover").gameObject;
        }

        public Text Label
        {
            get { return _name; }
        }

        public GameObject GreyTab
        {
            get { return _greyTab; }
        }

        public ColorElement Color
        {
            get { return _color; }
        }
    }
}
