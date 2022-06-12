using RimWorld;
using rjw;
using Verse;

namespace RJWSexperience.Virginity
{
	public static class TraitHandler
	{
		private const float hymenSurgeryChance = 0.05f;

		public static void AddVirginTrait(Pawn pawn)
		{
			if (pawn.story?.traits == null)
				return;

			if (pawn.IsVirgin())
			{
				TraitDegree degree = TraitDegree.MaleVirgin;
				if (pawn.gender == Gender.Female) degree = TraitDegree.FemaleVirgin;
				Trait virgin = new Trait(VariousDefOf.Virgin, (int)degree, true);
				pawn.story.traits.GainTrait(virgin);
			}
			else if (pawn.gender == Gender.Female && Rand.Chance(hymenSurgeryChance))
			{
				Trait virgin = new Trait(VariousDefOf.Virgin, (int)TraitDegree.FemaleAfterSurgery, true);
				pawn.story.traits.GainTrait(virgin);
			}
		}

		public static bool RemoveVirginTrait(Pawn pawn, Pawn partner, SexProps props)
		{
			Trait virgin = pawn.story?.traits?.GetTrait(VariousDefOf.Virgin);
			if (virgin == null)
				return false;

			int degree = virgin.Degree;
			if (pawn.gender == Gender.Female && degree > 0 && !pawn.Dead)
			{
				FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_Blood, pawn.LabelShort, 1, FilthSourceFlags.Pawn);
			}
			RJWUtility.ThrowVirginHIstoryEvent(pawn, partner, props, degree);
			pawn.story.traits.RemoveTrait(virgin);
			return true;
		}
	}
}
