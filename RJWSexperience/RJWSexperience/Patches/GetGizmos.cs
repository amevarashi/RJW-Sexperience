using HarmonyLib;
using RJWSexperience.UI;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RJWSexperience
{
	[HarmonyPatch(typeof(Pawn), "GetGizmos")]
	public class Pawn_GetGizmos
	{
		public static void Postfix(ref IEnumerable<Gizmo> __result, Pawn __instance)
		{
			if (Find.Selector.NumSelected > 1)
				return;

			List<Gizmo> gizmoList = __result.ToList();

			AddHistoryGizmo(__instance, ref gizmoList);

			__result = gizmoList;
		}

		private static void AddHistoryGizmo(Pawn pawn, ref List<Gizmo> gizmos)
		{
			SexPartnerHistory history = pawn.GetPartnerHistory();
			if (history != null) gizmos.Add(CreateHIstoryGizmo(pawn, history));
		}

		private static Gizmo CreateHIstoryGizmo(Pawn pawn, SexPartnerHistory history)
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
