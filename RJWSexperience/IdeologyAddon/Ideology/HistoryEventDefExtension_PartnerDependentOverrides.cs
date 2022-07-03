using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class HistoryEventDefExtension_PartnerDependentOverrides : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<PartnerDependentOverride> overrideRules = new List<PartnerDependentOverride>();

		public class PartnerDependentOverride
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
}
