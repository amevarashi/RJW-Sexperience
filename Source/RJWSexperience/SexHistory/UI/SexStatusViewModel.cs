using RimWorld;
using rjw;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public sealed class SexStatusViewModel
	{
		private static readonly int[] Sextype =
		{
			(int)xxx.rjwSextype.Vaginal,
			(int)xxx.rjwSextype.Anal,
			(int)xxx.rjwSextype.Oral,
			(int)xxx.rjwSextype.Fellatio,
			(int)xxx.rjwSextype.Cunnilingus,
			(int)xxx.rjwSextype.DoublePenetration,
			(int)xxx.rjwSextype.Boobjob,
			(int)xxx.rjwSextype.Handjob,
			(int)xxx.rjwSextype.Footjob,
			(int)xxx.rjwSextype.Fingering,
			(int)xxx.rjwSextype.Scissoring,
			(int)xxx.rjwSextype.MutualMasturbation,
			(int)xxx.rjwSextype.Fisting,
			(int)xxx.rjwSextype.Rimming,
			(int)xxx.rjwSextype.Sixtynine
		};

		private readonly SexHistoryComp _history;
		private readonly CompRJW _rjwComp;
		private int _validUntilTick;

		public SexStatusViewModel(SexHistoryComp history, PartnerOrderMode mode)
		{
			_history = history;
			_rjwComp = history.ParentPawn.TryGetComp<CompRJW>();
			SetPartnerOrder(mode);

			Name = Pawn.Name?.ToStringFull ?? Pawn.Label;
			if (Pawn.story != null)
			{
				AgeAndTitle = Pawn.ageTracker.AgeBiologicalYears + ", " + Pawn.story.Title;
			}
			else
			{
				AgeAndTitle = Pawn.ageTracker.AgeBiologicalYears + ", " + Pawn.def.label;
			}
		}

		public Pawn Pawn => _history.ParentPawn;
		public string Name { get; }
		public string AgeAndTitle { get; }
		public List<InfoCard> InfoCards { get; } = new List<InfoCard>();
		public InfoCard SelectedPartnerCard { get; private set; }
		public PreferedRaceCard PreferedRaceCard { get; private set; }
		public List<BarInfo> SexTypes { get; } = new List<BarInfo>();
		public BarInfo TotalSex { get; } = new BarInfo(HistoryUtility.TotalSex);
		public BarInfo Lust { get; } = new BarInfo(HistoryUtility.Slaanesh);
		public BarInfo BestSextype { get; } = new BarInfo();
		public BarInfo RecentSextype { get; } = new BarInfo();
		public BarInfo Necro { get; } = new BarInfo(HistoryUtility.Nurgle);
		public BarInfo Incest { get; } = new BarInfo(HistoryUtility.Nurgle);
		public BarInfo ConsumedCum { get; } = new BarInfo(Texture2D.linearGrayTexture);
		public BarInfo CumHediff { get; } = new BarInfo(Texture2D.linearGrayTexture);
		public BarInfo BeenRaped { get; } = new BarInfo(Texture2D.grayTexture);
		public BarInfo Raped { get; } = new BarInfo(HistoryUtility.Khorne);
		public BarInfo SexSatisfaction { get; } = new BarInfo(HistoryUtility.Satisfaction);
		public BarInfo SexSkill { get; } = new BarInfo(HistoryUtility.Tzeentch);
		public string VirginLabel { get; private set; }
		public string SexualityLabel { get; private set; }
		public string QuirksLabel { get; private set; }
		public TipSignal QuirksTooltip { get; private set; }
		public SexPartnerHistoryRecord SelectedPartner { get; private set; }
		public IEnumerable<PartnerPortraitInfo> Partners { get; private set; }

		public void Update()
		{
			if (Find.TickManager.TicksGame <= _validUntilTick)
			{
				return;
			}

			UpdateInfoCards();
			UpdateBars();
			UpdateQuirks();
			UpdateVirginAndSexuality();
			PreferedRaceCard = new PreferedRaceCard(_history);

			int tickRateMultiplier = (int)Find.TickManager.TickRateMultiplier;
			if (tickRateMultiplier == 0) // Paused
			{
				_validUntilTick = Find.TickManager.TicksGame;
				return;
			}

			_validUntilTick = Find.TickManager.TicksGame + (60 * tickRateMultiplier);
		}

		private void UpdateInfoCards()
		{
			InfoCards.Clear();

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.RecentPartnerRecord,
				label: Keyed.RS_Recent_Sex_Partner,
				tooltipLabel: Keyed.RS_Recent_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.RecentSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.FirstPartnerRecord,
				label: Keyed.RS_First_Sex_Partner,
				tooltipLabel: Keyed.RS_First_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.FirstSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.MostPartnerRecord,
				label: Keyed.RS_Most_Sex_Partner,
				tooltipLabel: Keyed.RS_Most_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.MostSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.BestSexPartnerRecord,
				label: Keyed.RS_Best_Sex_Partner,
				tooltipLabel: Keyed.RS_Best_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.BestSexTickAbs));

			if (SelectedPartner != null)
			{
				UpdateSelectedPartnerCard();
			}
		}

		private void UpdateBars()
		{
			float maxSatisfaction = _history.GetBestSextype(out _);
			if (maxSatisfaction == 0f)
			{
				maxSatisfaction = UIUtility.BASESAT;
			}

			SexTypes.Clear();

			for (int i = 0; i < Sextype.Length; i++)
			{
				int sexIndex = Sextype[i];
				float AverageSatisfaction = _history.GetAVGSat(sexIndex);
				float relativeSat = AverageSatisfaction / maxSatisfaction;
				float satisfactionRelativeToBase = AverageSatisfaction / UIUtility.BASESAT;
				SexTypes.Add(new BarInfo(
					label: Keyed.RS_SexInfo(Keyed.Sextype[sexIndex], _history.GetSexCount(sexIndex)),
					fillPercent: relativeSat,
					fillTexture: HistoryUtility.SextypeColor[sexIndex],
					tooltip: Keyed.RS_LastSex + ": " + UIUtility.GetSexDays(_history.GetSextypeRecentTickAbs(sexIndex), true),
					labelRight: Keyed.RS_SatAVG(satisfactionRelativeToBase)));
			}

			SexTypes.Add(new BarInfo(
				label: string.Format(Keyed.RS_Sex_Partners + ": {0} ({1})", _history.PartnerCount, Pawn.records.GetValue(RsDefOf.Record.SexPartnerCount)),
				fillPercent: _history.PartnerCount / 50,
				fillTexture: HistoryUtility.Partners));

			SexTypes.Add(new BarInfo(
				label: string.Format(Keyed.RS_VirginsTaken + ": {0:0}", _history.VirginsTaken),
				fillPercent: _history.VirginsTaken / 100,
				fillTexture: HistoryUtility.Partners));

			TotalSex.Label = string.Format(Keyed.RS_TotalSexHad + ": {0:0} ({1:0})", _history.TotalSexHad, Pawn.records.GetValue(xxx.CountOfSex));
			TotalSex.FillPercent = _history.TotalSexHad / 100;
			TotalSex.LabelRight = Keyed.RS_SatAVG(_history.AVGSat);

			float lust = Pawn.records.GetValue(RsDefOf.Record.Lust);
			float sexDrive = GetStatValue(xxx.sex_drive_stat);
			float lustLimit = SexperienceMod.Settings.LustLimit * 3f;
			Lust.Label = string.Format(Keyed.Lust + ": {0:0.00}", lust);
			Lust.FillPercent = lust.Normalization(-lustLimit, lustLimit);
			Lust.Tooltip = GetStatTooltip(xxx.sex_drive_stat, sexDrive);
			Lust.LabelRight = xxx.sex_drive_stat.LabelCap + ": " + sexDrive.ToStringPercent();

			float bestSextypeRelativeSatisfaction = _history.GetBestSextype(out xxx.rjwSextype bestSextype) / UIUtility.BASESAT;
			BestSextype.Label = string.Format(Keyed.RS_Best_Sextype + ": {0}", Keyed.Sextype[(int)bestSextype]);
			BestSextype.FillPercent = bestSextypeRelativeSatisfaction / 2;
			BestSextype.FillTexture = HistoryUtility.SextypeColor[(int)bestSextype];
			BestSextype.LabelRight = Keyed.RS_SatAVG(bestSextypeRelativeSatisfaction);

			float recentSextypeRelativeSatisfaction = _history.GetRecentSextype(out xxx.rjwSextype recentSextype) / UIUtility.BASESAT;
			RecentSextype.Label = string.Format(Keyed.RS_Recent_Sextype + ": {0}", Keyed.Sextype[(int)recentSextype]);
			RecentSextype.FillPercent = recentSextypeRelativeSatisfaction / 2;
			RecentSextype.FillTexture = HistoryUtility.SextypeColor[(int)recentSextype];
			RecentSextype.LabelRight = recentSextypeRelativeSatisfaction.ToStringPercent();

			Necro.Label = string.Format(Keyed.RS_Necrophile + ": {0}", _history.CorpseFuckCount);
			Necro.FillPercent = _history.CorpseFuckCount / 50f;

			Incest.Label = string.Format(Keyed.Incest + ": {0}", _history.IncestuousCount);
			Incest.FillPercent = _history.IncestuousCount / 50f;

			float amountofEatenCum = Pawn.records.GetValue(RsDefOf.Record.AmountofEatenCum);
			ConsumedCum.Label = string.Format(Keyed.RS_Cum_Swallowed + ": {0} mL, {1} " + Keyed.RS_NumofTimes, amountofEatenCum, Pawn.records.GetValue(RsDefOf.Record.NumofEatenCum));
			ConsumedCum.FillPercent = amountofEatenCum / 1000;

			Hediff cumHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(RsDefOf.Hediff.CumAddiction)
				?? Pawn.health.hediffSet.GetFirstHediffOfDef(RsDefOf.Hediff.CumTolerance);
			if (cumHediff != null)
			{
				CumHediff.Label = $"{cumHediff.Label}: {cumHediff.Severity.ToStringPercent()}";
				CumHediff.FillPercent = cumHediff.Severity;
				CumHediff.Tooltip = new TipSignal(() => cumHediff.GetTooltip(Pawn, false), cumHediff.Label.GetHashCode());
			}
			else
			{
				CumHediff.Label = "";
				CumHediff.FillPercent = 0f;
				CumHediff.Tooltip = default;
			}

			float vulnerability = GetStatValue(xxx.vulnerability_stat);
			string vulnerabilityLabel = xxx.vulnerability_stat.LabelCap + ": " + vulnerability.ToStringPercent();
			TipSignal vulnerabilityTip = GetStatTooltip(xxx.vulnerability_stat, vulnerability);

			Raped.Label = string.Format(Keyed.RS_RapedSomeone + ": {0}", _history.RapedCount);
			Raped.FillPercent = _history.RapedCount / 50f;
			Raped.Tooltip = vulnerabilityTip;
			Raped.LabelRight = vulnerabilityLabel;

			BeenRaped.Label = string.Format(Keyed.RS_BeenRaped + ": {0}", _history.BeenRapedCount);
			BeenRaped.FillPercent = _history.BeenRapedCount / 50f;
			BeenRaped.Tooltip = vulnerabilityTip;
			BeenRaped.LabelRight = vulnerabilityLabel;

			float sexSatisfaction = GetStatValue(xxx.sex_satisfaction);
			SexSatisfaction.Label = xxx.sex_satisfaction.LabelCap + ": " + sexSatisfaction.ToStringPercent();
			SexSatisfaction.FillPercent = sexSatisfaction / 2;
			SexSatisfaction.Tooltip = GetStatTooltip(xxx.sex_satisfaction, sexSatisfaction);

			SkillRecord skill = Pawn.skills?.GetSkill(RsDefOf.Skill.Sex);
			float sexSkillLevel = skill?.Level ?? 0f;
			float sexStat = Pawn.GetSexStat();
			SexSkill.Label = $"{Keyed.RS_SexSkill}: {sexSkillLevel}, {skill?.xpSinceLastLevel / skill?.XpRequiredForLevelUp:P2}";
			SexSkill.FillPercent = sexSkillLevel / 20;
			SexSkill.Tooltip = GetStatTooltip(RsDefOf.Stat.SexAbility, sexStat);
			SexSkill.LabelRight = RsDefOf.Stat.SexAbility.LabelCap + ": " + sexStat.ToStringPercent();
			SexSkill.Border = HistoryUtility.GetPassionBG(skill?.passion);
		}

		private void UpdateQuirks()
		{
			List<Quirk> quirks = Quirk.All.FindAll(x => Pawn.Has(x));
			string quirkstr = quirks.Select(x => x.Key).ToCommaList();
			QuirksLabel = "Quirks".Translate() + quirkstr;

			if (quirks.NullOrEmpty())
			{
				QuirksTooltip = "NoQuirks".Translate();
			}
			else
			{
				QuirksTooltip = new TipSignal(() =>
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (Quirk q in quirks)
					{
						stringBuilder.AppendLine(q.Key.Colorize(Color.yellow));
						stringBuilder.AppendLine(q.LocaliztionKey.Translate(Pawn.Named("pawn")).AdjustedFor(Pawn).Resolve());
						stringBuilder.AppendLine("");
					}
					return stringBuilder.ToString().TrimEndNewlines();
				}, "Quirks".GetHashCode());
			}
		}

		private void UpdateVirginAndSexuality()
		{
			Trait virginity = Pawn.story?.traits?.GetTrait(RsDefOf.Trait.Virgin);
			if (virginity != null && virginity.Degree != Virginity.TraitDegree.FemaleAfterSurgery)
			{
				VirginLabel = virginity.Label;
			}
			else
			{
				VirginLabel = null;
			}

			if (_rjwComp != null)
			{
				SexualityLabel = Keyed.RS_Sexuality + ": " + Keyed.Sexuality[(int)_rjwComp.orientation];
			}
		}

		public void SetPartnerOrder(PartnerOrderMode mode)
		{
			if (_history == null)
			{
				return;
			}

			var partners = _history.PartnerList.Select(x => new PartnerPortraitInfo(Pawn, x));

			switch (mode)
			{
				default:
					Partners = partners;
					break;
				case PartnerOrderMode.Recent:
					Partners = partners.OrderBy(x => x.partnerRecord.RecentSexTickAbs);
					break;
				case PartnerOrderMode.Most:
					Partners = partners.OrderBy(x => x.partnerRecord.TotalSexCount);
					break;
				case PartnerOrderMode.Name:
					Partners = partners.OrderBy(x => x.partnerRecord.Label);
					break;
			}
		}

		public void SetSelectedPartner(SexPartnerHistoryRecord sexPartner)
		{
			SelectedPartner = sexPartner;
			UpdateSelectedPartnerCard();
		}

		private void UpdateSelectedPartnerCard()
		{
			if (SelectedPartner == null)
			{
				SelectedPartnerCard = default;
				return;
			}

			SelectedPartnerCard = new InfoCard(
				pawn: Pawn,
				partnerRecord: SelectedPartner,
				label: Keyed.RS_Selected_Partner,
				tooltipLabel: Keyed.RS_Selected_Partner,
				lastSexTimeTicks: SelectedPartner.RecentSexTickAbs);
		}

		private float GetStatValue(StatDef statDef) => Pawn.Dead ? 0f : Pawn.GetStatValue(statDef);

		private TipSignal GetStatTooltip(StatDef stat, float val)
		{
			if (!Pawn.Dead)
			{
				return new TipSignal(
					textGetter: () => stat.description + "\n\n" + stat.Worker.GetExplanationFull(StatRequest.For(Pawn), ToStringNumberSense.Undefined, val),
					uniqueId: stat.GetHashCode());
			}
			return "Dead".Translate();
		}
	}
}
