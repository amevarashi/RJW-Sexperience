using HarmonyLib;
using RJWSexperience.Logs;
using RJWSexperience.SexHistory;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace RJWSexperience
{
	public static class Pawn_GetGizmos
	{
		private static Settings.SettingsTabHistory Settings => SexperienceMod.Settings.History;

		public static void DoConditionalPatch(Harmony harmony)
		{
			if (!Settings.EnableSexHistory)
				return;

			MethodInfo original = typeof(Pawn).GetMethod(nameof(Pawn.GetGizmos));
			MethodInfo postfix = typeof(Pawn_GetGizmos).GetMethod(nameof(Pawn_GetGizmos.Postfix));
			harmony.Patch(original, postfix: new HarmonyMethod(postfix));

			LogManager.GetLogger<DebugLogProvider>(nameof(Pawn_GetGizmos)).Message("Applied conditional patch to Pawn.GetGizmos()");
		}

		public static void Postfix(ref IEnumerable<Gizmo> __result, Pawn __instance)
		{
			if (Settings.HideGizmoWhenDrafted && __instance.Drafted)
				return;

			if (Find.Selector.NumSelected > 1)
				return;

			if (Settings.HideGizmoWithRJW && !rjw.RJWSettings.show_RJW_designation_box)
				return;

			SexHistoryComp history = __instance.TryGetComp<SexHistoryComp>();
			if (history == null)
				return;

			__result = AddHistoryGizmo(history.Gizmo, __result);
		}

		private static IEnumerable<Gizmo> AddHistoryGizmo(Gizmo historyGizmo, IEnumerable<Gizmo> gizmos)
		{
			foreach (Gizmo gizmo in gizmos)
				yield return gizmo;

			yield return historyGizmo;
		}
	}
}
