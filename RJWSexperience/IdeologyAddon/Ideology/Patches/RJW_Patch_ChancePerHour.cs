using HarmonyLib;
using RimWorld;
using rjw;
using RJWSexperience.Ideology.Precepts;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Bestiality), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_Bestiality
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			if (__result > 0f && pawn.Ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyBestialityMtb>(pawn.Ideo);
		}
	}

	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_RapeCP), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_RapeCP
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			if (__result < 0f || pawn.Ideo == null) // ideo is null if don't have dlc
				return;

			if (!VariousDefOf.RSI_Raped.CreateEvent(pawn).DoerWillingToDo())
			{
				__result = -2f;
				return;
			}
			__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyRapeCPMtb>(pawn.Ideo);
		}
	}
	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Necro), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_Necro
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			if (__result < 0f || pawn.Ideo == null) // ideo is null if don't have dlc
				return;

			if (!VariousDefOf.SexWithCorpse.CreateEvent(pawn).DoerWillingToDo())
			{
				__result = -2f;
				return;
			}
			__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyNecroMtb>(pawn.Ideo);
		}
	}

	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Fappin), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_Fappin
	{
		public static void Postfix(Pawn p, ref float __result)
		{
			if (__result > 0f && p.Ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyFappinMtb>(p.Ideo);
		}
	}
}
