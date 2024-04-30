using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsTabHistory : SettingsTab
	{
		public SettingsTabHistory(Configurations settings) : base(
			settings,
			Keyed.TabLabelHistory,
			new List<ISettingHandle> {
				settings.EnableRecordRandomizer,
				settings.MaxLustDeviation,
				settings.AvgLust,
				settings.MaxSexCountDeviation,
				settings.SexPerYear,
				settings.MinSexableFromLifestage,
				settings.MinSexablePercent,
				settings.VirginRatio,
				settings.SlavesBeenRapedExp,
				settings.EnableSexHistory,
				settings.HideGizmoWhenDrafted,
				settings.VirginityCheck_M2M_Anal,
				settings.VirginityCheck_F2F_Scissoring
			}
		) { }

		public override void DoTabContents(Rect inRect)
		{
			const float lineHeight = SettingsWidgets.lineHeight;

			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(inRect);

			listmain.CheckboxLabeled(Keyed.Option_1_Label, settings.EnableRecordRandomizer, Keyed.Option_1_Desc);
			if (settings.EnableRecordRandomizer)
			{
				float sectionHeight = 12f;
				if (!settings.MinSexableFromLifestage)
					sectionHeight += 2f;

				Listing_Standard section = listmain.BeginSection(lineHeight * sectionHeight);

				section.SliderOption(Keyed.Option_3_Label + " {0}", Keyed.Option_3_Desc, settings.MaxLustDeviation, new FloatRange(0f, 1000f), 1f);
				section.SliderOption(Keyed.Option_4_Label + " {0}", Keyed.Option_4_Desc, settings.AvgLust, new FloatRange(-200f, 200f), 1f);
				section.SliderOption(Keyed.Option_5_Label + " {0}", Keyed.Option_5_Desc, settings.MaxSexCountDeviation, new FloatRange(0f, 1000f), 1f);
				section.SliderOption(Keyed.Option_6_Label + " {0}", Keyed.Option_6_Desc, settings.SexPerYear, new FloatRange(0f, 2000f), 1f);

				section.CheckboxLabeled(Keyed.Option_MinSexableFromLifestage_Label, settings.MinSexableFromLifestage, Keyed.Option_MinSexableFromLifestage_Desc);

				if (!settings.MinSexableFromLifestage)
					section.SliderOption($"{Keyed.Option_9_Label} {{0:P1}}  {ThingDefOf.Human.race.lifeExpectancy * settings.MinSexablePercent} human years", Keyed.Option_9_Desc, settings.MinSexablePercent, FloatRange.ZeroToOne, 0.001f);

				section.SliderOption(Keyed.Option_10_Label + " {0:P1}", Keyed.Option_10_Desc, settings.VirginRatio, FloatRange.ZeroToOne, 0.001f);
				section.CheckboxLabeled(Keyed.Option_7_Label, settings.SlavesBeenRapedExp, Keyed.Option_7_Desc);

				listmain.EndSection(section);
			}

			listmain.CheckboxLabeled(Keyed.Option_EnableSexHistory_Label, settings.EnableSexHistory, Keyed.Option_EnableSexHistory_Desc);

			if (settings.EnableSexHistory)
			{
				listmain.CheckboxLabeled(Keyed.Option_HideGizmoWhenDrafted_Label, settings.HideGizmoWhenDrafted, Keyed.Option_HideGizmoWhenDrafted_Desc);
			}
			
			listmain.CheckboxLabeled(Keyed.Option_VirginityCheck_M2M_Label, settings.VirginityCheck_M2M_Anal, Keyed.Option_VirginityCheck_M2M_Desc);
			listmain.CheckboxLabeled(Keyed.Option_VirginityCheck_F2F_Label, settings.VirginityCheck_F2F_Scissoring, Keyed.Option_VirginityCheck_F2F_Label);

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				Reset();
			}
			listmain.End();
		}
	}
}
