using RJWSexperience;
using Verse;
using Verse.AI;

namespace RJWSexperienceCum
{
    public class JobGiver_CleanSelfWithBucket : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
		{
            if (HediffDefOf.Hediff_CumController == null || !pawn.health.hediffSet.HasHediff(HediffDefOf.Hediff_CumController))
            {
                // Nothing to clean
                return null;
            }

			Building_CumBucket bucket = pawn.FindClosestBucket();

			if (bucket == null)
			{
				return null;
			}

            return JobMaker.MakeJob(JobDefOf.CleanSelfwithBucket, pawn, bucket);
        }
    }
}
