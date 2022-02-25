using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RJWSexperience.Ideology
{
	public static class IdeoUtility
	{
		public static bool IsSubmissive(this Pawn pawn)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null)
			{
				if (ideo.HasPrecept(VariousDefOf.Submissive_Female) && pawn.gender == Gender.Female) return true;
				else if (ideo.HasPrecept(VariousDefOf.Submissive_Male) && pawn.gender == Gender.Male) return true;
			}

			return false;
		}

		public static bool ContainAll(string str, string[] tags)
		{
			string lstr = str.ToLower();
			if (!tags.NullOrEmpty()) for (int i = 0; i < tags.Count(); i++)
				{
					if (!lstr.Contains('[' + tags[i].ToLower() + ']')) return false;
				}
			return true;
		}

		public static bool IsIncest(Pawn pawn, Pawn partner)
		{
			IEnumerable<PawnRelationDef> relations = pawn.GetRelations(partner);
			Ideo ideo = pawn.Ideo;
			bool wide = false;
			if (ideo != null) wide = ideo.HasPrecept(VariousDefOf.Incestuos_Disapproved_CloseOnly);
			if (!relations.EnumerableNullOrEmpty()) foreach (PawnRelationDef relation in relations)
				{
					if (wide)
					{
						if (relation.incestOpinionOffset < 0) return true;
					}
					else if (relation.familyByBloodRelation) return true;
				}
			return false;
		}
	}
}
