using UnityEngine;
using Verse;

namespace RJWSexperience
{

	public class SexperienceMod : Mod
	{
		private static Configurations settings;
		public static Configurations Settings { get => settings; }

		public SexperienceMod(ModContentPack content) : base(content)
		{
			settings = GetSettings<Configurations>();
		}

		public override string SettingsCategory()
		{
			return Keyed.Mod_Title;
		}

		public override void DoSettingsWindowContents(Rect inRect) => Settings.DoSettingsWindowContents(inRect);
	}
}
