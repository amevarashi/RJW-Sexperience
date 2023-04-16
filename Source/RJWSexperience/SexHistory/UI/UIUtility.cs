using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public static class UIUtility
	{
		public const float FONTHEIGHT = 22f;
		public const float CARDHEIGHT = 110f;
		public const float LISTPAWNSIZE = 100f;
		public const float BASESAT = 0.40f;
		public const float ICONSIZE = 30f;

		public static string GetRelationsString(this Pawn pawn, Pawn otherpawn)
		{
			if (otherpawn != null)
			{
				IEnumerable<PawnRelationDef> relations = pawn.GetRelations(otherpawn);
				if (!relations.EnumerableNullOrEmpty())
					return relations.Select(x => x.GetGenderSpecificLabel(otherpawn)).ToCommaList().CapitalizeFirst();
			}
			return "";
		}

		public static void DrawBorder(this Rect rect, Texture border, float thickness = 1f)
		{
			GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), border);
			GUI.DrawTexture(new Rect(rect.x + rect.width - thickness, rect.y, thickness, rect.height), border);
			GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - thickness, rect.width, thickness), border);
			GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), border);
		}

		public static string GetSexDays(int absticks, bool printUnknown = false)
		{
			if (absticks != 0)
				return GenDate.ToStringTicksToDays(GenTicks.TicksAbs - absticks) + " " + Keyed.RS_Ago;
			else if (printUnknown)
				return Keyed.Unknown;
			return "";
		}

		public static Texture GetRaceIcon(Pawn pawn, Vector2 size)
		{
			if (pawn != null)
				return PortraitsCache.Get(pawn, size, Rot4.South, default, 1, true, true, false, false);
			return HistoryUtility.UnknownPawn;
		}

		public static void FillableBarLabeled(Rect rect, BarInfo context)
		{
			Widgets.FillableBar(rect, context.fillPercent, context.fillTexture, null, true);
			Rect labelRect = rect.ContractedBy(4f, 0f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(labelRect, context.label);
			if (context.labelRight != "")
			{
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(labelRect, context.labelRight);
			}
			GenUI.ResetLabelAlign();
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, context.tooltip);

			if (context.border != null)
			{
				rect.DrawBorder(context.border, 2f);
			}
		}

		public static void FillableBarLabeled(this Listing_Standard list, BarInfo context)
		{
			FillableBarLabeled(list.GetRect(FONTHEIGHT), context);
			list.Gap(1f);
		}
	}
}
