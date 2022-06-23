using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Ideology
{
	[StaticConstructorOnStartup]
	public static class IdeoUtility
	{
		public static bool IsSubmissive(this Pawn pawn)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
				return false;

			if (ideo.HasPrecept(VariousDefOf.Submissive_Female) && pawn.gender == Gender.Female)
				return true;
			else if (ideo.HasPrecept(VariousDefOf.Submissive_Male) && pawn.gender == Gender.Male)
				return true;

			return false;
		}

		public static bool ContainAll(string str, string[] tags)
		{
			if (tags.NullOrEmpty())
				return true;

			string lstr = str.ToLower();
			for (int i = 0; i < tags.Length; i++)
			{
				if (!lstr.Contains('[' + tags[i].ToLower() + ']'))
					return false;
			}
			return true;
		}

		public static bool IsIncest(Pawn pawn, Pawn partner)
		{
			IEnumerable<PawnRelationDef> relations = pawn.GetRelations(partner);
			if (relations.EnumerableNullOrEmpty())
				return false;

			bool wide = pawn.Ideo?.HasPrecept(VariousDefOf.Incestuos_Disapproved_CloseOnly) == true;
			foreach (PawnRelationDef relation in relations)
			{
				if (wide)
				{
					if (relation.incestOpinionOffset < 0)
						return true;
				}
				else if (relation.familyByBloodRelation)
				{
					return true;
				}
			}
			return false;
		}

		public static float GetPreceptsMtbMultiplier<T>(Ideo ideo) where T : Precepts.DefExtension_ModifyMtb
		{
			float finalMultiplier = 1f;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
			{
				T defExtension = ideo.PreceptsListForReading[i].def.GetModExtension<T>();

				if (defExtension == null)
					continue;

				if (defExtension.disable)
					return -1f;

				finalMultiplier *= defExtension.multiplier;
			}
			return finalMultiplier;
		}

		public static HistoryEventDef GetSextypeEventDef(xxx.rjwSextype sextype)
		{
			if (historyEventBySextype.TryGetValue(sextype, out HistoryEventDef historyEventDef))
				return historyEventDef;
			return null;
		}

		public static HistoryEventDef GetSextypeEventDef(string sextype)
		{
			if (!Enum.TryParse(sextype, out xxx.rjwSextype rjwSextype))
				return null;
			return GetSextypeEventDef(rjwSextype);
		}

		private static readonly Dictionary<xxx.rjwSextype, HistoryEventDef> historyEventBySextype = BuildHistoryEventBySextype();

		private static Dictionary<xxx.rjwSextype, HistoryEventDef> BuildHistoryEventBySextype()
		{
			Dictionary<xxx.rjwSextype, HistoryEventDef> dictionary = new Dictionary<xxx.rjwSextype, HistoryEventDef>();
			foreach (HistoryEventDef historyEventDef in DefDatabase<HistoryEventDef>.AllDefsListForReading)
			{
				HistoryEventDefExtension_AssociatedSextypes associatedSextypes = historyEventDef.GetModExtension<HistoryEventDefExtension_AssociatedSextypes>();
				if (associatedSextypes == null)
					continue;
				foreach (xxx.rjwSextype sextype in associatedSextypes.sextypes)
				{
					if (!dictionary.TryAdd(sextype, historyEventDef))
						Log.Error($"[Sexperience.Ideology] Error in HistoryEventDef {historyEventDef.defName}: {sextype} sextype is already associated with {dictionary[sextype].defName} HistoryEventDef");
				}
			}
			return dictionary;
		}
	}
}
