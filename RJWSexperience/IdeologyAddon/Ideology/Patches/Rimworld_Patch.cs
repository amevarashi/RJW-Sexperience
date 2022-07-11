using HarmonyLib;
using RimWorld;
using rjw;
using RJWSexperience.Ideology.Precepts;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Ideology.Patches
{
	[HarmonyPatch(typeof(MarriageCeremonyUtility), nameof(MarriageCeremonyUtility.Married))]
	public static class Rimworld_Patch_Marriage
	{
		public static void Postfix(Pawn firstPawn, Pawn secondPawn)
		{
			RsiHistoryEventDefOf.RSI_NonIncestuosMarriage.RecordEventWithPartner(firstPawn, secondPawn);
			RsiHistoryEventDefOf.RSI_NonIncestuosMarriage.RecordEventWithPartner(secondPawn, firstPawn);
		}
	}

	[HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality), "GiveMemoryToPawn")]
	public static class Rimworld_Patch_RitualOutcome_DontGiveMemoryToAnimals
	{
		public static bool Prefix(Pawn pawn)
		{
			return !pawn.IsAnimal();
		}
	}

	[HarmonyPatch(typeof(IdeoFoundation), nameof(IdeoFoundation.CanAdd))]
	public static class Rimworld_Patch_IdeoFoundation
	{
		public static void Postfix(PreceptDef precept, ref IdeoFoundation __instance, ref AcceptanceReport __result)
		{
			DefExtension_MultipleMemesRequired extension = precept.GetModExtension<DefExtension_MultipleMemesRequired>();

			if (extension == null)
				return;

			if (extension.requiredAllMemes.NullOrEmpty())
				return;

			for (int i = 0; i < extension.requiredAllMemes.Count; i++)
			{
				if (!__instance.ideo.memes.Contains(extension.requiredAllMemes[i]))
				{
					List<string> report = new List<string>();
					foreach (MemeDef meme in extension.requiredAllMemes) report.Add(meme.LabelCap);

					__result = new AcceptanceReport("RequiresMeme".Translate() + ": " + report.ToCommaList());
					return;
				}
			}
		}
	}
}
