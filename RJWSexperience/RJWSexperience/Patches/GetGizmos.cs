using HarmonyLib;
using RJWSexperience.UI;
using System.Collections.Generic;
using System.Linq;
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
			if (history != null) yield return CreateHistoryGizmo(pawn, history);
		}

		private static Gizmo CreateHistoryGizmo(Pawn pawn, SexPartnerHistory history)
		{
			Gizmo gizmo = new Command_Action
			{
				defaultLabel = Keyed.RS_Sex_History,
				icon = HistoryUtility.HistoryIcon,
				defaultIconColor = HistoryUtility.HistoryColor,
				hotKey = VariousDefOf.OpenSexStatistics,
				action = delegate
				{
					SexStatusWindow.ToggleWindow(pawn, history);
				}
			};

			return gizmo;
		}
	}
}
