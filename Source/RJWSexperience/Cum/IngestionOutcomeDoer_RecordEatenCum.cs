using RimWorld;
using Verse;

namespace RJWSexperience.Cum
{
	public class IngestionOutcomeDoer_RecordEatenCum : IngestionOutcomeDoer
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float unitAmount = 1.0f;

		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			int amount = ingested.stackCount * (int)unitAmount;
			Logs.LogManager.GetLogger<IngestionOutcomeDoer_RecordEatenCum, Logs.DebugLogProvider>().Message($"Record {pawn.NameShortColored} eating {amount} ml of cum");
			pawn.records.Increment(RsDefOf.Record.NumofEatenCum);
			pawn.records.AddTo(RsDefOf.Record.AmountofEatenCum, amount);
		}
	}
}
