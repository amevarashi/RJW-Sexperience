using rjw;
using rjw.Modules.Interactions.Enums;
using RJWSexperience.ExtensionMethods;
using RJWSexperience.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RJWSexperience.Cum
{
	public static class CumUtility
	{
		private static readonly rjw.Modules.Shared.Logs.ILog log = LogManager.GetLogger<DebugLogProvider>("CumUtility");

		public static float GetOnePartCumVolume(Pawn pawn)
		{
			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
			if (hediffs.NullOrEmpty())
				return 0f;

			float result = GetCumVolume(pawn, GetOneBodyPartComp(hediffs));
			log.Message($"GetOnePartCumVolume({pawn.NameShortColored}) = {result}");
			return result;
		}

		public static float GetCumVolume(Pawn pawn)
		{
			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
			if (hediffs.NullOrEmpty())
				return 0f;
			float result = GetCumVolume(pawn, GetAllBodyPartComps(hediffs));
			log.Message($"GetCumVolume({pawn.NameShortColored}) = {result}");
			return result;
		}

		public static float GetCumVolume(Pawn pawn, List<CompHediffBodyPart> parts)
		{
			return parts.Select(part => GetCumVolume(pawn, part)).Aggregate((sum, x) => sum + x);
		}

		public static float GetCumVolume(Pawn pawn, CompHediffBodyPart part)
		{
			float res;

			try
			{
				res = part.FluidAmmount * part.FluidModifier * pawn.BodySize / pawn.RaceProps.baseBodySize * Rand.Range(0.8f, 1.2f) * 0.3f;
			}
			catch (NullReferenceException)
			{
				res = 0.0f;
			}
			if (pawn.Has(Quirk.Messy)) res *= Rand.Range(4.0f, 8.0f);

			return res;
		}

		public static List<CompHediffBodyPart> GetAllBodyPartComps(List<Hediff> hediffs)
		{
			List<CompHediffBodyPart> bodyPartComps = new List<CompHediffBodyPart>();

			foreach (Hediff bodyPart in hediffs)
			{
				CompHediffBodyPart bodyPartComp = bodyPart.TryGetComp<CompHediffBodyPart>();
				if (bodyPartComp != null)
					bodyPartComps.Add(bodyPartComp);
			}

			return bodyPartComps;
		}

		public static CompHediffBodyPart GetOneBodyPartComp(List<Hediff> hediffs)
		{
			CompHediffBodyPart part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("penis")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorf")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorm")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("tentacle")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();

			return part;
		}

		public static void TryFeedCum(SexProps props)
		{
			if (!Genital_Helper.has_penis_fertile(props.pawn))
				return;

			if (!PawnsPenisIsInPartnersMouth(props))
				return;

			float cumAmount = CumUtility.GetOnePartCumVolume(props.pawn);

			if (cumAmount <= 0)
				return;

			FeedCum(props.partner, cumAmount);
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

		private static void FeedCum(Pawn pawn, float amount)
		{
			const float allOf = 1000f;

			log.Message($"FeedCum({pawn.NameShortColored}, {amount})");
			Thing cum = ThingMaker.MakeThing(RsDefOf.Thing.GatheredCum);
			cum.stackCount = (int)Math.Ceiling(amount);
			log.Message($"Created a stack of {cum.stackCount} cum");
			cum.Ingested(pawn, allOf);
			log.Message($"{pawn.NameShortColored} ingested cum");
		}

		public static void FillCumBuckets(SexProps props)
		{
			xxx.rjwSextype sextype = props.sexType;

			bool sexFillsCumbuckets =
				// Base: Fill Cumbuckets on Masturbation. Having no partner means it must be masturbation too
				sextype == xxx.rjwSextype.Masturbation || props.partner == null
				// Depending on configuration, also fill cumbuckets when certain sextypes are matched 
				|| (SexperienceMod.Settings.SexCanFillBuckets && (sextype == xxx.rjwSextype.Boobjob || sextype == xxx.rjwSextype.Footjob || sextype == xxx.rjwSextype.Handjob));

			if (!sexFillsCumbuckets)
				return;

			// Enumerable throws System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
			List<Building_CumBucket> buckets = props.pawn.GetAdjacentBuildings<Building_CumBucket>().ToList();

			if (buckets?.Count > 0)
			{
				var initialCum = GetCumVolume(props.pawn);
				foreach (Building_CumBucket bucket in buckets)
				{
					bucket.AddCum(initialCum / buckets.Count);
				}
			}
		}
	}
}
