using HarmonyLib;
using RimWorld;
using rjw;
using rjw.Modules.Interactions.Internals.Implementation;
using rjw.Modules.Interactions.Objects;
using RJWSexperience.Ideology.Precepts;
using System;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	public static class RJWUtility_Ideo
	{
		public static HistoryEvent CreateTaggedEvent(this HistoryEventDef def, Pawn pawn, string tag, Pawn partner)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), tag.Named(HistoryEventArgsNamesCustom.Tag), partner.Named(HistoryEventArgsNamesCustom.Partner));
		}

		public static HistoryEvent CreateEvent(this HistoryEventDef def, Pawn pawn)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer));
		}

		public static HistoryEvent CreateEventWithPartner(this HistoryEventDef def, Pawn pawn, Pawn partner)
		{
			HistoryEventDefExtension_PartnerDependentOverrides overrides = def.GetModExtension<HistoryEventDefExtension_PartnerDependentOverrides>();

			if (overrides == null)
				return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(HistoryEventArgsNamesCustom.Partner));

			foreach (var rule in overrides.overrideRules)
			{
				if (rule.Applies(pawn, partner))
					return rule.historyEventDef.CreateEventWithPartner(pawn, partner);
			}

			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer), partner.Named(HistoryEventArgsNamesCustom.Partner));
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

	[HarmonyPatch(typeof(xxx), "is_rapist")]
	public static class RJW_Patch_is_rapist
	{
		public static void Postfix(Pawn pawn, ref bool __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null && !pawn.IsSubmissive())
			{
				__result = __result || ideo.HasMeme(VariousDefOf.Rapist);
			}
		}
	}

	[HarmonyPatch(typeof(xxx), "is_zoophile")]
	public static class RJW_Patch_is_zoophile
	{
		public static void Postfix(Pawn pawn, ref bool __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null)
			{
				__result = __result || ideo.HasMeme(VariousDefOf.Zoophile);
			}
		}
	}

	[HarmonyPatch(typeof(xxx), "is_necrophiliac")]
	public static class RJW_Patch_is_necrophiliac
	{
		public static void Postfix(Pawn pawn, ref bool __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo != null)
			{
				__result = __result || ideo.HasMeme(VariousDefOf.Necrophile);
			}
		}
	}

	[HarmonyPatch(typeof(SexUtility), "Aftersex", new Type[] { typeof(SexProps) })]
	public static class RJW_Patch_Aftersex
	{
		public static void Postfix(SexProps props)
		{
			InteractionDefExtension_HistoryEvents interactionEvents = props.dictionaryKey.GetModExtension<InteractionDefExtension_HistoryEvents>();

			if (props.hasPartner())
			{
				if (xxx.is_human(props.pawn))
					AfterSexHuman(props.pawn, props.partner, props.isRape);
				else if (xxx.is_human(props.partner))
					AfterSexHuman(props.partner, props.pawn, false);

				if (xxx.is_human(props.partner) && props.isRape)
				{
					if (props.partner.IsPrisoner)
						props.partner.guest.will = Math.Max(0, props.partner.guest.will - 0.2f);
					if (props.partner.IsSlave)
						RapeEffectSlave(props.partner);
				}

				if (interactionEvents != null)
				{
					foreach (HistoryEventDef eventDef in interactionEvents.pawnEvents)
						Find.HistoryEventsManager.RecordEvent(eventDef.CreateEventWithPartner(props.pawn, props.partner));

					foreach (HistoryEventDef eventDef in interactionEvents.partnerEvents)
						Find.HistoryEventsManager.RecordEvent(eventDef.CreateEventWithPartner(props.partner, props.pawn));
				}
			}
			else
			{
				if (interactionEvents != null)
				{
					foreach (HistoryEventDef eventDef in interactionEvents.pawnEvents)
						Find.HistoryEventsManager.RecordEvent(eventDef.CreateEvent(props.pawn));
				}
			}
		}

		public static void AfterSexHuman(Pawn human, Pawn partner, bool rape)
		{
			Find.HistoryEventsManager.RecordEvent(VariousDefOf.RSI_NonIncestuosSex.CreateEventWithPartner(human, partner));
			Find.HistoryEventsManager.RecordEvent(VariousDefOf.RSI_NonIncestuosSex.CreateEventWithPartner(partner, human));

			if (partner.IsAnimal())
			{
				if (human.Ideo?.IsVeneratedAnimal(partner) ?? false)
					Find.HistoryEventsManager.RecordEvent(VariousDefOf.SexWithVeneratedAnimal.CreateEventWithPartner(human, partner));
				else
					Find.HistoryEventsManager.RecordEvent(VariousDefOf.SexWithNonVeneratedAnimal.CreateEventWithPartner(human, partner));

				if (human.Ideo != null && human.relations?.DirectRelationExists(PawnRelationDefOf.Bond, partner) == true)
					Find.HistoryEventsManager.RecordEvent(VariousDefOf.SexWithBondedAnimal.CreateEventWithPartner(human, partner));
				else
					Find.HistoryEventsManager.RecordEvent(VariousDefOf.SexWithNonBondAnimal.CreateEventWithPartner(human, partner));

				Find.HistoryEventsManager.RecordEvent(VariousDefOf.SexWithAnimal.CreateEventWithPartner(human, partner));
			}
			else if (xxx.is_human(partner))
			{
				if (rape)
				{
					if (partner.IsSlave)
					{
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.RapedSlave.CreateTaggedEvent(human, HETag.Rape + HETag.Gender(human), partner));
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.WasRapedSlave.CreateTaggedEvent(partner, HETag.BeenRaped + HETag.Gender(partner), human));
					}
					else if (partner.IsPrisoner)
					{
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.RapedPrisoner.CreateTaggedEvent(human, HETag.Rape + HETag.Gender(human), partner));
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.WasRapedPrisoner.CreateTaggedEvent(partner, HETag.BeenRaped + HETag.Gender(partner), human));
					}
					else
					{
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.Raped.CreateTaggedEvent(human, HETag.Rape + HETag.Gender(human), partner));
						Find.HistoryEventsManager.RecordEvent(VariousDefOf.WasRaped.CreateTaggedEvent(partner, HETag.BeenRaped + HETag.Gender(partner), human));
					}
				}
			}
		}

		public static void RapeEffectSlave(Pawn victim)
		{
			Need_Suppression suppression = victim.needs.TryGetNeed<Need_Suppression>();
			if (suppression != null)
			{
				Hediff broken = victim.health.hediffSet.GetFirstHediffOfDef(xxx.feelingBroken);
				if (broken != null) suppression.CurLevel += (0.3f * broken.Severity) + 0.05f;
				else suppression.CurLevel += 0.05f;
			}
		}
	}

	/// <summary>
	/// Set prefer sextype using precepts
	/// </summary>
	[HarmonyPatch(typeof(InteractionScoringService), nameof(InteractionScoringService.Score), new Type[] { typeof(InteractionWithExtension), typeof(InteractionPawn), typeof(InteractionPawn) })]
	public static class RJW_Patch_DetermineSexScores
	{
		public static void Postfix(InteractionWithExtension interaction, InteractionPawn dominant, InteractionPawn submissive, ref InteractionScore __result)
		{
			InteractionDefExtension_HistoryEvents interactionEvents = interaction.Interaction.GetModExtension<InteractionDefExtension_HistoryEvents>();
			if (interactionEvents == null)
				return;

			Ideo ideo = dominant.Pawn.Ideo;
			if (ideo != null)
				__result.Dominant = PreceptSextype(ideo, dominant.Pawn.GetStatValue(xxx.sex_drive_stat), __result.Dominant, interactionEvents.pawnEvents);

			ideo = submissive.Pawn.Ideo;
			if (ideo != null)
				__result.Submissive = PreceptSextype(ideo, submissive.Pawn.GetStatValue(xxx.sex_drive_stat), __result.Submissive, interactionEvents.partnerEvents);
		}

		public static float PreceptSextype(Ideo ideo, float sexdrive, float score, List<HistoryEventDef> historyEventDefs)
		{
			foreach(HistoryEventDef eventDef in historyEventDefs)
			{
				if (ideo.MemberWillingToDo(new HistoryEvent(eventDef)))
				{
					float mult = 8.0f * Math.Max(0.3f, 1 / Math.Max(0.01f, sexdrive));
					return score * mult;
				}
			}
			return score;
		}
	}

	[HarmonyPatch(typeof(SexAppraiser), nameof(SexAppraiser.would_fuck), new Type[] { typeof(Pawn), typeof(Pawn), typeof(bool), typeof(bool), typeof(bool) })]
	public static class RJW_Patch_would_fuck
	{
		public static void Postfix(Pawn fucker, Pawn fucked, ref float __result)
		{
			if (!xxx.is_human(fucker))
				return;

			Ideo ideo = fucker.Ideo;
			if (ideo == null)
				return;

			for(int i = 0; i < ideo.PreceptsListForReading.Count; i++)
				ideo.PreceptsListForReading[i].def.GetModExtension<DefExtension_ModifyPreference>()?.Apply(fucker, fucked, ref __result);
		}
	}

	[HarmonyPatch(typeof(PawnDesignations_Breedee), "UpdateCanDesignateBreeding")]
	public static class RJW_Patch_UpdateCanDesignateBreeding
	{
		public static void Postfix(Pawn pawn, ref bool __result)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo?.HasMeme(VariousDefOf.Zoophile) == true)
			{
				SaveStorage.DataStore.GetPawnData(pawn).CanDesignateBreeding = true;
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(PawnDesignations_Comfort), "UpdateCanDesignateComfort")]
	public static class RJW_PatchUpdateCanDesignateComfort
	{
		public static void Postfix(Pawn pawn, ref bool __result)
		{
			if (pawn.IsSubmissive())
			{
				SaveStorage.DataStore.GetPawnData(pawn).CanDesignateComfort = true;
				__result = true;
			}
		}
	}

	[HarmonyPatch(typeof(Hediff_BasePregnancy), "PostBirth")]
	public static class RJW_Patch_PostBirth
	{
		public static void Postfix(Pawn mother, Pawn father, Pawn baby)
		{
			if (!mother.IsAnimal())
			{
				Faction faction = baby.GetFactionUsingPrecept(out Ideo ideo);
				if (baby.Faction != faction) baby.SetFaction(faction);
				baby.ideo?.SetIdeo(ideo);
				if (baby.Faction == Find.FactionManager.OfPlayer && !baby.IsSlave) baby.guest?.SetGuestStatus(null, GuestStatus.Guest);
			}
		}
	}
}
