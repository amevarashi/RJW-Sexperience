using RimWorld;
using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsTabHistory : IExposable, IResettable, ITab
	{
		public string Label => Keyed.TabLabelHistory;

		// Defaults
		public const bool EnableStatRandomizerDefault = true;
		public const float MaxLustDeviationDefault = 400f;
		public const float AvgLustDefault = 0f;
		public const float MaxSexCountDeviationDefault = 90f;
		public const float SexPerYearDefault = 30f;
		public const bool MinSexableFromLifestageDefault = true;
		public const float MinSexablePercentDefault = 0.2f;
		public const float VirginRatioDefault = 0.01f;
		public const bool SlavesBeenRapedExpDefault = true;

		// Private attributes
		private bool enableRecordRandomizer = EnableStatRandomizerDefault;
		private float maxLustDeviation = MaxLustDeviationDefault;
		private float avgLust = AvgLustDefault;
		private float maxSexCountDeviation = MaxSexCountDeviationDefault;
		private float sexPerYear = SexPerYearDefault;
		private bool minSexableFromLifestage = MinSexableFromLifestageDefault;
		private float minSexablePercent = MinSexablePercentDefault;
		private float virginRatio = VirginRatioDefault;
		private bool slavesBeenRapedExp = SlavesBeenRapedExpDefault;

		//Public read-only properties
		public bool EnableRecordRandomizer => enableRecordRandomizer;
		public float MaxLustDeviation => maxLustDeviation;
		public float AvgLust => avgLust;
		public float MaxSexCountDeviation => maxSexCountDeviation;
		public float SexPerYear => sexPerYear;
		public bool MinSexableFromLifestage => minSexableFromLifestage;
		public float MinSexablePercent => minSexablePercent;
		public float VirginRatio => virginRatio;
		public bool SlavesBeenRapedExp => slavesBeenRapedExp;

		public static SettingsTabHistory CreateDefault()
		{
			SettingsTabHistory history = new SettingsTabHistory();
			history.Reset();
			return history;
		}

		public void Reset()
		{
			enableRecordRandomizer = EnableStatRandomizerDefault;
			maxLustDeviation = MaxLustDeviationDefault;
			avgLust = AvgLustDefault;
			maxSexCountDeviation = MaxSexCountDeviationDefault;
			sexPerYear = SexPerYearDefault;
			minSexableFromLifestage = MinSexableFromLifestageDefault;
			minSexablePercent = MinSexablePercentDefault;
			virginRatio = VirginRatioDefault;
			slavesBeenRapedExp = SlavesBeenRapedExpDefault;
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref enableRecordRandomizer, "EnableRecordRandomizer", EnableStatRandomizerDefault);
			Scribe_Values.Look(ref maxLustDeviation, "MaxLustDeviation", MaxLustDeviationDefault);
			Scribe_Values.Look(ref avgLust, "AvgLust", AvgLustDefault);
			Scribe_Values.Look(ref maxSexCountDeviation, "MaxSexCountDeviation", MaxSexCountDeviationDefault);
			Scribe_Values.Look(ref sexPerYear, "SexPerYear", SexPerYearDefault);
			Scribe_Values.Look(ref minSexableFromLifestage, "MinSexableFromLifestage", MinSexableFromLifestageDefault);
			Scribe_Values.Look(ref minSexablePercent, "MinSexablePercent", MinSexablePercentDefault);
			Scribe_Values.Look(ref virginRatio, "VirginRatio", VirginRatioDefault);
			Scribe_Values.Look(ref slavesBeenRapedExp, "SlavesBeenRapedExp", SlavesBeenRapedExpDefault);
		}

		public void DoTabContents(Rect inRect)
		{
			const float lineHeight = SettingsWidgets.lineHeight;

			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(inRect);

			listmain.CheckboxLabeled(Keyed.Option_1_Label, ref enableRecordRandomizer, Keyed.Option_1_Desc);
			if (enableRecordRandomizer)
			{
				float sectionHeight = 12f;
				if (!minSexableFromLifestage)
					sectionHeight += 2f;

				Listing_Standard section = listmain.BeginSection(lineHeight * sectionHeight);

				SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_3_Label + " " + maxLustDeviation, Keyed.Option_3_Desc, ref maxLustDeviation, 0f, 2000f, 1f);
				SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_4_Label + " " + avgLust, Keyed.Option_4_Desc, ref avgLust, -1000f, 1000f, 1f);
				SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_5_Label + " " + maxSexCountDeviation, Keyed.Option_5_Desc, ref maxSexCountDeviation, 0f, 2000f, 1f);
				SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), Keyed.Option_6_Label + " " + sexPerYear, Keyed.Option_6_Desc, ref sexPerYear, 0f, 2000f, 1f);

				section.CheckboxLabeled(Keyed.Option_MinSexableFromLifestage_Label, ref minSexableFromLifestage, Keyed.Option_MinSexableFromLifestage_Desc);

				if (!minSexableFromLifestage)
					SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), $"{Keyed.Option_9_Label} {minSexablePercent:P1}   {ThingDefOf.Human.race.lifeExpectancy * minSexablePercent} human years", Keyed.Option_9_Desc, ref minSexablePercent, 0, 1, 0.001f);

				SettingsWidgets.SliderOption(section.GetRect(lineHeight * 2f), $"{Keyed.Option_10_Label} {virginRatio:P1}", Keyed.Option_10_Desc, ref virginRatio, 0f, 1f, 0.001f);
				section.CheckboxLabeled(Keyed.Option_7_Label, ref slavesBeenRapedExp, Keyed.Option_7_Desc);

				listmain.EndSection(section);
			}

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				Reset();
			}
			listmain.End();
		}
	}
}
