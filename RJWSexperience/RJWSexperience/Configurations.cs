using RimWorld;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	public class Configurations : ModSettings
	{
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
		public const bool DebugDefault = false;

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
		private bool debug = DebugDefault;

		public float MaxLustDeviation { get => maxLustDeviation; }
		public float AvgLust { get => avgLust; }
		public float MaxSexCountDeviation { get => maxSexCountDeviation; }
		public float LustEffectPower { get => lustEffectPower; }
		public float SexPerYear { get => sexPerYear; }
		public bool SlavesBeenRapedExp { get => slavesBeenRapedExp; }
		public bool EnableRecordRandomizer { get => enableRecordRandomizer; }
		public bool EnableBastardRelation { get => enableBastardRelation; }
		public float LustLimit { get => lustLimit; }
		public bool MinSexableFromLifestage { get => minSexableFromLifestage; }
		public float MinSexablePercent { get => minSexablePercent; }
		public float VirginRatio { get => virginRatio; }
		public float MaxSingleLustChange { get => maxSingleLustChange; }
		public bool Debug { get => debug; }

		private bool selectionLocked = false;

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
			debug = DebugDefault;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look(ref maxLustDeviation, "MaxLustDeviation", MaxInitialLustDefault, true);
			Scribe_Values.Look(ref avgLust, "AvgLust", AvgLustDefault, true);
			Scribe_Values.Look(ref maxSexCountDeviation, "MaxSexCountDeviation", MaxSexCountDeviationDefault, true);
			Scribe_Values.Look(ref lustEffectPower, "LustEffectPower", LustEffectPowerDefault, true);
			Scribe_Values.Look(ref sexPerYear, "SexPerYear", SexPerYearDefault, true);
			Scribe_Values.Look(ref slavesBeenRapedExp, "SlavesBeenRapedExp", SlavesBeenRapedExpDefault, true);
			Scribe_Values.Look(ref enableRecordRandomizer, "EnableRecordRandomizer", EnableStatRandomizerDefault, true);
			Scribe_Values.Look(ref enableBastardRelation, "EnableBastardRelation", EnableBastardRelationDefault, true);
			Scribe_Values.Look(ref lustLimit, "LustLimit", LustLimitDefault, true);
			Scribe_Values.Look(ref maxSingleLustChange, "maxSingleLustChange", MaxSingleLustChangeDefault, true);
			Scribe_Values.Look(ref minSexableFromLifestage, "MinSexableFromLifestage", MinSexableFromLifestageDefault, true);
			Scribe_Values.Look(ref minSexablePercent, "MinSexablePercent", MinSexablePercentDefault, true);
			Scribe_Values.Look(ref virginRatio, "VirginRatio", VirginRatioDefault, true);
			Scribe_Values.Look(ref debug, "Debug", DebugDefault, true);
			Scribe_Values.Look(ref selectionLocked, "SelectionLocked");
			base.ExposeData();
		}

		public void DoSettingsWindowContents(Rect inRect)
		{
			const float lineHeight = 24f;

			Listing_Standard listmain = new Listing_Standard();
			listmain.maxOneColumn = true;
			listmain.Begin(inRect);

			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_2_Label + " x" + lustEffectPower, Keyed.Option_2_Desc, ref lustEffectPower, 0f, 2f, 0.001f);
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
					SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_9_Label + " " + minSexablePercent * 100 + "%   " + ThingDefOf.Human.race.lifeExpectancy * minSexablePercent + " human years", Keyed.Option_9_Desc, ref minSexablePercent, 0, 1, 0.001f);

				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_10_Label + " " + virginRatio * 100 + "%", Keyed.Option_10_Desc, ref virginRatio, 0, 1, 0.001f);

				section.CheckboxLabeled(Keyed.Option_7_Label, ref slavesBeenRapedExp, Keyed.Option_7_Desc);

				listmain.EndSection(section);
			}

			listmain.CheckboxLabeled(Keyed.Option_EnableBastardRelation_Label, ref enableBastardRelation, Keyed.Option_EnableBastardRelation_Desc);
			listmain.CheckboxLabeled(Keyed.Option_Debug_Label, ref debug, Keyed.Option_Debug_Desc);

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
			LabelwithTextfield(doublerect.TopHalf(), label, tooltip, ref value, min, max);
			value = Widgets.HorizontalSlider(doublerect.BottomHalf(), value, min, max, roundTo: roundTo);
		}
	}
}
