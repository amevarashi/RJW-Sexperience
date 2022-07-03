using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public class PartnerDependentRule
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public HistoryEventDef historyEventDef;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public PartnerFilter filter;

		public bool Applies(Pawn pawn, Pawn partner)
		{
			if (filter == null)
				return false;

			return filter.Applies(pawn, partner);
		}
	}
}
