using RimWorld;
using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public static class HistoryEventDefExtensionMethods
	{
		public static void RecordEventWithPartner(this HistoryEventDef def, Pawn pawn, Pawn partner)
		{
			DefExtension_SecondaryEvents secondaryEvents = def.GetModExtension<DefExtension_SecondaryEvents>();

			if (secondaryEvents != null)
			{
				foreach (TwoPawnEventRule rule in secondaryEvents.generationRules)
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
			DefExtension_EventOverrides overrides = def.GetModExtension<DefExtension_EventOverrides>();

			if (overrides == null)
				return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(ArgsNamesCustom.Partner));

			foreach (TwoPawnEventRule rule in overrides.overrideRules)
			{
				if (rule.Applies(pawn, partner))
					return rule.historyEventDef.CreateEventWithPartner(pawn, partner);
			}

			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(ArgsNamesCustom.Partner));
		}
	}
}
