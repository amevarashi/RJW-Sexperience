using RimWorld;
using rjw.Modules.Interactions.Contexts;
using rjw.Modules.Interactions.Enums;
using rjw.Modules.Interactions.Rules.PartKindUsageRules;
using rjw.Modules.Shared;
using RJWSexperience.Logs;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RJWSexperience.Cum.Interactions
{
	public class CumAddictPartKindUsageRule : IPartPreferenceRule
	{
		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForDominant(InteractionContext context)
		{
			if (context.Internals.Submissive.Parts.Penises.Any())
				return GetForCumAddict(context.Internals.Dominant.Pawn);

			if (AddictionUtility.IsAddicted(context.Internals.Submissive.Pawn, RsDefOf.Chemical.Cum))
				return GetForPartner();

			return Enumerable.Empty<Weighted<LewdablePartKind>>();
		}

		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForSubmissive(InteractionContext context)
		{
			if (context.Internals.Dominant.Parts.Penises.Any())
				return GetForCumAddict(context.Internals.Submissive.Pawn);

			if (AddictionUtility.IsAddicted(context.Internals.Dominant.Pawn, RsDefOf.Chemical.Cum))
				return GetForPartner();

			return Enumerable.Empty<Weighted<LewdablePartKind>>();
		}

		/// <summary>
		/// Addict wants to use mouth
		/// </summary>
		private IEnumerable<Weighted<LewdablePartKind>> GetForCumAddict(Pawn pawn)
		{
			var log = LogManager.GetLogger<CumAddictPartKindUsageRule, DebugLogProvider>();
			log.Message($"Called for {pawn.NameShortColored}");

			if (!(pawn.needs?.TryGetNeed(RsDefOf.Need.Chemical_Cum) is Need_Chemical cumNeed))
				yield break;

			log.Message($"{pawn.NameShortColored} is cum addict, current desire level: {cumNeed.CurCategory}");

			yield return new Weighted<LewdablePartKind>(Multipliers.DoubledPlus, LewdablePartKind.Mouth);

			// In dire need also they are also refuse to use other orifices
			switch (cumNeed.CurCategory)
			{
				case DrugDesireCategory.Desire:
					yield return new Weighted<LewdablePartKind>(Multipliers.VeryRare, LewdablePartKind.Anus);
					yield return new Weighted<LewdablePartKind>(Multipliers.VeryRare, LewdablePartKind.Vagina);
					break;

				case DrugDesireCategory.Withdrawal:
					yield return new Weighted<LewdablePartKind>(Multipliers.Never, LewdablePartKind.Anus);
					yield return new Weighted<LewdablePartKind>(Multipliers.Never, LewdablePartKind.Vagina);
					break;
			}
		}

		/// <summary>
		/// Addict asks partner to use penis
		/// </summary>
		private IEnumerable<Weighted<LewdablePartKind>> GetForPartner()
		{
			yield return new Weighted<LewdablePartKind>(Multipliers.Common, LewdablePartKind.Penis);
		}
	}
}
