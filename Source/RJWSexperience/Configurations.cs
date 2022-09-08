using UnityEngine;
using Verse;
using RJWSexperience.Settings;

namespace RJWSexperience
{
	public class Configurations : ModSettings, ITab
	{
		public string Label => Keyed.TabLabelMain;
		public const int CurrentSettingsVersion = 1;

		// Defaults
		public const float LustEffectPowerDefault = 0.5f;
		public const bool EnableBastardRelationDefault = true;
		public const float LustLimitDefault = SettingsTabHistory.MaxLustDeviationDefault / 3f;
		public const float MaxSingleLustChangeDefault = 0.5f;
		public const bool SexCanFillBucketsDefault = false;
		public const bool selectionLockedDefault = false;

		// Private attributes
		private float lustEffectPower = LustEffectPowerDefault;
		private bool enableBastardRelation = EnableBastardRelationDefault;
		private float lustLimit = LustLimitDefault;
		private float maxSingleLustChange = MaxSingleLustChangeDefault;
		private bool sexCanFillBuckets = SexCanFillBucketsDefault;
		private bool selectionLocked = selectionLockedDefault;
		private SettingsTabHistory history = SettingsTabHistory.CreateDefault();
		private SettingsTabDebug debug = new SettingsTabDebug();

		//Public read-only properties
		public float LustEffectPower => lustEffectPower;
		public bool EnableBastardRelation => enableBastardRelation;
		public float LustLimit => lustLimit;
		public float MaxSingleLustChange => maxSingleLustChange;
		public bool SexCanFillBuckets => sexCanFillBuckets;
		public SettingsTabHistory History => history;
		public SettingsTabDebug Debug => debug;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2292:Trivial properties should be auto-implemented", Justification = "Can't scribe property")]
		public bool SelectionLocked { get => selectionLocked; set => selectionLocked = value; }

		public void ResetToDefault()
		{
			lustEffectPower = LustEffectPowerDefault;
			enableBastardRelation = EnableBastardRelationDefault;
			lustLimit = LustLimitDefault;
			maxSingleLustChange = MaxSingleLustChangeDefault;
			sexCanFillBuckets = SexCanFillBucketsDefault;
		}

		public override void ExposeData()
		{
			int version = CurrentSettingsVersion;
			Scribe_Values.Look(ref version, "SettingsVersion", 0);
			Scribe_Values.Look(ref lustEffectPower, "LustEffectPower", LustEffectPowerDefault);
			Scribe_Values.Look(ref enableBastardRelation, "EnableBastardRelation", EnableBastardRelationDefault);
			Scribe_Values.Look(ref lustLimit, "LustLimit", LustLimitDefault);
			Scribe_Values.Look(ref maxSingleLustChange, "maxSingleLustChange", MaxSingleLustChangeDefault);
			Scribe_Values.Look(ref selectionLocked, "SelectionLocked", selectionLockedDefault);
			Scribe_Values.Look(ref sexCanFillBuckets, "SexCanFillBuckets", SexCanFillBucketsDefault);
			Scribe_Deep.Look(ref history, "History");
			Scribe_Deep.Look(ref debug, "Debug");
			base.ExposeData();

			if (Scribe.mode != LoadSaveMode.LoadingVars)
				return;

			if (history == null)
			{
				history = new SettingsTabHistory();
				// Previously history settings were in Configurations. Direct call to try read old data
				history.ExposeData();
			}

			if (debug == null)
			{
				debug = new SettingsTabDebug();
				debug.Reset();
			}
		}

		public void DoTabContents(Rect inRect)
		{
			const float lineHeight = SettingsWidgets.lineHeight;

			Listing_Standard listmain = new Listing_Standard();
			listmain.maxOneColumn = true;
			listmain.Begin(inRect);

			SettingsWidgets.SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_2_Label + " x" + lustEffectPower, Keyed.Option_2_Desc, ref lustEffectPower, 0f, 2f, 0.01f);
			SettingsWidgets.SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_8_Label + " " + lustLimit, Keyed.Option_8_Desc, ref lustLimit, 0f, 5000f, 1f);
			SettingsWidgets.SliderOption(listmain.GetRect(lineHeight * 2f), Keyed.Option_MaxSingleLustChange_Label + " " + maxSingleLustChange, Keyed.Option_MaxSingleLustChange_Desc, ref maxSingleLustChange, 0f, 10f, 0.05f);

			listmain.CheckboxLabeled(Keyed.Option_EnableBastardRelation_Label, ref enableBastardRelation, Keyed.Option_EnableBastardRelation_Desc);
			listmain.CheckboxLabeled(Keyed.Option_SexCanFillBuckets_Label, ref sexCanFillBuckets, Keyed.Option_SexCanFillBuckets_Desc);

			if (SexperienceMod.Settings.Debug.DevMode)
				LustUtility.DrawGraph(listmain.GetRect(300f));

			if (listmain.ButtonText(Keyed.Button_ResetToDefault))
			{
				ResetToDefault();
			}
			listmain.End();
		}
	}
}
