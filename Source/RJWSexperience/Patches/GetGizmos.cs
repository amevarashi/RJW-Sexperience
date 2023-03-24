using HarmonyLib;
using RJWSexperience.SexHistory;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
	public static class Pawn_GetGizmos
	{
		private static Configurations Settings => SexperienceMod.Settings;

		public static bool Prepare() => Settings.EnableSexHistory;

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
