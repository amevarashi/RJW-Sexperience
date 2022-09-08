using RimWorld;
using RJWSexperience;
using Verse;
using Verse.AI;

namespace RJWSexperienceCum
{
    public class WorkGiver_CleanSelfWithBucket : WorkGiver_Scanner
    {
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(ThingDefOf.CumBucket);
		public override PathEndMode PathEndMode => PathEndMode.ClosestTouch;
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return HediffDefOf.Hediff_CumController == null || !pawn.health.hediffSet.HasHediff(HediffDefOf.Hediff_CumController);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Building_CumBucket bucket))
				return false;

			return bucket.StoredStackCount < ThingDefOf.GatheredCum.stackLimit;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.CleanSelfwithBucket, pawn, t);
		}
	}
}
