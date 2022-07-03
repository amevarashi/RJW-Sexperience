using RimWorld;
using rjw;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class PartnerFilter
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isAnimal;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isVeneratedAnimal;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isSlave;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isAlien;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<PawnRelationDef> hasOneOfRelations;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<PawnRelationDef> hasNoneOfRelations;

		private bool initialized = false;
		private HashSet<PawnRelationDef> hasOneOfRelationsHashed;
		private HashSet<PawnRelationDef> hasNoneOfRelationsHashed;

		public bool Applies(Pawn pawn, Pawn partner)
		{
			if (isAnimal != null && isAnimal != partner.IsAnimal())
				return false;

			if (isVeneratedAnimal != null && isVeneratedAnimal != pawn.Ideo.IsVeneratedAnimal(partner))
				return false;

			if (isSlave != null && isSlave != partner.IsSlave)
				return false;

			//if (isAlien != null && isAlien != partner)
			//	return false;

			if (!CheckRelations(pawn, partner))
				return false;

			return true;
		}

		private bool CheckRelations(Pawn pawn, Pawn partner)
		{
			if (!initialized)
				Initialize();

			if (hasNoneOfRelationsHashed == null && hasOneOfRelationsHashed == null)
				return true;

			IEnumerable<PawnRelationDef> relations = pawn.GetRelations(partner);

			if (hasOneOfRelationsHashed != null)
			{
				if (relations.EnumerableNullOrEmpty())
					return false;

				if (!hasOneOfRelationsHashed.Overlaps(relations))
					return false;
			}

			if (hasNoneOfRelationsHashed != null && !relations.EnumerableNullOrEmpty() && hasNoneOfRelationsHashed.Overlaps(relations))
			{
				return false;
			}

			return true;
		}

		private void Initialize()
		{
			if (!hasNoneOfRelations.NullOrEmpty())
				hasNoneOfRelationsHashed = new HashSet<PawnRelationDef>(hasNoneOfRelations);

			if (!hasOneOfRelations.NullOrEmpty())
				hasOneOfRelationsHashed = new HashSet<PawnRelationDef>(hasOneOfRelations);

			initialized = true;
		}
	}
}
