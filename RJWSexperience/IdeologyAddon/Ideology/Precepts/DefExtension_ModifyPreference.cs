using RimWorld;
using rjw;
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
			public Filter filter;

			public bool Applies(Pawn pawn, Pawn partner)
			{
				if (filter == null)
					return true;

				if (filter.isAnimal != null && filter.isAnimal != partner.IsAnimal())
					return false;

				if (filter.isVeneratedAnimal != null && filter.isVeneratedAnimal != pawn.Ideo.IsVeneratedAnimal(partner))
					return false;

				if (!filter.hasOneOfRelations.NullOrEmpty())
				{
					if (pawn.relations == null)
						return false;

					bool found = false;
					foreach (PawnRelationDef relationDef in filter.hasOneOfRelations)
					{
						if (pawn.relations?.DirectRelationExists(relationDef, partner) == true)
						{
							found = true;
							break;
						}
					}
					if (!found)
						return false;
				}

				if (!filter.hasNoneOfRelations.NullOrEmpty() && pawn.relations != null)
				{
					foreach (PawnRelationDef relationDef in filter.hasNoneOfRelations)
					{
						if (pawn.relations.DirectRelationExists(relationDef, partner))
							return false;
					}
				}

				return true;
			}
		}

		public class Filter
		{
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public bool? isAnimal;
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public bool? isVeneratedAnimal;
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public List<PawnRelationDef> hasOneOfRelations;
			[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
			public List<PawnRelationDef> hasNoneOfRelations;
		}
	}
}
