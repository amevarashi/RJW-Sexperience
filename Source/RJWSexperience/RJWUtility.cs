using RimWorld;
using rjw;
using rjw.Modules.Interactions.Enums;
using rjw.Modules.Interactions.Helpers;
using rjw.Modules.Interactions.Objects;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RJWSexperience
{
	public static class RJWUtility
	{
		/// <summary>
		/// For ideo patch
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "All parameters are needed for the ideology patch")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "All parameters are needed for the ideology patch")]
		public static void ThrowVirginHistoryEvent(Pawn exVirgin, Pawn partner, SexProps props, int degree)
		{
			//for non-ideo
			if (partner.Ideo == null)
			{
				partner.needs?.mood?.thoughts?.memories.TryGainMemory(xxx.took_virginity, exVirgin);
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
						IncreaseSameRecords(pawn, partner, RsDefOf.Record.VaginalSexCount);
						break;
					case xxx.rjwSextype.Anal:
						IncreaseSameRecords(pawn, partner, RsDefOf.Record.AnalSexCount);
						break;
					case xxx.rjwSextype.Oral:
					case xxx.rjwSextype.Fellatio:
						if (Genital_Helper.has_penis_fertile(giver) || Genital_Helper.has_penis_infertile(giver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.OralSexCount, RsDefOf.Record.BlowjobCount);
						}
						else if (Genital_Helper.has_penis_fertile(receiver) || Genital_Helper.has_penis_infertile(receiver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.BlowjobCount, RsDefOf.Record.OralSexCount);
						}
						break;
					case xxx.rjwSextype.Sixtynine:
						IncreaseSameRecords(pawn, partner, RsDefOf.Record.OralSexCount);
						RecordDef recordpawn, recordpartner;
						if (Genital_Helper.has_penis_fertile(pawn) || Genital_Helper.has_penis_infertile(pawn))
						{
							recordpartner = RsDefOf.Record.BlowjobCount;
						}
						else
						{
							recordpartner = RsDefOf.Record.CunnilingusCount;
						}

						if (Genital_Helper.has_penis_fertile(partner) || Genital_Helper.has_penis_infertile(partner))
						{
							recordpawn = RsDefOf.Record.BlowjobCount;
						}
						else
						{
							recordpawn = RsDefOf.Record.CunnilingusCount;
						}
						IncreaseRecords(pawn, partner, recordpawn, recordpartner);
						break;
					case xxx.rjwSextype.Cunnilingus:
						if (Genital_Helper.has_vagina(giver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.OralSexCount, RsDefOf.Record.CunnilingusCount);
						}
						else if (Genital_Helper.has_vagina(receiver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.CunnilingusCount, RsDefOf.Record.OralSexCount);
						}
						break;
					case xxx.rjwSextype.Masturbation:
						break;
					case xxx.rjwSextype.Handjob:
						if (Genital_Helper.has_penis_fertile(giver) || Genital_Helper.has_penis_infertile(giver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.GenitalCaressCount, RsDefOf.Record.HandjobCount);
						}
						else
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.HandjobCount, RsDefOf.Record.GenitalCaressCount);
						}
						break;
					case xxx.rjwSextype.Fingering:
					case xxx.rjwSextype.Fisting:
						if (Genital_Helper.has_vagina(giver))
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.GenitalCaressCount, RsDefOf.Record.FingeringCount);
						}
						else
						{
							IncreaseRecords(giver, receiver, RsDefOf.Record.FingeringCount, RsDefOf.Record.GenitalCaressCount);
						}
						break;
					case xxx.rjwSextype.Footjob:
						IncreaseSameRecords(pawn, partner, RsDefOf.Record.FootjobCount);
						break;
					default:
						IncreaseSameRecords(pawn, partner, RsDefOf.Record.MiscSexualBehaviorCount);
						break;
				}
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
			List<Building> buckets = pawn.Map.listerBuildings.allBuildingsColonist.FindAll(x => x is Building_CumBucket bucket && bucket.StoredStackCount < RsDefOf.Thing.GatheredCum.stackLimit);
			if (buckets.NullOrEmpty())
				return null;

			Dictionary<Building, float> targets = new Dictionary<Building, float>();
			for (int i = 0; i < buckets.Count; i++)
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
