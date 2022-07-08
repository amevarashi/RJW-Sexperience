using RimWorld;
using rjw;
using RJWSexperience.Ideology.HistoryEvents;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	public static class Sexperience_Patch_ThrowVirginHIstoryEvent
	{
		public static void Postfix(Pawn pawn, Pawn partner, SexProps props, int degree)
		{
			string tag = "";
			if (props.isRape && pawn != props.pawn)
			{
				tag += Tag.BeenRaped;
			}
			if (!pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, partner))
			{
				tag += Tag.NotSpouse;
			}

			if (pawn.gender == Gender.Male)
			{
				if (degree > 1) Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TakenM.CreateTaggedEvent(pawn, tag + Tag.Gender(pawn), partner));
				Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TookM.CreateTaggedEvent(partner, tag + Tag.Gender(pawn), pawn));
			}
			else
			{
				if (degree > 1) Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TakenF.CreateTaggedEvent(pawn, tag + Tag.Gender(pawn), partner));
				Find.HistoryEventsManager.RecordEvent(VariousDefOf.Virgin_TookF.CreateTaggedEvent(partner, tag + Tag.Gender(pawn), pawn));
			}
		}
	}
}
