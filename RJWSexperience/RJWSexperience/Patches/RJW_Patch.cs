using HarmonyLib;
using RimWorld;
using rjw;
using RJWSexperience.Cum;
using RJWSexperience.Logs;
using RJWSexperience.SexHistory;
using System;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(JobDriver_Sex), "Orgasm")]
	public static class RJW_Patch_Orgasm
	{
		public static void Postfix(JobDriver_Sex __instance)
		{
			if (__instance.Sexprops.sexType != xxx.rjwSextype.Masturbation && !(__instance is JobDriver_Masturbate))
			{
				if (__instance.Sexprops.isRape && __instance.Sexprops.isReceiver)
				{
					__instance.pawn?.skills?.Learn(VariousDefOf.SexSkill, 0.05f, true);
				}
				else
				{
					__instance.pawn?.skills?.Learn(VariousDefOf.SexSkill, 0.35f, true);
				}
			}
		}
	}

	[HarmonyPatch(typeof(WhoringHelper), "WhoreAbilityAdjustmentMin")]
	public static class RJW_Patch_WhoreAbilityAdjustmentMin
	{
		public static void Postfix(Pawn whore, ref float __result)
		{
			__result *= whore.GetSexStat();
		}
	}

	[HarmonyPatch(typeof(WhoringHelper), "WhoreAbilityAdjustmentMax")]
	public static class RJW_Patch_WhoreAbilityAdjustmentMax
	{
		public static void Postfix(Pawn whore, ref float __result)
		{
			__result *= whore.GetSexStat();
		}
	}

	[HarmonyPatch(typeof(SexUtility), nameof(SexUtility.SatisfyPersonal))]
	public static class RJW_Patch_SatisfyPersonal
	{
		private const float base_sat_per_fuck = 0.4f;

		public static void Prefix(SexProps props, ref float satisfaction)
		{
			satisfaction = Mathf.Max(base_sat_per_fuck, satisfaction * props.partner.GetSexStat());
		}

		public static void Postfix(SexProps props, ref float satisfaction)
		{
			LustUtility.UpdateLust(props, satisfaction, base_sat_per_fuck);
			CumUtility.FillCumBuckets(props);
			props.pawn.records?.Increment(VariousDefOf.OrgasmCount);
			if (SexperienceMod.Settings.History.EnableSexHistory && props.partner != null)
				props.pawn.TryGetComp<SexHistoryComp>()?.RecordSatisfaction(props.partner, props, satisfaction);
		}
	}

	[HarmonyPatch(typeof(SexUtility), "TransferNutrition")]
	public static class RJW_Patch_TransferNutrition
	{
		public static void Postfix(SexProps props)
		{
			CumUtility.TryFeedCum(props);
		}
	}

	[HarmonyPatch(typeof(Nymph_Generator), "set_skills")]
	public static class RJW_Patch_Nymph_set_skills
	{
		public static void Postfix(Pawn pawn)
		{
			SkillRecord sexskill = pawn.skills.GetSkill(VariousDefOf.SexSkill);
			if (sexskill != null)
			{
				sexskill.passion = Passion.Major;
				sexskill.Level = (int)Utility.RandGaussianLike(7f, 20.99f);
				sexskill.xpSinceLastLevel = sexskill.XpRequiredForLevelUp * Rand.Range(0.10f, 0.90f);
			}
		}
	}

	[HarmonyPatch(typeof(AfterSexUtility), "UpdateRecords")]
	public static class RJW_Patch_UpdateRecords
	{
		public static void Postfix(SexProps props)
		{
			RJWUtility.UpdateSextypeRecords(props);

			if (!SexperienceMod.Settings.History.EnableSexHistory || props.partner == null)
				return;

			props.pawn.TryGetComp<SexHistoryComp>()?.RecordSex(props.partner, props);
			props.partner.TryGetComp<SexHistoryComp>()?.RecordSex(props.pawn, props);
		}
	}

	[HarmonyPatch(typeof(JobDriver_SexBaseInitiator), "Start")]
	public static class RJW_Patch_LogSextype
	{
		public static void Postfix(JobDriver_SexBaseInitiator __instance)
		{
			if (__instance.Partner != null)
			{
				__instance.pawn.PoptheCherry(__instance.Partner, __instance.Sexprops);
				__instance.Partner.PoptheCherry(__instance.pawn, __instance.Sexprops);
			}
		}
	}

	[HarmonyPatch(typeof(CasualSex_Helper), nameof(CasualSex_Helper.FindSexLocation))]
	public static class RJW_Patch_CasualSex_Helper_FindSexLocation
	{
		/// <summary>
		/// If masturbation and current map has a bucket, return location near the bucket
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="partner"></param>
		/// <param name="__result"></param>
		/// <returns></returns>
		public static bool Prefix(Pawn pawn, Pawn partner, ref IntVec3 __result)
		{
			if (partner != null)
				return true; // Not masturbation

			var log = LogManager.GetLogger<DebugLogProvider>("RJW_Patch_CasualSex_Helper_FindSexLocation");
			log.Message($"Called for {pawn.NameShortColored}");

			if (pawn.Faction?.IsPlayer != true && !pawn.IsPrisonerOfColony)
			{
				log.Message("Not a player's faction or a prisoner");
				return true;
			}

			Building_CumBucket bucket = pawn.FindClosestBucket();

			if (bucket == null)
			{
				log.Message("Bucket not found");
				return true;
			}

			__result = bucket.RandomAdjacentCell8Way();
			log.Message($"Bucket location: {__result}");
			return false;
		}
	}

	[HarmonyPatch(typeof(SexUtility), nameof(SexUtility.Aftersex), new Type[] { typeof(SexProps) })]
	public static class RJW_Patch_SexUtility_Aftersex_RapeEffects
	{
		public static void Postfix(SexProps props)
		{
			if (!props.hasPartner() || !props.isRape || !xxx.is_human(props.partner))
				return;

			if (props.partner.IsPrisoner)
				RapeEffectPrisoner(props.partner);

			if (props.partner.IsSlave)
				RapeEffectSlave(props.partner);
		}

		private static void RapeEffectPrisoner(Pawn victim)
		{
			victim.guest.will = Math.Max(0, victim.guest.will - 0.2f);
		}

		private static void RapeEffectSlave(Pawn victim)
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
}
