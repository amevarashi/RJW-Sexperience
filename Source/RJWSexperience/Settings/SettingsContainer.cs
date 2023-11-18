using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Settings
{
	public class SettingsContainer : IExposable
	{
		public List<ISettingHandle> Handles { get; }

		public SettingsContainer(List<ISettingHandle> handles)
		{
			Handles = handles;
		}

		public void ExposeData()
		{
			foreach (ISettingHandle setting in Handles)
			{
				setting.Scribe();
			}
		}

		public static SettingsContainer CreateHistoryContainer(Configurations settings) => new SettingsContainer(new List<ISettingHandle> {
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
			);
	}
}