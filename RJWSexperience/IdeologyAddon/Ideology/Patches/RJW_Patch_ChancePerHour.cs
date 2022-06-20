using HarmonyLib;
using RimWorld;
using rjw;
using RJWSexperience.Ideology.Precepts;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Bestiality), "MtbHours")]
	public static class RJW_Patch_ChancePerHour_Bestiality
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyBestialityMtb>(ideo);
		}
	}

	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_RapeCP), "MtbHours")]
	public static class RJW_Patch_ChancePerHour_RapeCP
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyRapeCPMtb>(ideo);
		}
	}
	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Necro), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_Necro
	{
		public static void Postfix(Pawn pawn, ref float __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyNecroMtb>(ideo);
		}
	}

	[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Fappin), "MtbHours")]
	public static class RJW_Patch_ThinkNode_ChancePerHour_Fappin
	{
		public static void Postfix(Pawn p, ref float __result)
		{
			if (__result < 0f)
				return;

			Ideo ideo = p.Ideo;
			if (ideo != null) // ideo is null if don't have dlc
				__result *= IdeoUtility.GetPreceptsMtbMultiplier<DefExtension_ModifyFappinMtb>(ideo);
		}
	}
}
