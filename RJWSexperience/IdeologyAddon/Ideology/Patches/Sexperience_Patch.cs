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
				VariousDefOf.RSI_VirginStolen.RecordEventWithPartner(exVirgin, partner);

			if (degree != femaleAfterSurgery)
				VariousDefOf.RSI_VirginTaken.RecordEventWithPartner(exVirgin, partner);

			VariousDefOf.RSI_TookVirgin.RecordEventWithPartner(partner, exVirgin);
		}
	}
}
