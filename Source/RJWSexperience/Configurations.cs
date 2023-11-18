using Verse;
using RJWSexperience.Settings;

namespace RJWSexperience
{
	public class Configurations : ModSettings
	{
		public const int CurrentSettingsVersion = 1;

		public readonly SettingHandle<float> LustEffectPower = new SettingHandle<float>("LustEffectPower", 0.5f);
		public readonly SettingHandle<bool> EnableBastardRelation = new SettingHandle<bool>("EnableBastardRelation", true);
		public readonly SettingHandle<float> LustLimit = new SettingHandle<float>("LustLimit", 150f);
		public readonly SettingHandle<float> MaxSingleLustChange = new SettingHandle<float>("maxSingleLustChange", 1f);
		public readonly SettingHandle<bool> SexCanFillBuckets = new SettingHandle<bool>("SexCanFillBuckets", false);

		public readonly SettingHandle<bool> EnableRecordRandomizer = new SettingHandle<bool>("EnableRecordRandomizer", true);
		public readonly SettingHandle<float> MaxLustDeviation = new SettingHandle<float>("MaxLustDeviation", 200f);
		public readonly SettingHandle<float> AvgLust = new SettingHandle<float>("AvgLust", 0f);
		public readonly SettingHandle<float> MaxSexCountDeviation = new SettingHandle<float>("MaxSexCountDeviation", 90f);
		public readonly SettingHandle<float> SexPerYear = new SettingHandle<float>("SexPerYear", 30f);
		public readonly SettingHandle<bool> MinSexableFromLifestage = new SettingHandle<bool>("MinSexableFromLifestage", true);
		public readonly SettingHandle<float> MinSexablePercent = new SettingHandle<float>("MinSexablePercent", 0.2f);
		public readonly SettingHandle<float> VirginRatio = new SettingHandle<float>("VirginRatio", 0.01f);
		public readonly SettingHandle<bool> SlavesBeenRapedExp = new SettingHandle<bool>("SlavesBeenRapedExp", true);
		public readonly SettingHandle<bool> EnableSexHistory = new SettingHandle<bool>("EnableSexHistory", true);
		public readonly SettingHandle<bool> HideGizmoWhenDrafted = new SettingHandle<bool>("HideGizmoWhenDrafted", true);

		public readonly SettingHandle<bool> VirginityCheck_M2M_Anal = new SettingHandle<bool>("VirginityCheck_M2M_Anal", true);
		public readonly SettingHandle<bool> VirginityCheck_F2F_Scissoring = new SettingHandle<bool>("VirginityCheck_F2F_Scissoring", false);
		
		public readonly SettingHandle<bool> DevMode = new SettingHandle<bool>("DevMode", false);

		public readonly SettingHandle<bool> SelectionLocked = new SettingHandle<bool>("SelectionLocked", false);

		public override void ExposeData()
		{
			SettingsContainer history = SettingsContainer.CreateHistoryContainer(this);
			int version = CurrentSettingsVersion;
			Scribe_Values.Look(ref version, "SettingsVersion", 0);
			LustEffectPower.Scribe();
			EnableBastardRelation.Scribe();
			LustLimit.Scribe();
			MaxSingleLustChange.Scribe();
			SelectionLocked.Scribe();
			SexCanFillBuckets.Scribe();
			DevMode.Scribe();
			Scribe_Deep.Look(ref history, "History", history.Handles);
			base.ExposeData();

			if (Scribe.mode != LoadSaveMode.LoadingVars)
				return;

			if (history == null)
			{
				// Previously history settings were in Configurations. Direct call to try read old data
				SettingsContainer.CreateHistoryContainer(this).ExposeData();
			}
		}
	}
}
