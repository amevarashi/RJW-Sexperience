using RimWorld;
using RJWSexperience.ExtensionMethods;
using Verse;

namespace RJWSexperience
{
    public class CumOutcomeDoers : IngestionOutcomeDoer
    {
        public float unitAmount = 1.0f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            pawn.AteCum(ingested.stackCount * unitAmount);
        }
    }
}
