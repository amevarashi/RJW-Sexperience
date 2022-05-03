using HarmonyLib;
using rjw;
using System;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
	public static class Rimworld_Patch_GeneratePawn
	{
		public static void Postfix(PawnGenerationRequest request, ref Pawn __result)
		{
			if (SexperienceMod.Settings.EnableRecordRandomizer && __result != null && !request.Newborn && xxx.is_human(__result))
			{
				RecordRandomizer.Randomize(__result);
			}
			__result.AddVirginTrait();
		}
	}
}
