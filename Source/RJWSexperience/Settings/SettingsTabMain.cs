using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsTabMain : SettingsTab
	{
		public SettingsTabMain(Configurations settings) : base(
			settings,
			Keyed.TabLabelMain,
			new List<ISettingHandle> {
				settings.LustEffectPower,
				settings.EnableBastardRelation,
				settings.LustLimit,
				settings.MaxSingleLustChange,
				settings.SexCanFillBuckets,
				settings.VirginityCheck_M2M_Anal,
				settings.VirginityCheck_F2F_Scissoring
				}
			) { }

		public override void DoTabContents(Rect inRect)
		{
			Listing_Standard listmain = new Listing_Standard { maxOneColumn = true };
			listmain.Begin(inRect);
			listmain.SliderOption(Keyed.Option_2_Label + " x{0}", Keyed.Option_2_Desc, settings.LustEffectPower, new FloatRange(0f, 2f), 0.01f);
			listmain.SliderOption(Keyed.Option_8_Label + " {0}", Keyed.Option_8_Desc, settings.LustLimit, new FloatRange(0f, 500f), 1f);
			listmain.SliderOption(Keyed.Option_MaxSingleLustChange_Label + " {0}", Keyed.Option_MaxSingleLustChange_Desc, settings.MaxSingleLustChange, new FloatRange(0f, 10f), 0.05f);
			listmain.CheckboxLabeled(Keyed.Option_EnableBastardRelation_Label, settings.EnableBastardRelation, Keyed.Option_EnableBastardRelation_Desc);
			listmain.CheckboxLabeled(Keyed.Option_SexCanFillBuckets_Label, settings.SexCanFillBuckets, Keyed.Option_SexCanFillBuckets_Desc);
			listmain.CheckboxLabeled(Keyed.Option_VirginityCheck_M2M_Label, settings.VirginityCheck_M2M_Anal, Keyed.Option_VirginityCheck_M2M_Desc);
			listmain.CheckboxLabeled(Keyed.Option_VirginityCheck_F2F_Label, settings.VirginityCheck_F2F_Scissoring, Keyed.Option_VirginityCheck_F2F_Desc);

			if (settings.DevMode)
				LustUtility.DrawGraph(listmain.GetRect(300f));

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				Reset();
			}
			listmain.End();
		}
	}
}
