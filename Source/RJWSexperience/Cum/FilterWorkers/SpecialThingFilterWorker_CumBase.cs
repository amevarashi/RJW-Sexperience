using RimWorld;
using Verse;

namespace RJWSexperience.Cum.FilterWorkers
{
	public abstract class SpecialThingFilterWorker_CumBase : SpecialThingFilterWorker
	{
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsIngestible && def.IsProcessedFood;
		}

		protected bool IsCum(ThingDef t) => t == RsDefOf.Thing.GatheredCum;

		protected bool IsFoodWithCum(Thing food)
		{
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();

			if (compIngredients?.ingredients == null)
				return false;

			for (int i = 0; i < compIngredients.ingredients.Count; i++)
			{
				if (IsCum(compIngredients.ingredients[i]))
					return true;
			}

			return false;
		}
	}
}
