using RimWorld;
using rjw;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public class InfoCard
	{
		private readonly Pawn _pawn;
		private readonly string _tooltipLabel;

		public string Label { get; }
		public string LastSexTime { get; set; }
		public string Name { get; private set; }
		public string SexCount { get; private set; }
		public string Orgasms { get; private set; }
		public string Relations { get; private set; }
		public BarInfo BestSextype { get; }
		public SexPartnerHistoryRecord PartnerRecord { get; private set; }
		public PartnerPortraitInfo PortraitInfo { get; }
		public TipSignal Tooltip { get; private set; }

		public InfoCard(Pawn pawn, string label, string tooltipLabel)
		{
			Label = label;
			_pawn = pawn;
			_tooltipLabel = tooltipLabel;
			BestSextype = new BarInfo();
			PortraitInfo = new PartnerPortraitInfo(_pawn);
		}

		public void UpdatePartnerRecord(SexPartnerHistoryRecord partnerRecord)
		{
			PartnerRecord = partnerRecord;
			PortraitInfo.UpdatePartnerRecord(partnerRecord);

			if (partnerRecord == null)
			{
				Name = Keyed.Unknown;
				SexCount = Keyed.RS_Sex_Count + "?";
				Orgasms = Keyed.RS_Orgasms + "?";
				Relations = string.Empty;
				Tooltip = default;

				BestSextype.Label = Keyed.RS_Best_Sextype + ": " + Keyed.Sextype[(int)xxx.rjwSextype.None];
				BestSextype.FillPercent = 0f;
				BestSextype.FillTexture = Texture2D.linearGrayTexture;
				BestSextype.LabelRight = "";
				return;
			}

			Name = partnerRecord.Partner?.Name?.ToStringFull ?? partnerRecord.Label.CapitalizeFirst();
			SexCount = Keyed.RS_Sex_Count + partnerRecord.TotalSexCount;

			if (partnerRecord.Raped > 0)
			{
				SexCount += " " + Keyed.RS_Raped + partnerRecord.Raped;
			}
			if (partnerRecord.RapedMe > 0)
			{
				SexCount += " " + Keyed.RS_RapedMe + partnerRecord.RapedMe;
			}

			Orgasms = Keyed.RS_Orgasms + partnerRecord.OrgasmCount;
			Relations = _pawn.GetRelationsString(partnerRecord.Partner);
			Tooltip = new TipSignal(() =>
			{
				string completeTip = _tooltipLabel;

				if (partnerRecord.Incest)
				{
					completeTip += " - " + Keyed.Incest;
				}
				if (partnerRecord.IamFirst)
				{
					completeTip += "\n" + Keyed.RS_LostVirgin(partnerRecord.Label, _pawn.LabelShort);
				}
				if (partnerRecord.BestSexTickAbs != 0)
				{
					completeTip += "\n" + Keyed.RS_HadBestSexDaysAgo(partnerRecord.BestSexElapsedTicks.ToStringTicksToDays() + " " + Keyed.RS_Ago);
				}
				return completeTip;
			}, _tooltipLabel.GetHashCode());

			float relativeBestSatisfaction = partnerRecord.BestSatisfaction / UIUtility.BASESAT;
			BestSextype.Label = Keyed.RS_Best_Sextype + ": " + Keyed.Sextype[(int)partnerRecord.BestSextype];
			BestSextype.FillPercent = relativeBestSatisfaction / 2;
			BestSextype.FillTexture = HistoryUtility.SextypeColor[(int)partnerRecord.BestSextype];
			BestSextype.LabelRight = relativeBestSatisfaction.ToStringPercent();
		}
	}
}
