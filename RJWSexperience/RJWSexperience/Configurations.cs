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
		public const float LustLimitDefault = MaxInitialLustDefault / 3f;
		public const float MaxSingleLustChangeDefault = 0.5f;
		public const bool MinSexableFromLifestageDefault = true;
		public const float MinSexablePercentDefault = 0.2f;
		public const float VirginRatioDefault = 0.01f;

		private float maxSingleLustChange = MaxSingleLustChangeDefault;
		private bool minSexableFromLifestage = MinSexableFromLifestageDefault;

		public static float MaxLustDeviation = MaxInitialLustDefault;
		public static float AvgLust = AvgLustDefault;
		public static float MaxSexCountDeviation = MaxSexCountDeviationDefault;
		public static float LustEffectPower = LustEffectPowerDefault;
		public static float SexPerYear = SexPerYearDefault;
		public static bool SlavesBeenRapedExp = SlavesBeenRapedExpDefault;
		public static bool EnableRecordRandomizer = EnableStatRandomizerDefault;
		public static float LustLimit = LustLimitDefault;
		public bool MinSexableFromLifestage { get => minSexableFromLifestage; }
		public static float MinSexablePercent = MinSexablePercentDefault;
		public static float VirginRatio = VirginRatioDefault;
		public float MaxSingleLustChange { get => maxSingleLustChange; }

		public static bool SelectionLocked = false;

		public void ResetToDefault()
		{
			MaxLustDeviation = MaxInitialLustDefault;
			AvgLust = AvgLustDefault;
			MaxSexCountDeviation = MaxSexCountDeviationDefault;
			LustEffectPower = LustEffectPowerDefault;
			SexPerYear = SexPerYearDefault;
			SlavesBeenRapedExp = SlavesBeenRapedExpDefault;
			EnableRecordRandomizer = EnableStatRandomizerDefault;
			LustLimit = LustLimitDefault;
			maxSingleLustChange = MaxSingleLustChangeDefault;
			minSexableFromLifestage = MinSexableFromLifestageDefault;
			MinSexablePercent = MinSexablePercentDefault;
			VirginRatio = VirginRatioDefault;
		}

		public override void ExposeData()
		{
			Scribe_Values.Look(ref MaxLustDeviation, "MaxLustDeviation", MaxInitialLustDefault, true);
			Scribe_Values.Look(ref AvgLust, "AvgLust", AvgLust, true);
			Scribe_Values.Look(ref MaxSexCountDeviation, "MaxSexCountDeviation", MaxSexCountDeviation, true);
			Scribe_Values.Look(ref LustEffectPower, "LustEffectPower", LustEffectPower, true);
			Scribe_Values.Look(ref SexPerYear, "SexPerYear", SexPerYear, true);
			Scribe_Values.Look(ref SlavesBeenRapedExp, "SlavesBeenRapedExp", SlavesBeenRapedExp, true);
			Scribe_Values.Look(ref EnableRecordRandomizer, "EnableRecordRandomizer", EnableRecordRandomizer, true);
			Scribe_Values.Look(ref LustLimit, "LustLimit", LustLimit, true);
			Scribe_Values.Look(ref maxSingleLustChange, "maxSingleLustChange", MaxSingleLustChangeDefault, true);
			Scribe_Values.Look(ref minSexableFromLifestage, "MinSexableFromLifestage", MinSexableFromLifestage, true);
			Scribe_Values.Look(ref MinSexablePercent, "MinSexablePercent", MinSexablePercent, true);
			Scribe_Values.Look(ref VirginRatio, "VirginRatio", VirginRatio, true);
			Scribe_Values.Look(ref SelectionLocked, "SelectionLocked", SelectionLocked, true);
			base.ExposeData();
		}

		public void DoSettingsWindowContents(Rect inRect)
		{
			const float lineHeight = 24f;

			Listing_Standard listmain = new Listing_Standard();
			listmain.maxOneColumn = true;
			listmain.Begin(inRect);

			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_2_Label + " x" + LustEffectPower, Keyed.Option_2_Desc, ref LustEffectPower, 0f, 2f, 0.001f);
			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_8_Label + " " + LustLimit, Keyed.Option_8_Desc, ref LustLimit, 0f, 5000f, 1f);
			SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_MaxSingleLustChange_Label + " " + maxSingleLustChange, Keyed.Option_MaxSingleLustChange_Desc, ref maxSingleLustChange, 0f, 10f, 0.05f);

			listmain.CheckboxLabeled(Keyed.Option_1_Label, ref EnableRecordRandomizer, Keyed.Option_1_Desc);
			if (EnableRecordRandomizer)
			{
				float sectionHeight = 12f;
				if (!MinSexableFromLifestage) sectionHeight += 2f;

				Listing_Standard section = listmain.BeginSection(lineHeight * sectionHeight);

				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_3_Label + " " + MaxLustDeviation, Keyed.Option_3_Desc, ref MaxLustDeviation, 0f, 2000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_4_Label + " " + AvgLust, Keyed.Option_4_Desc, ref AvgLust, -1000f, 1000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_5_Label + " " + MaxSexCountDeviation, Keyed.Option_5_Desc, ref MaxSexCountDeviation, 0f, 2000f, 1f);
				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_6_Label + " " + SexPerYear, Keyed.Option_6_Desc, ref SexPerYear, 0f, 2000f, 1f);

				section.CheckboxLabeled(Keyed.Option_MinSexableFromLifestage_Label, ref minSexableFromLifestage, Keyed.Option_MinSexableFromLifestage_Desc);

				if (!MinSexableFromLifestage)
					SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_9_Label + " " + MinSexablePercent * 100 + "%   " + ThingDefOf.Human.race.lifeExpectancy * MinSexablePercent + " human years", Keyed.Option_9_Desc, ref MinSexablePercent, 0, 1, 0.001f);

				SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_10_Label + " " + VirginRatio * 100 + "%", Keyed.Option_10_Desc, ref VirginRatio, 0, 1, 0.001f);

				section.CheckboxLabeled(Keyed.Option_7_Label, ref SlavesBeenRapedExp, Keyed.Option_7_Desc);

				listmain.EndSection(section);
			}

			if (listmain.ButtonText("reset to default"))
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

	public class SexperienceMod : Mod
	{
		private static Configurations settings;
		public static Configurations Settings { get => settings; }

		public SexperienceMod(ModContentPack content) : base(content)
		{
			settings = GetSettings<Configurations>();
		}

		public override string SettingsCategory()
		{
			return Keyed.Mod_Title;
		}

		public override void DoSettingsWindowContents(Rect inRect) => Settings.DoSettingsWindowContents(inRect);
	}
}
