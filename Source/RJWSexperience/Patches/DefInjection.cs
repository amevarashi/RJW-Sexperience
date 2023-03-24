using RJWSexperience.Logs;
using RJWSexperience.SexHistory;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RJWSexperience
{
	[StaticConstructorOnStartup]
	public static class DefInjection
	{
		static DefInjection()
		{
			if (SexperienceMod.Settings.EnableSexHistory)
				InjectRaces();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1146:Use conditional access.", Justification = "race != null is needed")]
		private static void InjectRaces()
		{
			IEnumerable<ThingDef> PawnDefs = DefDatabase<ThingDef>.AllDefs.Where(x => x.race != null && !x.race.IsMechanoid);
			if (PawnDefs.EnumerableNullOrEmpty())
				return;

			CompProperties compProperties = new CompProperties(typeof(SexHistoryComp));
			foreach (ThingDef def in PawnDefs)
				def.comps.Add(compProperties);

			LogManager.GetLogger<DebugLogProvider>("StaticConstructorOnStartup").Message($"Injected SexHistoryComp into {PawnDefs.Count()} pawn Defs");
		}
	}
}
