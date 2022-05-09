using UnityEngine;
using System.Collections.Generic;
using Verse;
using RJWSexperience.Settings;

namespace RJWSexperience
{
	public class SexperienceMod : Mod
	{
		private static Configurations settings;
		public static Configurations Settings { get => settings; }

		public ITab CurrentTab { get; private set; }

		private readonly List<TabRecord> tabRecords;

		public SexperienceMod(ModContentPack content) : base(content)
		{
			settings = GetSettings<Configurations>();
			CurrentTab = settings;
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
				settings,
				settings.History,
				settings.Debug
			};

			foreach (ITab tab in tabs)
				tabRecords.Add(new TabRecord(tab.Label, delegate { this.CurrentTab = tab; }, delegate { return this?.CurrentTab == tab; }));
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			if (tabRecords.NullOrEmpty())
				InitTabRecords();

			Rect contentRect = inRect.ContractedBy(TabDrawer.TabHeight);

			_ = TabDrawer.DrawTabs(contentRect, tabRecords);

			CurrentTab.DoTabContents(contentRect);
		}
	}
}
