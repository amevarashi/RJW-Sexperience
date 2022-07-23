using RimWorld;
<<<<<<< HEAD
using System;
=======
using rjw;
>>>>>>> SizeMattersPrecept
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

        internal static void ConvertPawnBySex(Pawn pawn, Pawn partner, float severity = 0.01f)
        {
			// Important Note: This is called on "orgasm" - hence when a pawn has the orgasm he is the "pawn" here.
			// If Bob fucks Alice, Alice has the orgasm and Alice is the Pawn while Bob is the Partner.
			// Hence, the Conversion happens from Partner -> Pawn and not the other way round!

			// Short Circuit: Either pawn is null, exit early and do nothing
			if (pawn == null || partner == null) return;
			bool sameIdeo = pawn.Ideo == partner.Ideo;
			// Option A: Partner has same Ideo as Pawn, increase certainty
			if (sameIdeo)
            {
				pawn.ideo.OffsetCertainty(severity);
            }
			// Option B: Partner as different Ideo, try to convert
			else
			{
				pawn.ideo.IdeoConversionAttempt(severity, partner.Ideo);
			}
		}
    }
		public static float getGenitalSize(Pawn p)
		{
			if (p == null)
				return 0f;

			// Iff the pawn has multiple genitalia, the "best" is picked (the biggest penis or tightest vagina)
			float best_seen_size = 0f;
			foreach (Hediff part in rjw.Genital_Helper.get_AllPartsHediffList(p))
			{
				// Only check for Vaginas and Penises, not for Anus or for things not categorized as primary sexual parts
				if (Genital_Helper.is_penis(part) || Genital_Helper.is_vagina(part))
				{
					best_seen_size = part.Severity > best_seen_size ? part.Severity : best_seen_size;
				}
			}


			// For Women, the scale is inversed.
			if (p.gender == Gender.Female)
				return 1 - best_seen_size;

			return best_seen_size;
		}
	}
}
