using RimWorld;
using rjw;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Virginity
{
	public class Recipe_HymenSurgery : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			if (pawn.gender != Gender.Female)
				yield break;

			BodyPartRecord part = Genital_Helper.get_genitalsBPR(pawn);
			if (part == null)
				yield break;

			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, part);
			if (Genital_Helper.has_vagina(pawn, hediffs) && !pawn.HasHymen())
				yield return part;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer == null)
				return;

			TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
			{
				billDoer,
				pawn
			});
			TraitHandler.AddVirginTrait(pawn);
		}
	}
}
