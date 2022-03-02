using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using rjw;
using UnityEngine;

namespace RJWSexperience
{
    public static class Utility
    {
        public static System.Random random = new System.Random(Environment.TickCount);

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

        public static float RandGaussianLike(float min, float max, int iterations = 3)
        {
            double res = 0;
            for (int i = 0; i < iterations; i++)
            {
                res += random.NextDouble();
            }
            res = res / iterations;

            return (float)res * (max - min) + min;
        }

        public static void SetTo(this Pawn_RecordsTracker records, RecordDef record ,float value)
        {
            float recordval = records.GetValue(record);
            records.AddTo(record, value - recordval);
        }

        public static T GetAdjacentBuilding<T>(this Pawn pawn) where T : Building 
        {
            if (pawn.Spawned)
            {
                EdificeGrid edifice = pawn.Map.edificeGrid;
                if (edifice[pawn.Position] is T) return (T)edifice[pawn.Position];
                IEnumerable<IntVec3> adjcells = GenAdjFast.AdjacentCells8Way(pawn.Position);
                foreach(IntVec3 pos in adjcells)
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

        public static float Normalization(this float num, float min, float max)
        {
            return (num - min)/(max - min);
        }

        public static float Denormalization(this float num, float min, float max)
        {
            return num * (max - min) + min;
        }

        public static void ResetRecord(this Pawn pawn, bool allzero)
        {
            if (!allzero)
            {
                if (Configurations.EnableRecordRandomizer && pawn != null && xxx.is_human(pawn))
                {
                    RecordRandomizer.Randomize(pawn);
                }
                pawn.AddVirginTrait();
            }
            else
            {
                pawn.records.SetTo(VariousDefOf.Lust, 0);
                pawn.records.SetTo(VariousDefOf.NumofEatenCum, 0);
                pawn.records.SetTo(VariousDefOf.AmountofEatenCum, 0);
                pawn.records.SetTo(VariousDefOf.VaginalSexCount, 0);
                pawn.records.SetTo(VariousDefOf.AnalSexCount, 0);
                pawn.records.SetTo(VariousDefOf.OralSexCount, 0);
                pawn.records.SetTo(VariousDefOf.BlowjobCount, 0);
                pawn.records.SetTo(VariousDefOf.CunnilingusCount, 0);
                pawn.records.SetTo(VariousDefOf.GenitalCaressCount, 0);
                pawn.records.SetTo(VariousDefOf.HandjobCount, 0);
                pawn.records.SetTo(VariousDefOf.FingeringCount, 0);
                pawn.records.SetTo(VariousDefOf.FootjobCount, 0);
                pawn.records.SetTo(VariousDefOf.MiscSexualBehaviorCount, 0);
                pawn.records.SetTo(VariousDefOf.SexPartnerCount, 0);
                pawn.records.SetTo(VariousDefOf.OrgasmCount, 0);
                pawn.records.SetTo(xxx.CountOfBeenRapedByAnimals, 0);
                pawn.records.SetTo(xxx.CountOfBeenRapedByHumanlikes, 0);
                pawn.records.SetTo(xxx.CountOfBeenRapedByInsects, 0);
                pawn.records.SetTo(xxx.CountOfBeenRapedByOthers, 0);
                pawn.records.SetTo(xxx.CountOfBirthAnimal, 0);
                pawn.records.SetTo(xxx.CountOfBirthEgg, 0);
                pawn.records.SetTo(xxx.CountOfBirthHuman, 0);
                pawn.records.SetTo(xxx.CountOfFappin, 0);
                pawn.records.SetTo(xxx.CountOfRapedAnimals, 0);
                pawn.records.SetTo(xxx.CountOfRapedHumanlikes, 0);
                pawn.records.SetTo(xxx.CountOfRapedInsects, 0);
                pawn.records.SetTo(xxx.CountOfRapedOthers, 0);
                pawn.records.SetTo(xxx.CountOfSex, 0);
                pawn.records.SetTo(xxx.CountOfSexWithAnimals, 0);
                pawn.records.SetTo(xxx.CountOfSexWithCorpse, 0);
                pawn.records.SetTo(xxx.CountOfSexWithHumanlikes, 0);
                pawn.records.SetTo(xxx.CountOfSexWithInsects, 0);
                pawn.records.SetTo(xxx.CountOfSexWithOthers, 0);
                pawn.records.SetTo(xxx.CountOfWhore, 0);
            }
        }

    }
}
