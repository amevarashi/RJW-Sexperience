using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public static class SettingsWidgets
	{
		public const float lineHeight = 24f;

		public static void LabelwithTextfield(Rect rect, string label, string tooltip, ref float value, float min, float max)
		{
			Rect textfieldRect = new Rect(rect.xMax - 100f, rect.y, 100f, rect.height);
			string valuestr = value.ToString();
			Widgets.Label(rect, label);
			Widgets.TextFieldNumeric(textfieldRect, ref value, ref valuestr, min, max);
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, tooltip);
		}

		public static void SliderOption(Rect doublerect, string label, string tooltip, ref float value, float min, float max, float roundTo)
		{
			// Slider was fighting with textfield for "correct" decimals. Causes a repeating slider move sound
			float fieldValue = value;
			float sliderValue = value;
			float minChange = roundTo / 10f;

			LabelwithTextfield(doublerect.TopHalf(), label, tooltip, ref fieldValue, min, max);
			sliderValue = Widgets.HorizontalSlider(doublerect.BottomHalf(), sliderValue, min, max, roundTo: roundTo);

			if (Mathf.Abs(fieldValue - value) > minChange)
				value = fieldValue;
			else if (Mathf.Abs(sliderValue - value) > minChange)
				value = sliderValue;
		}
	}
}
