using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsTabDebug : SettingsTab
	{
		public SettingsTabDebug(Configurations settings) : base(settings, Keyed.TabLabelDebug, new List<ISettingHandle> { settings.DevMode }) { }

		public override void DoTabContents(Rect inRect)
		{
			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(inRect);
			listmain.CheckboxLabeled(Keyed.Option_Debug_Label, settings.DevMode, Keyed.Option_Debug_Desc);

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				Reset();
			}
			listmain.End();
		}
	}
}
