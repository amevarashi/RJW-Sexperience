using RimWorld;
using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public static class HistoryEventDefExtensionMethods
	{
		public static void RecordEventWithPartner(this HistoryEventDef def, Pawn pawn, Pawn partner)
		{
			DefExtension_PartnerDependentSecondaryEvents secondaryEvents = def.GetModExtension<DefExtension_PartnerDependentSecondaryEvents>();

			if (secondaryEvents != null)
			{
				foreach (PartnerDependentRule rule in secondaryEvents.generationRules)
				{
					if (rule.Applies(pawn, partner))
						rule.historyEventDef.RecordEventWithPartner(pawn, partner);
				}
			}

			Find.HistoryEventsManager.RecordEvent(def.CreateEventWithPartner(pawn, partner));
		}

		public static HistoryEvent CreateEvent(this HistoryEventDef def, Pawn pawn)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer));
		}

		public static HistoryEvent CreateEventWithPartner(this HistoryEventDef def, Pawn pawn, Pawn partner)
		{
			DefExtension_PartnerDependentOverrides overrides = def.GetModExtension<DefExtension_PartnerDependentOverrides>();

			if (overrides == null)
				return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(ArgsNamesCustom.Partner));

			foreach (PartnerDependentRule rule in overrides.overrideRules)
			{
				if (rule.Applies(pawn, partner))
					return rule.historyEventDef.CreateEventWithPartner(pawn, partner);
			}

			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(ArgsNamesCustom.Partner));
		}
	}
}
