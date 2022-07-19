using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public class DefExtension_SecondaryEvents : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<TwoPawnEventRule> generationRules = new List<TwoPawnEventRule>();
	}
}
