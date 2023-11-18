using HarmonyLib;
using RimWorld;
using rjw;
using RJWSexperience.Cum;
using RJWSexperience.Logs;
using RJWSexperience.SexHistory;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(JobDriver_Sex), "Orgasm")] // Despite the name, called every tick
	public static class RJW_Patch_Orgasm
	{
		public static void Postfix(JobDriver_Sex __instance)
		{
			if (__instance.Sexprops.sexType != xxx.rjwSextype.Masturbation && !(__instance is JobDriver_Masturbate))
			{
				if (__instance.Sexprops.isRape && __instance.Sexprops.isReceiver)
				{
					__instance.pawn?.skills?.Learn(RsDefOf.Skill.Sex, 0.05f, true);
				}
				else
				{
					__instance.pawn?.skills?.Learn(RsDefOf.Skill.Sex, 0.35f, true);
				}
			}
		}
	}

	[HarmonyPatch(typeof(SexUtility), nameof(SexUtility.SatisfyPersonal))] // Actual on orgasm method
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
			props.pawn.records?.Increment(RsDefOf.Record.OrgasmCount);
			if (SexperienceMod.Settings.EnableSexHistory && props.hasPartner())
				props.pawn.TryGetComp<SexHistoryComp>()?.RecordOrgasm(props.partner, props, satisfaction);
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
			SkillRecord sexskill = pawn.skills.GetSkill(RsDefOf.Skill.Sex);
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

			if (!SexperienceMod.Settings.EnableSexHistory || !props.hasPartner())
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
			if (__instance.Sexprops.hasPartner())
			{
				// remove hetero virginity
				if((__instance.Sexprops.sexType == xxx.rjwSextype.Vaginal || __instance.Sexprops.sexType == xxx.rjwSextype.DoublePenetration))
				{
					__instance.pawn.TryRemoveVirginity(__instance.Partner, __instance.Sexprops);
					__instance.Partner.TryRemoveVirginity(__instance.pawn, __instance.Sexprops);
				} 
				else
                {
					// check if both pawn are male -> anal used as alternative virginity remover
					if(SexperienceMod.Settings.VirginityCheck_M2M_Anal &&
					   __instance.Sexprops.sexType == xxx.rjwSextype.Anal
					   && __instance.pawn.gender == Gender.Male && __instance.Partner.gender == Gender.Male)
					{
						__instance.pawn.TryRemoveVirginity(__instance.Partner, __instance.Sexprops);
						__instance.Partner.TryRemoveVirginity(__instance.pawn, __instance.Sexprops);
					}
					
					// check if both pawn are female -> scissoring used as alternative virginity remover
					if(SexperienceMod.Settings.VirginityCheck_F2F_Scissoring &&
					   __instance.Sexprops.sexType == xxx.rjwSextype.Scissoring
					   && __instance.pawn.gender == Gender.Female && __instance.Partner.gender == Gender.Female)
					{
						__instance.pawn.TryRemoveVirginity(__instance.Partner, __instance.Sexprops);
						__instance.Partner.TryRemoveVirginity(__instance.pawn, __instance.Sexprops);
					}
                }
			}
		}
	}

	[HarmonyPatch(typeof(CasualSex_Helper), nameof(CasualSex_Helper.FindSexLocation))]
	public static class RJW_Patch_CasualSex_Helper_FindSexLocation
	{
		/// <summary>
		/// If masturbation and current map has a bucket, return location near the bucket
		/// </summary>
		/// <param name="__result">The place to stand near a bucket</param>
		/// <returns>Run original method</returns>
		public static bool Prefix(Pawn pawn, Pawn partner, ref IntVec3 __result)
		{
			if (partner != null && partner != pawn)
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
				log.Message("404 Bucket not found");
				return true;
			}

			Room bucketRoom = bucket.GetRoom();

			List<IntVec3> cellsAroundBucket = GenAdjFast.AdjacentCells8Way(bucket.Position);
			IntVec3 doorNearBucket = IntVec3.Invalid;

			foreach (IntVec3 cell in cellsAroundBucket.InRandomOrder())
			{
				if (!cell.Standable(bucket.Map))
				{
					log.Message($"Discarded {cell}: not standable");
					continue;
				}

				if (cell.GetRoom(bucket.Map) != bucketRoom)
				{
					if (cell.GetDoor(bucket.Map) != null)
					{
						doorNearBucket = cell;
					}
					else
					{
						log.Message($"Discarded {cell}: different room");
					}

					continue;
				}

				__result = cell;
				log.Message($"Masturbate at location: {__result}");
				return false;
			}

			if (doorNearBucket != IntVec3.Invalid)
			{
				__result = doorNearBucket;
				log.Message($"No proper place found, go jack off in the doorway: {__result}");
				return false;
			}

			log.Message($"Failed to find situable location near the bucket at {bucket.Position}");
			return true;
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
