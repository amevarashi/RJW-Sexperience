using RimWorld;
using rjw;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience
{
	public static class PawnExtensions
	{
		public static bool IsIncest(this Pawn pawn, Pawn otherpawn)
		{
			if (otherpawn == null)
				return false;

			IEnumerable<PawnRelationDef> relations = pawn.GetRelations(otherpawn);
			if (relations.EnumerableNullOrEmpty())
				return false;

			foreach (PawnRelationDef relation in relations)
			{
				if (relation.incestOpinionOffset < 0)
					return true;
			}
			return false;
		}

		public static float GetSexStat(this Pawn pawn)
		{
			if (xxx.is_human(pawn) && !pawn.Dead)
				return pawn.GetStatValue(xxx.sex_stat);
			return 1.0f;
		}

		public static T GetAdjacentBuilding<T>(this Pawn pawn) where T : Building
		{
			if (!pawn.Spawned)
				return null;

			EdificeGrid edifice = pawn.Map.edificeGrid;
			if (edifice[pawn.Position] is T building)
				return building;
			foreach (IntVec3 pos in GenAdjFast.AdjacentCells8Way(pawn.Position))
			{
				if (edifice[pos] is T adjBuilding)
					return adjBuilding;
			}
			return null;
		}

		public static IEnumerable<T> GetAdjacentBuildings<T>(this Pawn pawn) where T : Building
		{
			var results = new List<T>();

			if (!pawn.Spawned)
				return results;

			EdificeGrid edifice = pawn.Map.edificeGrid;
			if (edifice[pawn.Position] is T building)
				results.Add(building);
			foreach (IntVec3 pos in GenAdjFast.AdjacentCells8Way(pawn.Position))
			{
				if (edifice[pos] is T adjBuilding)
					results.Add(adjBuilding);
			}
			return results;
		}

		/// <summary>
		/// If the pawn is virgin, return true.
		/// </summary>
		public static bool IsVirgin(this Pawn pawn)
		{
			return pawn.records.GetValue(VariousDefOf.VaginalSexCount) == 0;
		}

		/// <summary>
		/// If pawn is virgin, lose his/her virginity.
		/// </summary>
		public static void PoptheCherry(this Pawn pawn, Pawn partner, SexProps props)
		{
			if (props?.sexType != xxx.rjwSextype.Vaginal)
				return;

			if (pawn.IsVirgin())
			{
				pawn.TryGetComp<SexHistory.SexHistoryComp>()?.RecordFirst(partner, props);
				int? removedDegree = Virginity.TraitHandler.RemoveVirginTrait(pawn);
				if (removedDegree != null)
				{
					RJWUtility.ThrowVirginHIstoryEvent(pawn, partner, props, (int)removedDegree);
					Messages.Message(Keyed.RS_LostVirgin(pawn.LabelShort, partner.LabelShort), MessageTypeDefOf.NeutralEvent, true);
				}
			}
			else
			{
				int? removedDegree = Virginity.TraitHandler.RemoveVirginTrait(pawn);
				if (removedDegree != null)
				{
					RJWUtility.ThrowVirginHIstoryEvent(pawn, partner, props, (int)removedDegree);
				}
			}
		}
	}
}
