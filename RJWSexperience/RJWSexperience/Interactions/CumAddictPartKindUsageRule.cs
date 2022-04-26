using rjw.Modules.Interactions.Contexts;
using rjw.Modules.Interactions.Enums;
using rjw.Modules.Interactions.Rules.PartKindUsageRules;
using rjw.Modules.Shared;
using System.Collections.Generic;

namespace RJWSexperience.Interactions
{
	public class CumAddictPartKindUsageRule : IPartPreferenceRule
	{
		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForDominant(InteractionContext context)
		{
			if (context.Internals.Dominant.Pawn.health?.hediffSet?.HasHediff(VariousDefOf.CumAddiction) ?? false)
			{
				// Cum addicts are really eager to use mouth
				yield return new Weighted<LewdablePartKind>(Multipliers.DoubledPlus, LewdablePartKind.Mouth);
			}
		}

		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForSubmissive(InteractionContext context)
		{
			Logs.LogManager.GetLogger<CumAddictPartKindUsageRule, Logs.DebugLogProvider>().Warning($"Called for {context.Internals.Submissive.Pawn.NameShortColored}");

			if (context.Internals.Submissive.Pawn.health?.hediffSet?.HasHediff(VariousDefOf.CumAddiction) ?? false)
			{
				// Cum addicts are really eager to use mouth
				yield return new Weighted<LewdablePartKind>(Multipliers.DoubledPlus, LewdablePartKind.Mouth);
			}
		}
	}
}
