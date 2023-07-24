using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public class PartnerPortraitInfo
	{
		private readonly Pawn _pawn;

		public SexPartnerHistoryRecord PartnerRecord { get; private set; }
		public bool Lover { get; private set; }
		public Func<Vector2, Texture> PortraitGetter { get; private set; }

		public PartnerPortraitInfo(Pawn pawn)
		{
			_pawn = pawn;
		}

		public PartnerPortraitInfo(Pawn pawn, SexPartnerHistoryRecord partnerRecord)
		{
			_pawn = pawn;
			UpdatePartnerRecord(partnerRecord);
		}

		public void UpdatePartnerRecord(SexPartnerHistoryRecord partnerRecord)
		{
			PartnerRecord = partnerRecord;

			if (partnerRecord?.Partner != null)
			{
				PortraitGetter = (size) => PortraitsCache.Get(partnerRecord.Partner, size, Rot4.South, default, 1, true, true, false, false);
				Lover = LovePartnerRelationUtility.LovePartnerRelationExists(_pawn, partnerRecord.Partner);
			}
			else if (partnerRecord?.Race?.uiIcon != null)
			{
				PortraitGetter = (_) => partnerRecord.Race.uiIcon;
				Lover = false;
			}
			else
			{
				PortraitGetter = (_) => HistoryUtility.UnknownPawn;
				Lover = false;
			}
		}
	}
}
