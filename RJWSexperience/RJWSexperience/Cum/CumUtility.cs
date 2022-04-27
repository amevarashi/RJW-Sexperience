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
		public static float GetCumVolume(Pawn pawn)
		{
			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
			if (hediffs.NullOrEmpty()) return 0f;
			else return GetCumVolume(pawn, hediffs);
		}

		public static float GetCumVolume(Pawn pawn, List<Hediff> hediffs)
		{
			CompHediffBodyPart part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("penis")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorf")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorm")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("tentacle")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) return 0f;

			return GetCumVolume(pawn, part);
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

		public static void FeedCum(Pawn pawn, float amount)
		{
			const float allOf = 1000f;

			var log = LogManager.GetLogger<DebugLogProvider>("PawnExtensions");
			log.Message($"AteCum({pawn.NameShortColored}, {amount})");

			Thing cum = ThingMaker.MakeThing(VariousDefOf.GatheredCum);
			cum.stackCount = (int)Math.Ceiling(amount);
			log.Message($"Created a stack of {cum.stackCount} cum");
			cum.Ingested(pawn, allOf);
			log.Message($"{pawn.NameShortColored} ingested cum");
		}
	}
}
