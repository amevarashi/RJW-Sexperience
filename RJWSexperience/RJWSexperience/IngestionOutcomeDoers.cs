using RimWorld;
using Verse;

namespace RJWSexperience
{
	public class CumOutcomeDoers : IngestionOutcomeDoer
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float unitAmount = 1.0f;

		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			pawn.AteCum(ingested.stackCount * unitAmount);
		}
	}
}
