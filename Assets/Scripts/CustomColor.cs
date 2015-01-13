using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

public static class CustomColor
{

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
}