using UnityEngine;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsTabDebug : IExposable, IResettable, ITab
	{
		public string Label => Keyed.TabLabelDebug;

		// Defaults
		public const bool DevModeDefault = false;

		// Private attributes
		private bool devMode;

		//Public read-only properties
		public bool DevMode => devMode;

		public void Reset()
		{
			devMode = DevModeDefault;
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref devMode, "DevMode", DevModeDefault);
		}

		public void DoTabContents(Rect inRect)
		{
			Listing_Standard listmain = new Listing_Standard();
			listmain.Begin(inRect);
			listmain.CheckboxLabeled(Keyed.Option_Debug_Label, ref devMode, Keyed.Option_Debug_Desc);

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				Reset();
			}
			listmain.End();
		}
	}
}
