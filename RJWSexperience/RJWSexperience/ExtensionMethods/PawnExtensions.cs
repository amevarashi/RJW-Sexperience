using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RJWSexperience.ExtensionMethods
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

		public static float GetCumVolume(this Pawn pawn)
		{
			List<Hediff> hediffs = Genital_Helper.get_PartsHediffList(pawn, Genital_Helper.get_genitalsBPR(pawn));
			if (hediffs.NullOrEmpty()) return 0;
			else return pawn.GetCumVolume(hediffs);
		}

		public static float GetCumVolume(this Pawn pawn, List<Hediff> hediffs)
		{
			CompHediffBodyPart part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("penis")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorf")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("ovipositorm")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();
			if (part == null) part = hediffs?.FindAll((Hediff hed) => hed.def.defName.ToLower().Contains("tentacle")).InRandomOrder().FirstOrDefault()?.TryGetComp<CompHediffBodyPart>();

			return pawn.GetCumVolume(part);
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
			if (virgin != null)
			{
				if (virgin.Degree > 0) return true;
			}
			return false;
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
					if (history != null)
					{
						history.RecordFirst(partner, props);
					}
					if (RJWUtility.RemoveVirginTrait(pawn, partner, props))
					{
						if (Configurations.EnableRecordRandomizer) Messages.Message(Keyed.RS_LostVirgin(pawn.LabelShort, partner.LabelShort), MessageTypeDefOf.NeutralEvent, true);
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

		public static Building_CumBucket FindClosestBucket(this Pawn pawn)
		{
			List<Building> buckets = pawn.Map.listerBuildings.allBuildingsColonist.FindAll(x => x is Building_CumBucket);
			Dictionary<Building, float> targets = new Dictionary<Building, float>();
			if (!buckets.NullOrEmpty()) for (int i = 0; i < buckets.Count; i++)
				{
					if (pawn.CanReach(buckets[i], PathEndMode.ClosestTouch, Danger.None))
					{
						targets.Add(buckets[i], pawn.Position.DistanceTo(buckets[i].Position));
					}
				}
			if (!targets.NullOrEmpty())
			{
				return (Building_CumBucket)targets.MinBy(x => x.Value).Key;
			}
			return null;
		}

		public static void AteCum(this Pawn pawn, float amount, bool doDrugEffect = false)
		{
			pawn.records.AddTo(VariousDefOf.NumofEatenCum, 1);
			pawn.records.AddTo(VariousDefOf.AmountofEatenCum, amount);
			if (doDrugEffect) pawn.CumDrugEffect();
		}

		public static void CumDrugEffect(this Pawn pawn)
		{
			Need need = pawn.needs?.TryGetNeed(VariousDefOf.Chemical_Cum);
			if (need != null) need.CurLevel += VariousDefOf.CumneedLevelOffset;
			Hediff addictive = HediffMaker.MakeHediff(VariousDefOf.CumTolerance, pawn);
			addictive.Severity = 0.032f;
			pawn.health.AddHediff(addictive);
			Hediff addiction = pawn.health.hediffSet.GetFirstHediffOfDef(VariousDefOf.CumAddiction);
			if (addiction != null) addiction.Severity += VariousDefOf.CumexistingAddictionSeverityOffset;

			pawn.needs?.mood?.thoughts?.memories?.TryGainMemoryFast(VariousDefOf.AteCum);
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
