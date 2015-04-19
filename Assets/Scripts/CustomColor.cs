using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    public static class CustomColor
    {
        //get color based on enum element
        public static Color GetColor(ColorElement _color)
        {
            switch (_color)
            {
                case ColorElement.Red:
                    return new Color(1f, 0f, 0f);

                case ColorElement.Green:
                    return new Color(0f, 1f, 0f);

                case ColorElement.Blue:
                    return new Color(0f, 0f, 1f);

                case ColorElement.Yellow:
                    return new Color(1f, 1f, 0f);

                case ColorElement.Cyan:
                    return new Color(0f, 1f, 1f);

                case ColorElement.Magenta:
                    return new Color(1f, 0f, 1f);

                case ColorElement.Orange:
                    return new Color(1f, 0.5f, 0f);

                case ColorElement.Chartreuse:
                    return new Color(0.5f, 1f, 0f);

                case ColorElement.Spring:
                    return new Color(0f, 1f, 0.5f);

                case ColorElement.Azure:
                    return new Color(0f, 0.5f, 1f);

                case ColorElement.Rose:
                    return new Color(1f, 0f, 0.5f);

                case ColorElement.Violet:
                    return new Color(0.5f, 0f, 1f);

                case ColorElement.White:
                    return new Color(1f, 1f, 1f);

                case ColorElement.Black:
                    return new Color(0f, 0f, 0f);
            }
            return new Color(0f, 0f, 0f);
        }


		//for determining weights of color
		public static ColorElement Weights(ColorElement _color)
		{
			float _rand = 0;
			switch (_color)
			{
			case ColorElement.Red:
				return ColorElement.Red;
				
			case ColorElement.Green:
				return ColorElement.Green;
				
			case ColorElement.Blue:
				return ColorElement.Blue;
				
			case ColorElement.Yellow:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.5f ? ColorElement.Red : ColorElement.Green);

			case ColorElement.Cyan:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.5f ? ColorElement.Green : ColorElement.Blue);
				
			case ColorElement.Magenta:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.5f ? ColorElement.Red : ColorElement.Blue);
				
			case ColorElement.Orange:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.25f ? ColorElement.Red : ColorElement.Green);
				
			case ColorElement.Chartreuse:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.75f ? ColorElement.Red : ColorElement.Green);
				
			case ColorElement.Spring:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.25f ? ColorElement.Green : ColorElement.Blue);
				
			case ColorElement.Azure:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.75f ? ColorElement.Green : ColorElement.Blue);
				
			case ColorElement.Rose:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.25f ? ColorElement.Red : ColorElement.Blue);

			case ColorElement.Violet:
				_rand = Random.Range(0f, 1f);
				return (_rand >= 0.75f ? ColorElement.Red : ColorElement.Blue);
			}
				
			//case ColorElement.White:
				//return ColorElement.White;
			//}
			_rand = Random.Range(0, 1.5f);
			if(_rand < 0.5f) return ColorElement.Red;
			else if(_rand < 1f) return ColorElement.Green;
			else return ColorElement.Blue;
		}

        //converting colors if using 0-255 instead of 0-1
        public static Color ConvertColor(float r, float g, float b)
        {
            Color color = new Color(r / 255, g / 255, b / 255);
            return color;
        }

        public static Color ConvertColor(float r, float g, float b, float a)
        {
            Color color = new Color(r / 255, g / 255, b / 255, a / 255);
            return color;
        }

        public static Color ConvertColor(Color _color)
        {
            Color color = new Color(_color.r / 255, _color.g / 255, _color.b / 255, _color.a / 255);
            return color;
        }
    }
}