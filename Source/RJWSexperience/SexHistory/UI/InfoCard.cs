using RimWorld;
using rjw;
using System;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public readonly struct InfoCard
	{
		public readonly SexPartnerHistoryRecord partnerRecord;
		public readonly string label;
		public readonly string lastSexTime;
		public readonly string name;
		public readonly string sexCount;
		public readonly string orgasms;
		public readonly string relations;
		public readonly BarInfo bestSextype;
		public readonly PartnerPortraitInfo portraitInfo;
		public readonly TipSignal tooltip;

		public InfoCard(Pawn pawn, SexPartnerHistoryRecord partnerRecord, string label, string tooltipLabel, int lastSexTimeTicks)
		{
			this.partnerRecord = partnerRecord;
			this.label = label;

			lastSexTime = UIUtility.GetSexDays(lastSexTimeTicks);
			portraitInfo = new PartnerPortraitInfo(pawn, partnerRecord);

			if (partnerRecord != null)
			{
				name = partnerRecord.Partner?.Name?.ToStringFull ?? partnerRecord.Label.CapitalizeFirst();
				sexCount = Keyed.RS_Sex_Count + partnerRecord.TotalSexCount;

				if (partnerRecord.Raped > 0)
				{
					sexCount += " " + Keyed.RS_Raped + partnerRecord.Raped;
				}
				if (partnerRecord.RapedMe > 0)
				{
					sexCount += " " + Keyed.RS_RapedMe + partnerRecord.RapedMe;
				}

				orgasms = Keyed.RS_Orgasms + partnerRecord.OrgasmCount;
				relations = pawn.GetRelationsString(partnerRecord.Partner);
				tooltip = new TipSignal(() =>
				{
					string completeTip = tooltipLabel;

						if (partnerRecord.Incest)
						{
							completeTip += " - " + Keyed.Incest;
						}
						if (partnerRecord.IamFirst)
						{
							completeTip += "\n" + Keyed.RS_LostVirgin(partnerRecord.Label, pawn.LabelShort);
						}
						if (partnerRecord.BestSexTickAbs != 0)
						{
							completeTip += "\n" + Keyed.RS_HadBestSexDaysAgo(partnerRecord.BestSexElapsedTicks.ToStringTicksToDays() + " " + Keyed.RS_Ago);
						}
					return completeTip;
				}, tooltipLabel.GetHashCode());

				float relativeBestSatisfaction = partnerRecord.BestSatisfaction / UIUtility.BASESAT;
				bestSextype = new BarInfo(
					label: Keyed.RS_Best_Sextype + ": " + Keyed.Sextype[(int)partnerRecord.BestSextype],
					fillPercent: relativeBestSatisfaction / 2,
					fillTexture: HistoryUtility.SextypeColor[(int)partnerRecord.BestSextype],
					labelRight: relativeBestSatisfaction.ToStringPercent());
			}
			else
			{
				name = Keyed.Unknown;
				sexCount = Keyed.RS_Sex_Count + "?";
				orgasms = Keyed.RS_Orgasms + "?";
				relations = string.Empty;
				tooltip = default;
				bestSextype = new BarInfo(
					label: String.Format(Keyed.RS_Best_Sextype + ": {0}", Keyed.Sextype[(int)xxx.rjwSextype.None]),
					fillPercent: 0f,
					fillTexture: Texture2D.linearGrayTexture);
			}
		}
	}
}
