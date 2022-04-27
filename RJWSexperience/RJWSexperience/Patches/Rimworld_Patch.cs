using HarmonyLib;
using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
	public static class Rimworld_Patch_GeneratePawn
	{
		public static void Postfix(PawnGenerationRequest request, ref Pawn __result)
		{
			if (SexperienceMod.Settings.EnableRecordRandomizer && __result != null && !request.Newborn && xxx.is_human(__result))
			{
				RecordRandomizer.Randomize(__result);
			}
			__result.AddVirginTrait();
		}
	}

	[HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
	public static class HumanlikeOrder_Patch
	{
		public static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			var targets = GenUI.TargetsAt(clickPos, TargetingParameters.ForBuilding());

			if (pawn.health.hediffSet.HasHediff(RJW_SemenoOverlayHediffDefOf.Hediff_Bukkake))
				foreach (LocalTargetInfo t in targets)
				{
					if (t.Thing is Building_CumBucket building)
					{
						opts.AddDistinct(MakeMenu(pawn, building));
						break;
					}
				}
		}

		public static FloatMenuOption MakeMenu(Pawn pawn, LocalTargetInfo target)
		{
			FloatMenuOption option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(Keyed.RS_FloatMenu_CleanSelf, delegate ()
			{
				pawn.jobs.TryTakeOrderedJob(new Verse.AI.Job(VariousDefOf.CleanSelfwithBucket, null, target, target.Cell));
			}, MenuOptionPriority.Low), pawn, target);

			return option;
		}
	}
}
