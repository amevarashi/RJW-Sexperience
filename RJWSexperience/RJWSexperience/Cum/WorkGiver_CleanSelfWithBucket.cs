using RimWorld;
using Verse;
using Verse.AI;

namespace RJWSexperience.Cum
{
    public class WorkGiver_CleanSelfWithBucket : WorkGiver_Scanner
    {
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VariousDefOf.CumBucket);
		public override PathEndMode PathEndMode => PathEndMode.ClosestTouch;
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return VariousDefOf.Hediff_CumController == null || !pawn.health.hediffSet.HasHediff(VariousDefOf.Hediff_CumController);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Building_CumBucket bucket))
				return false;

			return bucket.StoredStackCount < VariousDefOf.GatheredCum.stackLimit;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(VariousDefOf.CleanSelfwithBucket, pawn, t);
		}
	}
}
