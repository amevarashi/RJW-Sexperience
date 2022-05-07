using RimWorld;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	public class Configurations : ModSettings, Settings.ITab
	{
		public string Label => Keyed.TabLabelMain;

		// Defaults
		public const float MaxInitialLustDefault = 400f;
		public const float AvgLustDefault = 0f;
		public const float MaxSexCountDeviationDefault = 90f;
		public const float LustEffectPowerDefault = 0.5f;
		public const float SexPerYearDefault = 30f;
		public const bool SlavesBeenRapedExpDefault = true;
		public const bool EnableStatRandomizerDefault = true;
		public const bool EnableBastardRelationDefault = true;
		public const float LustLimitDefault = MaxInitialLustDefault / 3f;
		public const float MaxSingleLustChangeDefault = 0.5f;
		public const bool MinSexableFromLifestageDefault = true;
		public const float MinSexablePercentDefault = 0.2f;
		public const float VirginRatioDefault = 0.01f;
		public const bool SexCanFillBucketsDefault = false;
		public const bool selectionLockedDefault = false;

		// Private attributes
		private float maxLustDeviation = MaxInitialLustDefault;
		private float avgLust = AvgLustDefault;
		private float maxSexCountDeviation = MaxSexCountDeviationDefault;
		private float lustEffectPower = LustEffectPowerDefault;
		private float sexPerYear = SexPerYearDefault;
		private bool slavesBeenRapedExp = SlavesBeenRapedExpDefault;
		private bool enableRecordRandomizer = EnableStatRandomizerDefault;
		private bool enableBastardRelation = EnableBastardRelationDefault;
		private float lustLimit = LustLimitDefault;
		private bool minSexableFromLifestage = MinSexableFromLifestageDefault;
		private float minSexablePercent = MinSexablePercentDefault;
		private float virginRatio = VirginRatioDefault;
		private float maxSingleLustChange = MaxSingleLustChangeDefault;
		private bool sexCanFillBuckets = SexCanFillBucketsDefault;
		private bool selectionLocked = selectionLockedDefault;
		private Settings.SettingsTabDebug debug;

		//Public read-only properties
		public float MaxLustDeviation => maxLustDeviation;
		public float AvgLust => avgLust;
		public float MaxSexCountDeviation => maxSexCountDeviation;
		public float LustEffectPower => lustEffectPower;
		public float SexPerYear => sexPerYear;
		public bool SlavesBeenRapedExp => slavesBeenRapedExp;
		public bool EnableRecordRandomizer => enableRecordRandomizer;
		public bool EnableBastardRelation => enableBastardRelation;
		public float LustLimit => lustLimit;
		public bool MinSexableFromLifestage => minSexableFromLifestage;
		public float MinSexablePercent => minSexablePercent;
		public float VirginRatio => virginRatio;
		public float MaxSingleLustChange => maxSingleLustChange;
		public bool SexCanFillBuckets => sexCanFillBuckets;
		public Settings.SettingsTabDebug Debug => debug;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2292:Trivial properties should be auto-implemented", Justification = "Can't scribe property")]
		public bool SelectionLocked { get => selectionLocked; set => selectionLocked = value; }

		public void ResetToDefault()
		{
			maxLustDeviation = MaxInitialLustDefault;
			avgLust = AvgLustDefault;
			maxSexCountDeviation = MaxSexCountDeviationDefault;
			lustEffectPower = LustEffectPowerDefault;
			sexPerYear = SexPerYearDefault;
			slavesBeenRapedExp = SlavesBeenRapedExpDefault;
			enableRecordRandomizer = EnableStatRandomizerDefault;
			enableBastardRelation = EnableBastardRelationDefault;
			lustLimit = LustLimitDefault;
			maxSingleLustChange = MaxSingleLustChangeDefault;
			minSexableFromLifestage = MinSexableFromLifestageDefault;
			minSexablePercent = MinSexablePercentDefault;
			virginRatio = VirginRatioDefault;
            sexCanFillBuckets = SexCanFillBucketsDefault;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look(ref maxLustDeviation, "MaxLustDeviation", MaxInitialLustDefault);
			Scribe_Values.Look(ref avgLust, "AvgLust", AvgLustDefault);
			Scribe_Values.Look(ref maxSexCountDeviation, "MaxSexCountDeviation", MaxSexCountDeviationDefault);
			Scribe_Values.Look(ref lustEffectPower, "LustEffectPower", LustEffectPowerDefault);
			Scribe_Values.Look(ref sexPerYear, "SexPerYear", SexPerYearDefault);
			Scribe_Values.Look(ref slavesBeenRapedExp, "SlavesBeenRapedExp", SlavesBeenRapedExpDefault);
			Scribe_Values.Look(ref enableRecordRandomizer, "EnableRecordRandomizer", EnableStatRandomizerDefault);
			Scribe_Values.Look(ref enableBastardRelation, "EnableBastardRelation", EnableBastardRelationDefault);
			Scribe_Values.Look(ref lustLimit, "LustLimit", LustLimitDefault);
			Scribe_Values.Look(ref maxSingleLustChange, "maxSingleLustChange", MaxSingleLustChangeDefault);
			Scribe_Values.Look(ref minSexableFromLifestage, "MinSexableFromLifestage", MinSexableFromLifestageDefault);
			Scribe_Values.Look(ref minSexablePercent, "MinSexablePercent", MinSexablePercentDefault);
			Scribe_Values.Look(ref virginRatio, "VirginRatio", VirginRatioDefault);
			Scribe_Values.Look(ref selectionLocked, "SelectionLocked", selectionLockedDefault);
			Scribe_Values.Look(ref sexCanFillBuckets, "SexCanFillBuckets", SexCanFillBucketsDefault);
			Scribe_Deep.Look(ref debug, "Debug");
			base.ExposeData();

			if (Scribe.mode != LoadSaveMode.LoadingVars)
				return;

			if (debug == null)
			{
				debug = new Settings.SettingsTabDebug();
				debug.Reset();
			}
		}

		public void DoTabContents(Rect inRect)
		{
			const float lineHeight = 24f;

			Listing_Standard listmain = new Listing_Standard();
			listmain.maxOneColumn = true;
			listmain.Begin(inRect);

			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_2_Label + " x" + lustEffectPower, Keyed.Option_2_Desc, ref lustEffectPower, 0f, 2f, 0.01f);
			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_8_Label + " " + lustLimit, Keyed.Option_8_Desc, ref lustLimit, 0f, 5000f, 1f);
			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_MaxSingleLustChange_Label + " " + maxSingleLustChange, Keyed.Option_MaxSingleLustChange_Desc, ref maxSingleLustChange, 0f, 10f, 0.05f);

			listmain.CheckboxLabeled(Keyed.Option_1_Label, ref enableRecordRandomizer, Keyed.Option_1_Desc);
			if (enableRecordRandomizer)
			{
				float sectionHeight = 12f;
				if (!minSexableFromLifestage) sectionHeight += 2f;

				Listing_Standard section = listmain.BeginSection(lineHeight * sectionHeight);

				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_3_Label + " " + maxLustDeviation, Keyed.Option_3_Desc, ref maxLustDeviation, 0f, 2000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_4_Label + " " + avgLust, Keyed.Option_4_Desc, ref avgLust, -1000f, 1000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_5_Label + " " + maxSexCountDeviation, Keyed.Option_5_Desc, ref maxSexCountDeviation, 0f, 2000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_6_Label + " " + sexPerYear, Keyed.Option_6_Desc, ref sexPerYear, 0f, 2000f, 1f);

				section.CheckboxLabeled(Keyed.Option_MinSexableFromLifestage_Label, ref minSexableFromLifestage, Keyed.Option_MinSexableFromLifestage_Desc);

				if (!minSexableFromLifestage)
					SliderOption(section.GetRect(lineHeight * 2f), $"{Keyed.Option_9_Label} {minSexablePercent:P1}   {ThingDefOf.Human.race.lifeExpectancy * minSexablePercent} human years", Keyed.Option_9_Desc, ref minSexablePercent, 0, 1, 0.001f);

				SliderOption(section.GetRect(lineHeight * 2f), $"{Keyed.Option_10_Label} {virginRatio:P1}", Keyed.Option_10_Desc, ref virginRatio, 0f, 1f, 0.001f);

				section.CheckboxLabeled(Keyed.Option_7_Label, ref slavesBeenRapedExp, Keyed.Option_7_Desc);

				listmain.EndSection(section);
			}

			listmain.CheckboxLabeled(Keyed.Option_EnableBastardRelation_Label, ref enableBastardRelation, Keyed.Option_EnableBastardRelation_Desc);
			listmain.CheckboxLabeled(Keyed.Option_SexCanFillBuckets_Label, ref sexCanFillBuckets, Keyed.Option_SexCanFillBuckets_Desc);

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				ResetToDefault();
			}
			listmain.End();
		}

		private void LabelwithTextfield(Rect rect, string label, string tooltip, ref float value, float min, float max)
		{
			Rect textfieldRect = new Rect(rect.xMax - 100f, rect.y, 100f, rect.height);
			string valuestr = value.ToString();
			Widgets.Label(rect, label);
			Widgets.TextFieldNumeric(textfieldRect, ref value, ref valuestr, min, max);
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, tooltip);
		}

		private void SliderOption(Rect doublerect, string label, string tooltip, ref float value, float min, float max, float roundTo = -1f)
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
