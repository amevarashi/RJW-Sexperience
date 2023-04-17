using RimWorld;
using rjw;
using System;
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
		public BarInfo TotalSex { get; private set; }
		public BarInfo Lust { get; private set; }
		public BarInfo BestSextype { get; private set; }
		public BarInfo RecentSextype { get; private set; }
		public BarInfo Necro { get; private set; }
		public BarInfo Incest { get; private set; }
		public BarInfo ConsumedCum { get; private set; }
		public BarInfo? CumHediff { get; private set; }
		public BarInfo BeenRaped { get; private set; }
		public BarInfo Raped { get; private set; }
		public BarInfo SexSatisfaction { get; private set; }
		public BarInfo SexSkill { get; private set; }
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
				partnerRecord: _history.GetRecentPartnersHistory,
				label: Keyed.RS_Recent_Sex_Partner,
				tooltipLabel: Keyed.RS_Recent_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.RecentSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.GetFirstPartnerHistory,
				label: Keyed.RS_First_Sex_Partner,
				tooltipLabel: Keyed.RS_First_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.FirstSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.GetMostPartnerHistory,
				label: Keyed.RS_Most_Sex_Partner,
				tooltipLabel: Keyed.RS_Most_Sex_Partner_ToolTip,
				lastSexTimeTicks: _history.MostSexTickAbs));

			InfoCards.Add(new InfoCard(
				pawn: Pawn,
				partnerRecord: _history.GetBestSexPartnerHistory,
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
				label: String.Format(Keyed.RS_Sex_Partners + ": {0} ({1})", _history.PartnerCount, Pawn.records.GetValue(RsDefOf.Record.SexPartnerCount)),
				fillPercent: _history.PartnerCount / 50,
				fillTexture: HistoryUtility.Partners));

			SexTypes.Add(new BarInfo(
				label: String.Format(Keyed.RS_VirginsTaken + ": {0:0}", _history.VirginsTaken),
				fillPercent: _history.VirginsTaken / 100,
				fillTexture: HistoryUtility.Partners));

			TotalSex = new BarInfo(
				label: String.Format(Keyed.RS_TotalSexHad + ": {0:0} ({1:0})", _history.TotalSexHad, Pawn.records.GetValue(xxx.CountOfSex)),
				fillPercent: _history.TotalSexHad / 100,
				fillTexture: HistoryUtility.TotalSex,
				labelRight: Keyed.RS_SatAVG(_history.AVGSat));

			float lust = Pawn.records.GetValue(RsDefOf.Record.Lust);
			float sexDrive = GetStatValue(xxx.sex_drive_stat);
			float lustLimit = SexperienceMod.Settings.LustLimit * 3f;
			Lust = new BarInfo(
				label: String.Format(Keyed.Lust + ": {0:0.00}", lust),
				fillPercent: Mathf.Clamp01(lust.Normalization(-lustLimit, lustLimit)),
				fillTexture: HistoryUtility.Slaanesh,
				tooltip: GetStatTooltip(xxx.sex_drive_stat, sexDrive),
				labelRight: xxx.sex_drive_stat.LabelCap + ": " + sexDrive.ToStringPercent());

			float bestSextypeRelativeSatisfaction = _history.GetBestSextype(out xxx.rjwSextype bestSextype) / UIUtility.BASESAT;
			BestSextype = new BarInfo(
				label: String.Format(Keyed.RS_Best_Sextype + ": {0}", Keyed.Sextype[(int)bestSextype]),
				fillPercent: bestSextypeRelativeSatisfaction / 2,
				fillTexture: HistoryUtility.SextypeColor[(int)bestSextype],
				labelRight: Keyed.RS_SatAVG(bestSextypeRelativeSatisfaction));

			float recentSextypeRelativeSatisfaction = _history.GetRecentSextype(out xxx.rjwSextype recentSextype) / UIUtility.BASESAT;
			RecentSextype = new BarInfo(
				label: String.Format(Keyed.RS_Recent_Sextype + ": {0}", Keyed.Sextype[(int)recentSextype]),
				fillPercent: recentSextypeRelativeSatisfaction / 2,
				fillTexture: HistoryUtility.SextypeColor[(int)recentSextype],
				labelRight: recentSextypeRelativeSatisfaction.ToStringPercent());

			Necro = new BarInfo(
				label: String.Format(Keyed.RS_Necrophile + ": {0}", _history.CorpseFuckCount),
				fillPercent: _history.CorpseFuckCount / 50,
				fillTexture: HistoryUtility.Nurgle);

			Incest = new BarInfo(
				label: String.Format(Keyed.Incest + ": {0}", _history.IncestuousCount),
				fillPercent: _history.IncestuousCount / 50,
				fillTexture: HistoryUtility.Nurgle);

			float amountofEatenCum = Pawn.records.GetValue(RsDefOf.Record.AmountofEatenCum);
			ConsumedCum = new BarInfo(
				label: String.Format(Keyed.RS_Cum_Swallowed + ": {0} mL, {1} " + Keyed.RS_NumofTimes, amountofEatenCum, Pawn.records.GetValue(RsDefOf.Record.NumofEatenCum)),
				fillPercent: amountofEatenCum / 1000,
				fillTexture: Texture2D.linearGrayTexture);

			Hediff cumHediff = Pawn.health.hediffSet.GetFirstHediffOfDef(RsDefOf.Hediff.CumAddiction)
				?? Pawn.health.hediffSet.GetFirstHediffOfDef(RsDefOf.Hediff.CumTolerance);
			if (cumHediff != null)
			{
				CumHediff = new BarInfo(
					label: $"{cumHediff.Label}: {cumHediff.Severity.ToStringPercent()}",
					fillPercent: cumHediff.Severity,
					fillTexture: Texture2D.linearGrayTexture,
					tooltip: new TipSignal(() => cumHediff.GetTooltip(Pawn, false), cumHediff.Label.GetHashCode()));
			}

			float vulnerability = GetStatValue(xxx.vulnerability_stat);
			string vulnerabilityLabel = xxx.vulnerability_stat.LabelCap + ": " + vulnerability.ToStringPercent();
			TipSignal vulnerabilityTip = GetStatTooltip(xxx.vulnerability_stat, vulnerability);

			Raped = new BarInfo(
				label: String.Format(Keyed.RS_RapedSomeone + ": {0}", _history.RapedCount),
				fillPercent: _history.RapedCount / 50,
				fillTexture: HistoryUtility.Khorne,
				tooltip: vulnerabilityTip,
				labelRight: vulnerabilityLabel);

			BeenRaped = new BarInfo(
				label: String.Format(Keyed.RS_BeenRaped + ": {0}", _history.BeenRapedCount),
				fillPercent: _history.BeenRapedCount / 50,
				fillTexture: Texture2D.grayTexture,
				tooltip: vulnerabilityTip,
				labelRight: vulnerabilityLabel);

			float sexSatisfaction = GetStatValue(xxx.sex_satisfaction);
			SexSatisfaction = new BarInfo(
				label: xxx.sex_satisfaction.LabelCap + ": " + sexSatisfaction.ToStringPercent(),
				fillPercent: sexSatisfaction / 2,
				fillTexture: HistoryUtility.Satisfaction,
				tooltip: GetStatTooltip(xxx.sex_satisfaction, sexSatisfaction));

			SkillRecord skill = Pawn.skills?.GetSkill(RsDefOf.Skill.Sex);
			float sexSkillLevel = skill?.Level ?? 0f;
			float sexStat = Pawn.GetSexStat();
			SexSkill = new BarInfo(
				label: $"{Keyed.RS_SexSkill}: {sexSkillLevel}, {skill?.xpSinceLastLevel / skill?.XpRequiredForLevelUp:P2}",
				fillPercent: sexSkillLevel / 20,
				fillTexture: HistoryUtility.Tzeentch,
				tooltip: GetStatTooltip(RsDefOf.Stat.SexAbility, sexStat),
				labelRight: RsDefOf.Stat.SexAbility.LabelCap + ": " + sexStat.ToStringPercent(),
				border: HistoryUtility.GetPassionBG(skill?.passion));
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
