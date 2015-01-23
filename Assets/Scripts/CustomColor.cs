using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public static class CustomColor
{
	//get color based on enum element
	public static Color GetColor(ColorElement _color)
	{
		switch(_color)
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

	//converting colors if using 0-255 instead of 0-1
	public static Color ConvertColor(float r, float g, float b){
		Color color = new Color(r/255, g/255, b/255);
		return color;
	}

	public static Color ConvertColor(float r, float g, float b, float a){
		Color color = new Color(r/255, g/255, b/255, a/255);
		return color;
	}

	public static Color ConvertColor(Color _color){
		Color color = new Color(_color.r/255, _color.g/255, _color.b/255, _color.a/255);
		return color;
	}
}