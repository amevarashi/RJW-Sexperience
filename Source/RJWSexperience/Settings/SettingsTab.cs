using System.Collections.Generic;
using UnityEngine;

namespace RJWSexperience.Settings
{
	public abstract class SettingsTab : ITab, IResettable
	{
		protected readonly List<ISettingHandle> tabSettings;
		protected readonly Configurations settings;

		public string Label { get; protected set; }

		protected SettingsTab(Configurations settings, string label, List<ISettingHandle> tabSettings)
		{
			this.settings = settings;
			Label = label;
			this.tabSettings = tabSettings;
		}

		public void Reset()
		{
			foreach (ISettingHandle setting in tabSettings)
			{
				setting.Reset();
			}
		}

		public abstract void DoTabContents(Rect inRect);
	}
}