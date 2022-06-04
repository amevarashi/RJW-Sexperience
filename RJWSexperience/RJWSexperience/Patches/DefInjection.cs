﻿using RJWSexperience.Logs;
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
			InjectRaces();
		}

		private static void InjectRaces()
		{
			IEnumerable<ThingDef> PawnDefs = DefDatabase<ThingDef>.AllDefs.Where(x => x.race != null && !x.race.IsMechanoid);
			if (PawnDefs.EnumerableNullOrEmpty())
				return;

			CompProperties comp = new CompProperties(typeof(SexHistoryComp));
			foreach (ThingDef def in PawnDefs)
				def.comps.Add(comp);

			LogManager.GetLogger("StaticConstructorOnStartup").Message($"Injected SexHistoryComp into {PawnDefs.Count()} pawn Defs");
		}
	}
}
