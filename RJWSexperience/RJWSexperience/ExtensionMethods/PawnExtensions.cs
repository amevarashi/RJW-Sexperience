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

		public static float GetCumVolume(this Pawn pawn)
		{
			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
			if (hediffs.NullOrEmpty()) return 0;
			else return pawn.GetCumVolume(hediffs);
		}

		public static float GetCumVolume(this Pawn pawn, List<Hediff> hediffs)
		{
			float cum_value = 0;
			// Add Cum for every existing Penis at the pawn
			foreach (var penis in hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("penis")))
			{
				cum_value += pawn.GetCumVolume(penis.TryGetComp<CompHediffBodyPart>());
			}
			// Look for more exotic parts - if any is found, add some more cum for the first special part found
			CompHediffBodyPart special_part = null;
			if (special_part == null) special_part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorf")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (special_part == null) special_part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorm")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (special_part == null) special_part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("tentacle")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();

			cum_value += pawn.GetCumVolume(special_part);

			return cum_value;
		}

		public static float GetCumVolume(this Pawn pawn, CompHediffBodyPart part)
		{
			float res;

			try
			{
				res = part.FluidAmmount * part.FluidModifier * pawn.BodySize / pawn.RaceProps.baseBodySize * Rand.Range(0.8f, 1.2f) * RJWSettings.cum_on_body_amount_adjust * 0.3f;
			}
			catch (NullReferenceException)
			{
				res = 0.0f;
			}
			if (pawn.Has(Quirk.Messy)) res *= Rand.Range(4.0f, 8.0f);

			return res;
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
					SexPartnerHistory history = pawn.GetPartnerHistory();
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
