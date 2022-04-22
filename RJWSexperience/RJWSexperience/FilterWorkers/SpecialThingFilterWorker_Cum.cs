using Verse;

namespace RJWSexperience
{
	public class SpecialThingFilterWorker_Cum : SpecialThingFilterWorker_CumBase
	{
		public override bool Matches(Thing t)
		{
			return IsCum(t) || IsFoodWithCum(t);
		}
	}
}
