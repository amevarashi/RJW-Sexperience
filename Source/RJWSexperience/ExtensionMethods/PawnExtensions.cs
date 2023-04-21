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
			if (relations == null)
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
				return pawn.GetStatValue(RsDefOf.Stat.SexAbility);
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
				if (pos.InBounds(pawn.Map) && edifice[pos] is T adjBuilding)
					results.Add(adjBuilding);
			}
			return results;
		}

		/// <summary>
		/// Check if the pawn is virgin
		/// </summary>
		public static bool IsVirgin(this Pawn pawn)
		{
			return pawn.records.GetValue(RsDefOf.Record.VaginalSexCount) == 0 ||
				pawn.relations?.ChildrenCount > 0; // Male is a virgins unless he stick into vagina? Not sure it should work this way
		}

		/// <summary>
		/// Remove virginity if pawn is virgin and announce it
		/// </summary>
		public static void TryRemoveVirginity(this Pawn pawn, Pawn partner, SexProps props)
		{
			int? removedDegree = Virginity.TraitHandler.RemoveVirginTrait(pawn);

			if (SexperienceMod.Settings.EnableSexHistory && pawn.IsVirgin())
			{
				pawn.TryGetComp<SexHistory.SexHistoryComp>()?.RecordFirst(partner);
			}

			if (removedDegree != null && removedDegree != Virginity.TraitDegree.FemaleAfterSurgery)
			{
				Messages.Message(Keyed.RS_LostVirgin(pawn.LabelShort, partner.LabelShort), MessageTypeDefOf.NeutralEvent, true);
				RJWUtility.ThrowVirginHistoryEvent(pawn, partner, props, (int)removedDegree);
			}
		}
	}
}
