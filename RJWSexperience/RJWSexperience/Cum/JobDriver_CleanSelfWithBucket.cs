using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RJWSexperience  // Used in Menstruation with this namespace
{
	public class JobDriver_CleanSelfWithBucket : JobDriver
	{
		protected const int UNITTIME = 240;//ticks - 120 = 2 real seconds, 3 in-game minutes
		protected float progress = 0;
		protected float severitycache = 1;
		protected Hediff hediffcache;
		protected float CleaningTime
		{
			get
			{
				return severitycache * UNITTIME;
			}
		}

		protected Building_CumBucket Bucket => TargetB.Thing as Building_CumBucket;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(pawn, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(delegate
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				return !hediffs.Exists(x => x.def == VariousDefOf.Hediff_CumController);
			});
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			Toil cleaning = new Toil
			{
				initAction = CleaningInit,
				tickAction = CleaningTick,
				defaultCompleteMode = ToilCompleteMode.Never
			};
			cleaning.AddFinishAction(Finish);
			cleaning.WithProgressBar(TargetIndex.A, () => progress / CleaningTime);

			yield return cleaning;
		}

		protected void CleaningInit()
		{
			hediffcache = pawn.health.hediffSet.hediffs.Find(x => x.def == VariousDefOf.Hediff_Cum || x.def == VariousDefOf.Hediff_InsectSpunk);
			if (hediffcache == null)
			{
				pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
			}
			else
			{
				progress = 0;
				severitycache = hediffcache.Severity;
			}
		}

		protected void CleaningTick()
		{
			progress++;
			if (progress > CleaningTime)
			{
				Cleaned();
			}
		}

		protected void Cleaned()
		{
			if (hediffcache != null)
			{
				float cumamount = hediffcache.Severity * 10;
				hediffcache.Severity = 0;
				Bucket.AddCum(cumamount);
			}
			CleaningInit();
		}

		protected void Finish()
		{
			if (pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}
	}
}
