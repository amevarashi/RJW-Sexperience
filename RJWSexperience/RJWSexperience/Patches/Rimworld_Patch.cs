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
			if (__result == null)
				return;

			bool doVirginTrait = true;

			if (SexperienceMod.Settings.History.EnableRecordRandomizer && !request.Newborn && xxx.is_human(__result))
				doVirginTrait = RecordRandomizer.Randomize(__result);

			if (doVirginTrait)
				Virginity.TraitHandler.AddVirginTrait(__result);
		}
	}
}
