using HarmonyLib;
using RJWSexperience.Logs;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace RJWSexperience
{
	public static class Pawn_GetGizmos
	{
		public static void DoConditionalPatch(Harmony harmony)
		{
			if (!SexperienceMod.Settings.History.EnableSexHistory)
				return;

			MethodInfo original = typeof(Pawn).GetMethod(nameof(Pawn.GetGizmos));
			MethodInfo postfix = typeof(Pawn_GetGizmos).GetMethod(nameof(Pawn_GetGizmos.Postfix));
			harmony.Patch(original, postfix: new HarmonyMethod(postfix));

			LogManager.GetLogger<DebugLogProvider>(nameof(Pawn_GetGizmos)).Message("Applied conditional patch to Pawn.GetGizmos()");
		}

		public static void Postfix(ref IEnumerable<Gizmo> __result, Pawn __instance)
		{
			if (Find.Selector.NumSelected > 1)
				return;

			SexHistoryComp history = __instance.TryGetComp<SexHistoryComp>();
			if (history == null)
				return;

			__result = AddHistoryGizmo(history, __result);
		}

		private static IEnumerable<Gizmo> AddHistoryGizmo(SexHistoryComp history, IEnumerable<Gizmo> gizmos)
		{
			foreach (Gizmo gizmo in gizmos)
				yield return gizmo;

			yield return history.Gizmo;
		}
	}
}
