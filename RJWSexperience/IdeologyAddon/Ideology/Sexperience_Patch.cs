using RimWorld;
using rjw;
using Verse;

namespace RJWSexperience.Ideology
{
	public static class Sexperience_Patch_ThrowVirginHIstoryEvent
	{
		public static void Postfix(Pawn pawn, Pawn partner, SexProps props, int degree)
		{
			string tag = "";
			if (props.isRape)
			{
				if (pawn == props.pawn && props.isRapist) tag += HETag.Rape;
				else tag += HETag.BeenRaped;
			}
			if (!pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, partner))
			{
				tag += HETag.NotSpouse;
			}

			if (pawn.gender == Gender.Male)
			{
				if (degree > 1) Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TakenM.TaggedEvent(pawn, tag + HETag.Gender(pawn), partner));
				Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TookM.TaggedEvent(partner, tag + HETag.Gender(pawn), pawn));
			}
			else
			{
				if (degree > 1) Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TakenF.TaggedEvent(pawn, tag + HETag.Gender(pawn), partner));
				Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TookF.TaggedEvent(partner, tag + HETag.Gender(pawn), pawn));
			}
		}
	}

	public static class Sexperience_Patch_IsIncest
	{
		public static bool Prefix(Pawn pawn, Pawn otherpawn, ref bool __result)
		{
			__result = IdeoUtility.IsIncest(pawn, otherpawn);
			return false;
		}
	}

}
