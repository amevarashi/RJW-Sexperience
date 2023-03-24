using UnityEngine;
using System.Collections.Generic;
using Verse;
using RJWSexperience.Settings;

namespace RJWSexperience
{
	public class SexperienceMod : Mod
	{
		public static Configurations Settings { get; private set; }

		public ITab CurrentTab { get; private set; }

		private readonly List<TabRecord> tabRecords;

		public SexperienceMod(ModContentPack content) : base(content)
		{
			Settings = GetSettings<Configurations>();
			tabRecords = new List<TabRecord>();
		}

		public override string SettingsCategory()
		{
			return Keyed.Mod_Title;
		}

		/// <summary>
		/// Fills tabRecords list.
		/// This method cannot be called in constructor because at that stage language file is not loaded
		/// </summary>
		private void InitTabRecords()
		{
			List<ITab> tabs = new List<ITab>
			{
				new SettingsTabMain(Settings),
				new SettingsTabHistory(Settings),
				new SettingsTabDebug(Settings),
			};

			foreach (ITab tab in tabs)
				tabRecords.Add(new TabRecord(tab.Label, delegate { this.CurrentTab = tab; }, delegate { return this?.CurrentTab == tab; }));

			CurrentTab = tabs[0];
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			if (tabRecords.NullOrEmpty())
				InitTabRecords();

			Rect contentRect = inRect.BottomPartPixels(inRect.height - TabDrawer.TabHeight);

			_ = TabDrawer.DrawTabs(contentRect, tabRecords);

			CurrentTab.DoTabContents(contentRect);
		}
	}
}
