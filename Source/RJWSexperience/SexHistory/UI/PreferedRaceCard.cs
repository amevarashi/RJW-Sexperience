using System;
using UnityEngine;

namespace RJWSexperience.SexHistory.UI
{
	public readonly struct PreferedRaceCard
	{
		public readonly string preferRaceLabel;
		public readonly string preferRaceTypeLabel;
		public readonly string sexCount;
		public readonly BarInfo barInfo;
		public readonly Func<Vector2, Texture> portraitGetter;

		public PreferedRaceCard(SexHistoryComp sexHistory)
		{
			if (sexHistory.PreferRace == null)
			{
				preferRaceLabel = Keyed.None;
				preferRaceTypeLabel = null;
				sexCount = null;
				barInfo = null;
				portraitGetter = (_) => HistoryUtility.UnknownPawn;
				return;
			}

			preferRaceLabel = sexHistory.PreferRace.LabelCap;
			sexCount = Keyed.RS_Sex_Count + sexHistory.PreferRaceSexCount;
			portraitGetter = (size) => UIUtility.GetRaceIcon(sexHistory.PreferRacePawn, size);

			if (sexHistory.PreferRace != sexHistory.ParentPawn.def)
			{
				if (sexHistory.PreferRace.race.Animal != sexHistory.ParentPawn.def.race.Animal)
				{
					preferRaceTypeLabel = Keyed.RS_Bestiality;
					barInfo = new BarInfo(
						label: Keyed.RS_SexInfo(Keyed.RS_Bestiality, sexHistory.BestialityCount),
						fillPercent: sexHistory.BestialityCount / 100f,
						fillTexture: Texture2D.linearGrayTexture);
				}
				else
				{
					preferRaceTypeLabel = Keyed.RS_Interspecies;
					barInfo = new BarInfo(
						label: Keyed.RS_SexInfo(Keyed.RS_Interspecies, sexHistory.InterspeciesCount),
						fillPercent: sexHistory.InterspeciesCount / 100f,
						fillTexture: Texture2D.linearGrayTexture);
				}
			}
			else
			{
				preferRaceTypeLabel = null;
				barInfo = null;
			}
		}
	}
}
