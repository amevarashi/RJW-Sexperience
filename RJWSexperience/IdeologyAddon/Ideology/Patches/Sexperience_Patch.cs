using rjw;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	public static class Sexperience_Patch_ThrowVirginHistoryEvent
	{
		public static void Postfix(Pawn exVirgin, Pawn partner, SexProps props, int degree)
		{
			const int femaleAfterSurgery = 1;

			if (props.isRape && exVirgin == props.partner)
				RsiHistoryEventDefOf.RSI_VirginStolen.RecordEventWithPartner(exVirgin, partner);
			else if (degree != femaleAfterSurgery)
				RsiHistoryEventDefOf.RSI_VirginTaken.RecordEventWithPartner(exVirgin, partner);

			RsiHistoryEventDefOf.RSI_TookVirgin.RecordEventWithPartner(partner, exVirgin);
		}
	}
}
