using RimWorld;
using Verse;

namespace RJWSexperience.Virginity
{
	public static class TraitHandler
	{
		private const float hymenSurgeryChance = 0.05f;

		public static void GenerateVirginTrait(Pawn pawn)
		{
			if (pawn.story?.traits == null)
				return;

			if (pawn.gender == Gender.Female && !pawn.IsVirgin())
			{
				TechLevel techLevel = pawn.Faction?.def.techLevel ?? TechLevel.Industrial;

				if (techLevel >= TechLevel.Industrial && Rand.Chance(hymenSurgeryChance))
				{
					Trait virgin = new Trait(RsDefOf.Trait.Virgin, TraitDegree.FemaleAfterSurgery, true);
					pawn.story.traits.GainTrait(virgin);
				}
				return;
			}

			AddVirginTrait(pawn);
		}

		public static void AddVirginTrait(Pawn pawn)
		{
			if (pawn.story?.traits == null)
				return;

			if (pawn.IsVirgin())
			{
				int degree = TraitDegree.MaleVirgin;
				if (pawn.gender == Gender.Female)
					degree = TraitDegree.FemaleVirgin;
				Trait virgin = new Trait(RsDefOf.Trait.Virgin, degree, true);
				pawn.story.traits.GainTrait(virgin);
			}
			else if (pawn.gender == Gender.Female)
			{
				Trait virgin = new Trait(RsDefOf.Trait.Virgin, TraitDegree.FemaleAfterSurgery, true);
				pawn.story.traits.GainTrait(virgin);
			}
		}

		/// <summary>
		/// Remove virginity trait and spawn blood filth if applicable
		/// </summary>
		/// <returns>Degree of the removed trait</returns>
		public static int? RemoveVirginTrait(Pawn pawn)
		{
			Trait virgin = pawn.story?.traits?.GetTrait(RsDefOf.Trait.Virgin);
			if (virgin == null)
				return null;

			int degree = virgin.Degree;
			if (pawn.gender == Gender.Female && degree > 0 && pawn.Spawned && !pawn.Dead)
			{
				FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_Blood, pawn.LabelShort, 1, FilthSourceFlags.Pawn);
			}
			pawn.story.traits.RemoveTrait(virgin);
			return degree;
		}
	}
}
