using rjw;
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
				res = part.FluidAmmount * part.FluidModifier * pawn.BodySize / pawn.RaceProps.baseBodySize * Rand.Range(0.8f, 1.2f) * RJWSettings.cum_on_body_amount_adjust * 0.3f;
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

		public static void FeedCum(Pawn pawn, float amount)
		{
			const float allOf = 1000f;

			log.Message($"FeedCum({pawn.NameShortColored}, {amount})");
			Thing cum = ThingMaker.MakeThing(VariousDefOf.GatheredCum);
			cum.stackCount = (int)Math.Ceiling(amount);
			log.Message($"Created a stack of {cum.stackCount} cum");
			cum.Ingested(pawn, allOf);
			log.Message($"{pawn.NameShortColored} ingested cum");
		}
	}
}
