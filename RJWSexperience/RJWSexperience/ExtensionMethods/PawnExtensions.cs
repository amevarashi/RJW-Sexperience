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
			if (otherpawn != null)
			{
				IEnumerable<PawnRelationDef> relations = pawn.GetRelations(otherpawn);
				if (!relations.EnumerableNullOrEmpty()) foreach (PawnRelationDef relation in relations)
					{
						if (relation.incestOpinionOffset < 0) return true;
					}
			}
			return false;
		}

		public static float GetSexStat(this Pawn pawn)
		{
			if (xxx.is_human(pawn) && !pawn.Dead)
			{
				return pawn.GetStatValue(xxx.sex_stat);
			}
			else return 1.0f;
		}

		public static T GetAdjacentBuilding<T>(this Pawn pawn) where T : Building
		{
			if (pawn.Spawned)
			{
				EdificeGrid edifice = pawn.Map.edificeGrid;
				if (edifice[pawn.Position] is T) return (T)edifice[pawn.Position];
				IEnumerable<IntVec3> adjcells = GenAdjFast.AdjacentCells8Way(pawn.Position);
				foreach (IntVec3 pos in adjcells)
				{
					if (edifice[pos] is T) return (T)edifice[pos];
				}
			}
			return null;
		}

		public static IEnumerable<T> GetAdjacentBuildings<T>(this Pawn pawn) where T : Building
		{
			// This Method was introduced to fill multiple CumBuckets around a single pawn.
			var results = new List<T>();
			if (pawn.Spawned)
			{
				EdificeGrid edifice = pawn.Map.edificeGrid;
				if (edifice[pawn.Position] is T)
					results.Add((T)edifice[pawn.Position]);
				IEnumerable<IntVec3> adjcells = GenAdjFast.AdjacentCells8Way(pawn.Position);
				foreach (IntVec3 pos in adjcells)
				{
					if (edifice[pos] is T)
						results.Add((T)edifice[pos]);
				}
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
		public static bool HasHymen(this Pawn pawn)
		{
			Trait virgin = pawn.story?.traits?.GetTrait(VariousDefOf.Virgin);
			return virgin?.Degree > 0;
		}

		/// <summary>
		/// If pawn is virgin, lose his/her virginity.
		/// </summary>
		public static void PoptheCherry(this Pawn pawn, Pawn partner, SexProps props)
		{
			if (props != null && props.sexType == xxx.rjwSextype.Vaginal)
			{
				if (pawn.IsVirgin())
				{
					SexPartnerHistory history = pawn.TryGetComp<SexPartnerHistory>();
					history?.RecordFirst(partner, props);
					if (RJWUtility.RemoveVirginTrait(pawn, partner, props))
					{
						Messages.Message(Keyed.RS_LostVirgin(pawn.LabelShort, partner.LabelShort), MessageTypeDefOf.NeutralEvent, true);
					}
				}
				else
				{
					RJWUtility.RemoveVirginTrait(pawn, partner, props);
				}
			}
		}

		public static Gender PreferGender(this Pawn pawn)
		{
			if (pawn.gender == Gender.Male)
			{
				if (xxx.is_homosexual(pawn)) return Gender.Male;
				else return Gender.Female;
			}
			else
			{
				if (xxx.is_homosexual(pawn)) return Gender.Female;
				else return Gender.Male;
			}
		}

		public static void AddVirginTrait(this Pawn pawn)
		{
			if (pawn.story?.traits != null)
			{
				if (pawn.IsVirgin())
				{
					int degree = 0;
					if (pawn.gender == Gender.Female) degree = 2;
					Trait virgin = new Trait(VariousDefOf.Virgin, degree, true);
					pawn.story.traits.GainTrait(virgin);
				}
				else if (pawn.gender == Gender.Female && Rand.Chance(0.05f))
				{
					Trait virgin = new Trait(VariousDefOf.Virgin, 1, true);
					pawn.story.traits.GainTrait(virgin);
				}
			}
		}
	}
}
