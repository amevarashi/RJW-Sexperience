using RimWorld;
using Verse;

namespace RJWSexperience
{
	public abstract class SpecialThingFilterWorker_CumBase : SpecialThingFilterWorker
	{
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsIngestible && def.IsProcessedFood;
		}

		protected bool IsCum(Thing t) => IsCum(t.def);

		protected bool IsCum(ThingDef t) => t == VariousDefOf.GatheredCum;

		protected bool IsFoodWithCum(Thing food)
		{
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();

			if (compIngredients == null)
				return false;

			foreach (ThingDef ingredient in compIngredients.ingredients)
			{
				if (IsCum(ingredient))
					return true;
			}

			return false;
		}
	}
}
