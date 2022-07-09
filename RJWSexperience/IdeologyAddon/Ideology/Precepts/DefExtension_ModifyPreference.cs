using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.Precepts
{
	public class DefExtension_ModifyPreference : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<Rule> rules;

		public void Apply(Pawn pawn, Pawn partner, ref float preference)
		{
			foreach (Rule rule in rules)
			{
				if (rule.Applies(pawn, partner))
					preference *= rule.multiplier;
			}
		}

		public class Rule
		{
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public float multiplier = 1f;
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public TwoPawnFilter filter;

			public bool Applies(Pawn pawn, Pawn partner)
			{
				if (filter == null)
					return true;

				return filter.Applies(pawn, partner);
			}
		}
	}
}
