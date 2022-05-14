using System.Collections.Generic;
using rjw;
using RimWorld;
using Verse;
using Verse.AI;
using rjw.Modules.Interactions.Objects;
using rjw.Modules.Interactions.Helpers;
using rjw.Modules.Interactions.Enums;

namespace RJWSexperience
{
	public static class RJWUtility
    {
        public static bool RemoveVirginTrait(Pawn pawn, Pawn partner, SexProps props)
        {
            int degree;
            Trait virgin = pawn.story?.traits?.GetTrait(VariousDefOf.Virgin);
            if (virgin != null)
            {
                degree = virgin.Degree;
                if (pawn.gender == Gender.Female && degree > 0)
                {
                    FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_Blood, pawn.LabelShort, 1, FilthSourceFlags.Pawn);
                }
                ThrowVirginHIstoryEvent(pawn, partner, props, degree);
                pawn.story.traits.RemoveTrait(virgin);
                return true;
            }
            return false;
        }

        /// <summary>
        /// For ideo patch
        /// </summary>
        public static void ThrowVirginHIstoryEvent(Pawn pawn, Pawn partner, SexProps props, int degree)
        {
            //for non-ideo
            if (partner.Ideo == null)
            {
                partner.needs?.mood?.thoughts?.memories.TryGainMemory(xxx.took_virginity, pawn);
            }
        }

        /*
         * Uses RJW 4.9.0's new interactiondefs to determine giver and receiver based on reverse interactiontag
         */

        public static void DetermineGiversAndReceivers(SexProps props, out Pawn giver, out Pawn receiver)
        {
            InteractionWithExtension interaction = InteractionHelper.GetWithExtension(props.dictionaryKey);
            if (interaction.HasInteractionTag(InteractionTag.Reverse))
            {
                receiver = props.partner;
                giver = props.pawn;
            }
            else
            {
                receiver = props.pawn;
                giver = props.partner;
            }
        }

        public static void UpdateSextypeRecords(SexProps props)
        {
            xxx.rjwSextype sextype = props.sexType;
            Pawn pawn = props.pawn;
            Pawn partner = props.partner;

            DetermineGiversAndReceivers(props, out Pawn giver, out Pawn receiver);

            if (partner != null)
            {
                switch (sextype)
                {
                    case xxx.rjwSextype.Vaginal:
                    case xxx.rjwSextype.Scissoring:
                        IncreaseSameRecords(pawn, partner, VariousDefOf.VaginalSexCount);
                        break;
                    case xxx.rjwSextype.Anal:
                        IncreaseSameRecords(pawn, partner, VariousDefOf.AnalSexCount);
                        break;
                    case xxx.rjwSextype.Oral:
                    case xxx.rjwSextype.Fellatio:
                        if (Genital_Helper.has_penis_fertile(giver) || Genital_Helper.has_penis_infertile(giver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.OralSexCount, VariousDefOf.BlowjobCount);
                        }
                        else if (Genital_Helper.has_penis_fertile(receiver) || Genital_Helper.has_penis_infertile(receiver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.BlowjobCount, VariousDefOf.OralSexCount);
                        }
                        break;
                    case xxx.rjwSextype.Sixtynine:
                        IncreaseSameRecords(pawn, partner, VariousDefOf.OralSexCount);
                        RecordDef recordpawn, recordpartner;
                        if (Genital_Helper.has_penis_fertile(pawn) || Genital_Helper.has_penis_infertile(pawn))
                        {
                            recordpartner = VariousDefOf.BlowjobCount;
                        }
                        else
                        {
                            recordpartner = VariousDefOf.CunnilingusCount;
                        }

                        if (Genital_Helper.has_penis_fertile(partner) || Genital_Helper.has_penis_infertile(partner))
                        {
                            recordpawn = VariousDefOf.BlowjobCount;
                        }
                        else
                        {
                            recordpawn = VariousDefOf.CunnilingusCount;
                        }
                        IncreaseRecords(pawn, partner, recordpawn, recordpartner);
                        break;
                    case xxx.rjwSextype.Cunnilingus:
                        if (Genital_Helper.has_vagina(giver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.OralSexCount, VariousDefOf.CunnilingusCount);
                        }
                        else if (Genital_Helper.has_vagina(receiver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.CunnilingusCount, VariousDefOf.OralSexCount);
                        }
                        break;
                    case xxx.rjwSextype.Masturbation:
                        break;
                    case xxx.rjwSextype.Handjob:
                        if (Genital_Helper.has_penis_fertile(giver) || Genital_Helper.has_penis_infertile(giver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.GenitalCaressCount, VariousDefOf.HandjobCount);
                        }
                        else
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.HandjobCount, VariousDefOf.GenitalCaressCount);
                        }
                        break;
                    case xxx.rjwSextype.Fingering:
                    case xxx.rjwSextype.Fisting:
                        if (Genital_Helper.has_vagina(giver))
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.GenitalCaressCount, VariousDefOf.FingeringCount);
                        }
                        else
                        {
                            IncreaseRecords(giver, receiver, VariousDefOf.FingeringCount, VariousDefOf.GenitalCaressCount);
                        }
                        break;
                    case xxx.rjwSextype.Footjob:
                        IncreaseSameRecords(pawn, partner, VariousDefOf.FootjobCount);
                        break;
                    default:
                        IncreaseSameRecords(pawn, partner, VariousDefOf.MiscSexualBehaviorCount);
                        break;
                }
            }
        }

        public static void UpdatePartnerHistory(Pawn pawn, Pawn partner, SexProps props)
        {
            if (partner != null)
            {
                SexPartnerHistory pawnshistory = pawn.TryGetComp<SexPartnerHistory>();
                pawnshistory?.RecordSex(partner, props);
            }
        }

        public static void UpdateSatisfactionHIstory(Pawn pawn, Pawn partner, SexProps props, float satisfaction)
        {
            if (partner != null)
            {
                SexPartnerHistory pawnshistory = pawn.TryGetComp<SexPartnerHistory>();
                pawnshistory?.RecordSatisfaction(partner, props, satisfaction);
            }
        }

        public static void IncreaseSameRecords(Pawn pawn, Pawn partner, RecordDef record)
        {
            pawn.records?.AddTo(record, 1);
            partner.records?.AddTo(record, 1);
        }

        public static void IncreaseRecords(Pawn pawn, Pawn partner, RecordDef recordforpawn, RecordDef recordforpartner)
        {
            pawn.records?.AddTo(recordforpawn, 1);
            partner.records?.AddTo(recordforpartner, 1);
        }

        // Moved this method back because of Menstruation
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

    }
}
