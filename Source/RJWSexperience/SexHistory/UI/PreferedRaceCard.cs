using System;
using UnityEngine;

namespace RJWSexperience.SexHistory.UI
{
	public class PreferedRaceCard
	{
		private readonly SexHistoryComp _sexHistory;

		public string PreferRaceLabel { get; private set; }
		public string PreferRaceTypeLabel { get; private set; }
		public string SexCount { get; private set; }
		public BarInfo BarInfo { get; } = new BarInfo(Texture2D.linearGrayTexture);
		public Func<Vector2, Texture> PortraitGetter { get; private set; }

		public PreferedRaceCard(SexHistoryComp sexHistory)
		{
			_sexHistory = sexHistory;
		}

		public void Update()
		{
			if (_sexHistory.PreferRace == null)
			{
				PreferRaceLabel = Keyed.None;
				PreferRaceTypeLabel = null;
				SexCount = null;
				BarInfo.Label = null;
				BarInfo.FillPercent = 0f;
				PortraitGetter = (_) => HistoryUtility.UnknownPawn;
				return;
			}

			PreferRaceLabel = _sexHistory.PreferRace.LabelCap;
			SexCount = Keyed.RS_Sex_Count + _sexHistory.PreferRaceSexCount;
			PortraitGetter = (size) => UIUtility.GetRaceIcon(_sexHistory.PreferRacePawn, size);

			if (_sexHistory.PreferRace != _sexHistory.ParentPawn.def)
			{
				if (_sexHistory.PreferRace.race.Animal != _sexHistory.ParentPawn.def.race.Animal)
				{
					PreferRaceTypeLabel = Keyed.RS_Bestiality;
					BarInfo.Label = Keyed.RS_SexInfo(Keyed.RS_Bestiality, _sexHistory.BestialityCount);
					BarInfo.FillPercent = _sexHistory.BestialityCount / 100f;
				}
				else
				{
					PreferRaceTypeLabel = Keyed.RS_Interspecies;
					BarInfo.Label = Keyed.RS_SexInfo(Keyed.RS_Interspecies, _sexHistory.InterspeciesCount);
					BarInfo.FillPercent = _sexHistory.InterspeciesCount / 100f;
				}
			}
			else
			{
				PreferRaceTypeLabel = null;
				BarInfo.Label = null;
				BarInfo.FillPercent = 0f;
			}
		}
	}
}
