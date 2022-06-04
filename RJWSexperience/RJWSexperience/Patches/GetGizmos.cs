using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(Pawn), "GetGizmos")]
	public static class Pawn_GetGizmos
	{
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
