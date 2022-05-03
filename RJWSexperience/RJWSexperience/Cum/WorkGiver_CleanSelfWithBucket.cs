using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
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
			return !pawn.health.hediffSet.HasHediff(RJW_SemenoOverlayHediffDefOf.Hediff_Bukkake);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Building_CumBucket bucket))
				return false;

			List<Thing> thingsInBucket = bucket.Map.thingGrid.ThingsListAt(bucket.Position);

			int stackInBucket = thingsInBucket.Select(thing => thing.stackCount).Aggregate((sum, x) => sum + x);

			return stackInBucket < VariousDefOf.GatheredCum.stackLimit;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(VariousDefOf.CleanSelfwithBucket, pawn, t);
		}
	}
}
