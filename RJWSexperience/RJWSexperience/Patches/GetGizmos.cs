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

			__result = AddHistoryGizmo(__instance, __result);
		}

		private static IEnumerable<Gizmo> AddHistoryGizmo(Pawn pawn, IEnumerable<Gizmo> gizmos)
		{
			foreach (Gizmo gizmo in gizmos)
				yield return gizmo;

			SexPartnerHistory history = pawn.GetPartnerHistory();
			if (history != null)
				yield return history.Gizmo;
		}
	}
}
