using HarmonyLib;
using RimWorld;
using rjw;
using rjw.Modules.Interactions.Enums;
using RJWSexperience.ExtensionMethods;
using UnityEngine;
using Verse;
using Verse.AI;

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

	[HarmonyPatch(typeof(SexUtility), "SatisfyPersonal")]
	public static class RJW_Patch_SatisfyPersonal
	{
		private const float base_sat_per_fuck = 0.4f;

		public static void Prefix(SexProps props, ref float satisfaction)
		{
			satisfaction = Mathf.Max(base_sat_per_fuck, satisfaction * props.partner.GetSexStat());
		}

		public static void Postfix(SexProps props, ref float satisfaction)
		{
			Pawn pawn = props.pawn;
			Pawn partner = props.partner;
			UpdateLust(props, satisfaction);

			if (props.sexType == xxx.rjwSextype.Masturbation || partner == null)
			{
				Building_CumBucket cumbucket = pawn.GetAdjacentBuilding<Building_CumBucket>();
				cumbucket?.AddCum(pawn.GetCumVolume());
			}

			RJWUtility.UpdateSatisfactionHIstory(pawn, partner, props, satisfaction);
			pawn.records?.Increment(VariousDefOf.OrgasmCount);
		}

		private static void UpdateLust(SexProps props, float satisfaction)
		{
			float? lust = props.pawn.records?.GetValue(VariousDefOf.Lust);

			if (lust == null)
				return;

			float lustDelta;

			if (props.sexType != xxx.rjwSextype.Masturbation)
			{
				lustDelta = satisfaction - base_sat_per_fuck;
				if (Mathf.Sign(lustDelta) == Mathf.Sign((float)lust)) // Only if getting closer to the limit
					lustDelta *= LustIncrementFactor((float)lust);
				lustDelta = Mathf.Clamp(lustDelta, -SexperienceMod.Settings.MaxSingleLustChange, SexperienceMod.Settings.MaxSingleLustChange); // If the sex is satisfactory, lust grows up. Declines at the opposite.
			}
			else
			{
				lustDelta = Mathf.Clamp(satisfaction * satisfaction * LustIncrementFactor((float)lust), 0, SexperienceMod.Settings.MaxSingleLustChange); // Masturbation always increases lust.
			}

			if (lustDelta == 0)
				return;

			rjw.Modules.Shared.Logs.LogManager.GetLogger<SexperienceMod>().Message($"{props.pawn.NameShortColored}'s lust changed by {lustDelta} (from {lust})");
			props.pawn.records.AddTo(VariousDefOf.Lust, lustDelta);
		}

		private static float LustIncrementFactor(float lust)
		{
			return Mathf.Exp(-Mathf.Pow(lust / SexperienceMod.Settings.LustLimit, 2));
		}

	}

	[HarmonyPatch(typeof(SexUtility), "TransferNutrition")]
	public static class RJW_Patch_TransferNutrition
	{
		public static void Postfix(SexProps props)
		{
			TryFeedCum(props);
		}

		private static void TryFeedCum(SexProps props)
		{
			if (!Genital_Helper.has_penis_fertile(props.pawn))
				return;

			if (!PawnsPenisIsInPartnersMouth(props))
				return;

			props.partner.AteCum(props.pawn.GetCumVolume(), true);
		}

		private static bool PawnsPenisIsInPartnersMouth(SexProps props)
		{
			var interaction = rjw.Modules.Interactions.Helpers.InteractionHelper.GetWithExtension(props.dictionaryKey);

			if (props.pawn == props.GetInteractionInitiator())
			{
				if (!interaction.DominantHasTag(GenitalTag.CanPenetrate) && !interaction.DominantHasFamily(GenitalFamily.Penis))
					return false;
				var requirement = interaction.SelectorExtension.submissiveRequirement;
				if (!requirement.mouth && !requirement.beak && !requirement.mouthORbeak)
					return false;
			}
			else
			{
				if (!interaction.SubmissiveHasTag(GenitalTag.CanPenetrate) && !interaction.SubmissiveHasFamily(GenitalFamily.Penis))
					return false;
				var requirement = interaction.SelectorExtension.dominantRequirement;
				if (!requirement.mouth && !requirement.beak && !requirement.mouthORbeak)
					return false;
			}

			return true;
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
			RJWUtility.UpdatePartnerHistory(props.pawn, props.partner, props);
			RJWUtility.UpdatePartnerHistory(props.partner, props.pawn, props);
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

	[HarmonyPatch(typeof(WorkGiver_CleanSelf), "JobOnThing")]
	public static class RJW_Patch_CleanSelf_JobOnThing
	{
		public static bool Prefix(Pawn pawn, Thing t, bool forced, ref Job __result)
		{
			Building_CumBucket bucket = pawn.GetAdjacentBuilding<Building_CumBucket>();
			if (bucket == null) bucket = pawn.FindClosestBucket();
			if (bucket != null)
			{
				__result = JobMaker.MakeJob(VariousDefOf.CleanSelfwithBucket, null, bucket, bucket.Position);
				return false;
			}

			return true;
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

			Log.Message($"CasualSex_Helper.FindSexLocation for {pawn.NameShortColored}");

			if (!pawn.Faction?.IsPlayer ?? true)
			{
				Log.Message("Not player faction");
				return true;
			}

			Building_CumBucket bucket = pawn.FindClosestBucket();

			if (bucket == null)
			{
				Log.Message("Bucket not found");
				return true;
			}

			__result = bucket.RandomAdjacentCell8Way();
			Log.Message($"Bucket location: {__result}");
			return false;
		}
	}
}
