using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public class DefExtension_PartnerDependentSecondaryEvents : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<PartnerDependentRule> generationRules = new List<PartnerDependentRule>();
	}
}
