using Verse;

namespace RJWSexperience.Cum.FilterWorkers
{
	public class SpecialThingFilterWorker_NoCum : SpecialThingFilterWorker_CumBase
	{
		public override bool Matches(Thing t)
		{
			return !IsCum(t) && !IsFoodWithCum(t);
		}
	}
}
