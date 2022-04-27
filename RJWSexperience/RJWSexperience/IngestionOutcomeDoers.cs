﻿using RimWorld;
using Verse;

namespace RJWSexperience
{
	public class CumOutcomeDoers : IngestionOutcomeDoer
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float unitAmount = 1.0f;

		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			int amount = ingested.stackCount * (int)unitAmount;
			Logs.LogManager.GetLogger<CumOutcomeDoers, Logs.DebugLogProvider>().Message($"Record {pawn.NameShortColored} eating {amount} ml of cum");
			pawn.records.Increment(VariousDefOf.NumofEatenCum);
			pawn.records.AddTo(VariousDefOf.AmountofEatenCum, amount);
		}
	}
}
