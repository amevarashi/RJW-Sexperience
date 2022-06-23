using HarmonyLib;
using RimWorld;
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
				doVirginTrait = SexHistory.RecordRandomizer.Randomize(__result);

			if (doVirginTrait)
				Virginity.TraitHandler.GenerateVirginTrait(__result);
		}
	}

	[HarmonyPatch(typeof(ParentRelationUtility), nameof(ParentRelationUtility.SetMother))]
	public static class Rimworld_Patch_RemoveVirginOnSetMother
	{
		public static void Postfix(Pawn pawn, Pawn newMother)
		{
			if (!pawn.relations.DirectRelationExists(PawnRelationDefOf.Parent, newMother))
				return;

			Trait virgin = newMother.story?.traits?.GetTrait(VariousDefOf.Virgin, Virginity.TraitDegree.FemaleVirgin);
			if (virgin != null)
			{
				newMother.story.traits.RemoveTrait(virgin);
				newMother.story.traits.GainTrait(new Trait(VariousDefOf.Virgin, Virginity.TraitDegree.FemaleAfterSurgery));
			}
		}
	}
}
