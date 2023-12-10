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
		public static void Postfix(ref Pawn __result)
		{
			if (__result == null)
				return;

			bool doVirginTrait = true;

			if (SexperienceMod.Settings.EnableRecordRandomizer && __result.DevelopmentalStage != DevelopmentalStage.Newborn && xxx.is_human(__result))
				doVirginTrait = SexHistory.RecordRandomizer.Randomize(__result);

			if (doVirginTrait)
				Virginity.TraitHandler.GenerateVirginTrait(__result);
		}
	}

	[HarmonyPatch(typeof(ParentRelationUtility), nameof(ParentRelationUtility.SetMother))]
	public static class Rimworld_Patch_RemoveVirginOnSetMother
	{
		/// <summary>
		/// Retcon virginity if game desides to generate a child for the pawn
		/// </summary>
		public static void Postfix(Pawn pawn, Pawn newMother)
		{
			if (!pawn.relations.DirectRelationExists(PawnRelationDefOf.Parent, newMother))
				return; // Failed to add relation?

			Trait virgin = newMother.story?.traits?.GetTrait(RsDefOf.Trait.Virgin, Virginity.TraitDegree.FemaleVirgin);
			if (virgin != null)
			{
				newMother.story.traits.RemoveTrait(virgin);

				// Player may notice the missing trait on their pawns.
				// Doing this for all pawns results in up to a half of tribal raid generating with "virgin?"
				if (newMother.IsColonist || newMother.IsPrisonerOfColony)
				{
					newMother.story.traits.GainTrait(new Trait(RsDefOf.Trait.Virgin, Virginity.TraitDegree.FemaleAfterSurgery));
				}
			}
		}
	}
}
