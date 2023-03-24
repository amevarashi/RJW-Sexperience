using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public static class SettingsWidgets
	{
		public const float lineHeight = 24f;

		public static void LabelwithTextfield(Rect rect, string label, string tooltip, ref float value, FloatRange range)
		{
			Rect textfieldRect = new Rect(rect.xMax - 100f, rect.y, 100f, rect.height);
			string valuestr = value.ToString();
			Widgets.Label(rect, label);
			Widgets.TextFieldNumeric(textfieldRect, ref value, ref valuestr, range.TrueMin, range.TrueMax);
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, tooltip);
		}

		public static void SliderOption(this Listing_Standard outList, string label, string tooltip, SettingHandle<float> handle, FloatRange range, float roundTo)
		{
			// Slider was fighting with textfield for "correct" decimals. Causes a repeating slider move sound
			float fieldValue = handle.Value;
			float sliderValue = handle.Value;
			float minChange = roundTo / 10f;

			string formattedLabel = string.Format(label, handle.Value);

			LabelwithTextfield(outList.GetRect(lineHeight), formattedLabel, tooltip, ref fieldValue, range);
			sliderValue = Widgets.HorizontalSlider_NewTemp(outList.GetRect(lineHeight), sliderValue, range.TrueMin, range.TrueMax, roundTo: roundTo);

			if (Mathf.Abs(fieldValue - handle.Value) > minChange)
				handle.Value = fieldValue;
			else
				handle.Value = sliderValue;
		}

		public static void CheckboxLabeled(this Listing_Standard outList, string label, SettingHandle<bool> handle, string tooltip)
		{
			bool value = handle.Value;
			outList.CheckboxLabeled(label, ref value, tooltip);
			handle.Value = value;
		}
	}
}
