using RimWorld;
using RJWSexperience.Ideology.HistoryEvents;
using Verse;

namespace RJWSexperience.Ideology
{
	public static class RJWUtility_Ideo
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

		public static HistoryEvent CreateTaggedEvent(this HistoryEventDef def, Pawn pawn, string tag, Pawn partner)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), tag.Named(ArgsNamesCustom.Tag), partner.Named(ArgsNamesCustom.Partner));
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

		public static Faction GetFactionUsingPrecept(this Pawn baby, out Ideo ideo)
		{
			Faction playerfaction = Find.FactionManager.OfPlayer;
			Ideo mainideo = playerfaction.ideos.PrimaryIdeo;
			if (mainideo != null)
			{
				if (mainideo.HasPrecept(VariousDefOf.BabyFaction_AlwaysFather))
				{
					Pawn parent = baby.GetFather() ?? baby.GetMother();

					ideo = parent.Ideo;
					return parent.Faction;
				}
				else if (mainideo.HasPrecept(VariousDefOf.BabyFaction_AlwaysColony))
				{
					ideo = mainideo;
					return playerfaction;
				}
			}
			Pawn mother = baby.GetMother();
			ideo = mother?.Ideo;
			return mother?.Faction ?? baby.Faction;
		}
	}
}
