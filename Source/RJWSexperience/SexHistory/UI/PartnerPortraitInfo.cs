using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public readonly struct PartnerPortraitInfo
	{
		public readonly SexPartnerHistoryRecord partnerRecord;
		public readonly bool lover;
		public readonly Func<Vector2, Texture> portraitGetter;

		public PartnerPortraitInfo(Pawn pawn, SexPartnerHistoryRecord partnerRecord)
		{
			this.partnerRecord = partnerRecord;
			lover = false;

			if (partnerRecord?.Partner != null)
			{
				portraitGetter = (size) => PortraitsCache.Get(partnerRecord.Partner, size, Rot4.South, default, 1, true, true, false, false);
				lover = LovePartnerRelationUtility.LovePartnerRelationExists(pawn, partnerRecord.Partner);
			}
			else if (partnerRecord?.Race?.uiIcon != null)
			{
				portraitGetter = (_) => partnerRecord.Race.uiIcon;
			}
			else
			{
				portraitGetter = (_) => HistoryUtility.UnknownPawn;
			}
		}
	}
}
