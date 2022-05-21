using HarmonyLib;
using RimWorld;
using rjw;
using System;
using Verse;

namespace RJWSexperience.Ideology
{
	/*
	[HarmonyPatch(typeof(JobDriver_Sex), "Orgasm")]
	public static class RJW_Patch_Orgasm_IdeoConversion
	{
		public static void Postfix(JobDriver_Sex __instance)
		{
			// ShortCuts: Exit Early if Pawn or Partner are null (can happen with Animals or Masturbation)
			if (__instance.pawn == null || __instance.Partner == null)
				return;
			// Orgasm is called "all the time" - it exits early when the sex is still going. 
			// Hence, we exit early if there is no actual orgasm happening
			if (__instance.sex_ticks > __instance.orgasmstick)
				return;

			if (__instance.pawn.Ideo.HasPrecept(VariousDefOf.Proselyzing_By_Orgasm))
			{
				IdeoUtility.ConvertPawnBySex(__instance.pawn, __instance.Partner);
			}
		}
	}

	[HarmonyPatch(typeof(SexUtility), "Aftersex", new Type[] { typeof(SexProps) })]
	public static class RJW_Patch_Aftersex_IdeoConversion
	{
		// This is not exactly where I should put it (Maybe after The JobDriver_Sex Finishes??)
		// But it works here and doesn't damage things.
		public static void Postfix(SexProps props)
		{
			
		}
	}
	*/
}
