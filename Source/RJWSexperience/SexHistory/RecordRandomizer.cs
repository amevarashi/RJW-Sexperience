using rjw;
using System;
using Verse;
using RimWorld;
using UnityEngine;
using RJWSexperience.Logs;
using System.Collections.Generic;

namespace RJWSexperience.SexHistory
{
	public static class RecordRandomizer
	{
		private static readonly rjw.Modules.Shared.Logs.ILog log = LogManager.GetLogger<DebugLogProvider>("RecordRandomizer");

		private static Configurations Settings => SexperienceMod.Settings;

		public static bool Randomize(Pawn pawn)
		{
			log.Message($"Randomize request for {pawn.NameShortColored}");

			int avgSexPerYear = -500;
			bool isVirgin = Rand.Chance(Settings.VirginRatio);

			if (isVirgin)
				log.Message("Rand.Chance rolled virgin");

			if (pawn.story == null)
			{
				log.Error("Tried to randomize records for a pawn without a story");
				return false;
			}

			int sexLifeYears = 0;
			int minsexage = GetMinSexAge(pawn, out string errorMessage);

			if (minsexage < 0)
			{
				log.Error(errorMessage);
				return false;
			}

			log.Message($"Min sex age is {minsexage}");

			float lust = RandomizeLust(pawn);
			log.Message($"Lust set to {lust}");

			if (pawn.ageTracker.AgeBiologicalYears > minsexage)
			{
				sexLifeYears = pawn.ageTracker.AgeBiologicalYears - minsexage;
				avgSexPerYear = (int)(sexLifeYears * Settings.SexPerYear * LustUtility.GetLustFactor(lust));
			}

			log.Message($"Generating {sexLifeYears} years of sex life");
			log.Message($"Average sex/year: {avgSexPerYear}");

			int totalSexCount = 0;

			if (pawn.relations != null && pawn.gender == Gender.Female)
			{
				// Make sure pawn had at least as much sex as ChildrenCount
				// TODO: make sure the sex is vaginal
				// TODO: children may be not humanlike
				totalSexCount += pawn.relations.ChildrenCount;
				pawn.records.AddTo(xxx.CountOfSexWithHumanlikes, pawn.relations.ChildrenCount);
				pawn.records.Set(xxx.CountOfBirthHuman, pawn.relations.ChildrenCount);

				if (pawn.relations.ChildrenCount > 0)
					isVirgin = false;
			}

			if (!isVirgin)
			{
				Dictionary<RecordDef, int> generatedRecords = GeneratePartnerTypeRecords(pawn, avgSexPerYear, sexLifeYears);

				foreach(KeyValuePair<RecordDef, int> record in generatedRecords)
				{
					pawn.records.Set(record.Key, record.Value);
					totalSexCount += record.Value;
				}

				if (totalSexCount > 0)
					pawn.records.AddTo(RsDefOf.Record.SexPartnerCount, Math.Max(1, Rand.Range(0, totalSexCount / 7)));
			}

			pawn.records.Set(xxx.CountOfSex, totalSexCount);
			log.Message($"Splitting {totalSexCount} sex acts between sex types");
			GenerateSextypeRecords(pawn, totalSexCount);
			log.Message($"{pawn.NameShortColored} randomized");
			return true;
		}

		/// <summary>
		/// Assign a new random value to the Lust record of the <paramref name="pawn"/>
		/// </summary>
		/// <param name="pawn"></param>
		/// <returns>Assigned value</returns>
		public static float RandomizeLust(Pawn pawn)
		{
			float value = Utility.RandGaussianLike(Settings.AvgLust - Settings.MaxLustDeviation, Settings.AvgLust + Settings.MaxLustDeviation);

			if (xxx.is_nympho(pawn))
				value = Mathf.Clamp(value, 0, float.MaxValue);

			pawn.records.SetTo(RsDefOf.Record.Lust, value);

			return value;
		}

		/// <summary>
		/// Get minimal biological age the pawn can begin sex life
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="errorMessage">null if no error</param>
		/// <returns>-1 means error. <paramref name="errorMessage"/> should be not null in that case</returns>
		private static int GetMinSexAge(Pawn pawn, out string errorMessage)
		{
			errorMessage = null;

			if (!Settings.MinSexableFromLifestage)
			{
				// Legacy approach. Does not account for long-lived races with human-like aging curve
				return (int)(pawn.RaceProps.lifeExpectancy * Settings.MinSexablePercent);
			}

			LifeStageAge lifeStageAges = pawn.RaceProps.lifeStageAges.Find(x => x.def.reproductive);
			if (lifeStageAges == null)
			{
				errorMessage = $"No reproductive life stage! {pawn.NameShortColored}'s randomization cancelled";
				return -1;
			}
			return (int)lifeStageAges.minAge;
		}

		private static int Randomize(int avg, int dist, int min = 0, int max = int.MaxValue) => (int)Mathf.Clamp(Utility.RandGaussianLike(avg - dist, avg + dist), min, max);

		private static Dictionary<RecordDef, int> GeneratePartnerTypeRecords(Pawn pawn, int avgSexPerYear, int sexLifeYears)
		{
			Dictionary<RecordDef, int> generatedRecords = new Dictionary<RecordDef, int>();
			int deviation = (int)Settings.MaxSexCountDeviation;

			if (xxx.is_rapist(pawn))
			{
				if (xxx.is_zoophile(pawn))
				{
					if (pawn.Has(Quirk.ChitinLover))
						generatedRecords.Add(xxx.CountOfRapedInsects, Randomize(avgSexPerYear, deviation));
					else
						generatedRecords.Add(xxx.CountOfRapedAnimals, Randomize(avgSexPerYear, deviation));
				}
				else
				{
					generatedRecords.Add(xxx.CountOfRapedHumanlikes, Randomize(avgSexPerYear, deviation));
				}

				avgSexPerYear /= 8;
			}

			if (xxx.is_zoophile(pawn))
			{
				if (pawn.Has(Quirk.ChitinLover))
					generatedRecords.Add(xxx.CountOfRapedInsects, Randomize(avgSexPerYear, deviation));
				else
					generatedRecords.Add(xxx.CountOfSexWithAnimals, Randomize(avgSexPerYear, deviation));

				avgSexPerYear /= 10;
			}
			else if (xxx.is_necrophiliac(pawn))
			{
				generatedRecords.Add(xxx.CountOfSexWithCorpse, Randomize(avgSexPerYear, deviation));
				avgSexPerYear /= 4;
			}

			generatedRecords.Add(xxx.CountOfSexWithHumanlikes, Randomize(avgSexPerYear, deviation));

			if (Settings.SlavesBeenRapedExp && pawn.IsSlave)
			{
				generatedRecords.Add(xxx.CountOfBeenRapedByAnimals, Randomize(Rand.Range(-50, 10), Rand.Range(0, 10) * sexLifeYears));
				generatedRecords.Add(xxx.CountOfBeenRapedByHumanlikes, Randomize(0, Rand.Range(0, 100) * sexLifeYears));
			}

			return generatedRecords;
		}

		private static void GenerateSextypeRecords(Pawn pawn, int totalsex)
		{
			float totalweight =
				RJWPreferenceSettings.vaginal +
				RJWPreferenceSettings.anal +
				RJWPreferenceSettings.fellatio +
				RJWPreferenceSettings.cunnilingus +
				RJWPreferenceSettings.rimming +
				RJWPreferenceSettings.double_penetration +
				RJWPreferenceSettings.breastjob +
				RJWPreferenceSettings.handjob +
				RJWPreferenceSettings.mutual_masturbation +
				RJWPreferenceSettings.fingering +
				RJWPreferenceSettings.footjob +
				RJWPreferenceSettings.scissoring +
				RJWPreferenceSettings.fisting +
				RJWPreferenceSettings.sixtynine;
			Gender prefer = PreferredGender(pawn);
			int sex = (int)(totalsex * RJWPreferenceSettings.vaginal / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.VaginalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.anal / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.AnalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fellatio / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(RsDefOf.Record.BlowjobCount, sex);
			else pawn.records.AddTo(RsDefOf.Record.OralSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.cunnilingus / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(RsDefOf.Record.OralSexCount, sex);
			else pawn.records.AddTo(RsDefOf.Record.CunnilingusCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.rimming / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.double_penetration / totalweight) / 2;
			totalsex -= sex;
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.VaginalSexCount, sex);
			pawn.records.AddTo(RsDefOf.Record.AnalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.breastjob / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.handjob / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(RsDefOf.Record.HandjobCount, sex);
			else pawn.records.AddTo(RsDefOf.Record.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fingering / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Female) pawn.records.AddTo(RsDefOf.Record.FingeringCount, sex);
			else pawn.records.AddTo(RsDefOf.Record.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.mutual_masturbation / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(RsDefOf.Record.HandjobCount, sex);
			else pawn.records.AddTo(RsDefOf.Record.FingeringCount, sex);
			pawn.records.AddTo(RsDefOf.Record.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.footjob / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.FootjobCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.scissoring / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fisting / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(RsDefOf.Record.MiscSexualBehaviorCount, sex);

			pawn.records.AddTo(RsDefOf.Record.OralSexCount, totalsex);
			if (prefer == Gender.Male) pawn.records.AddTo(RsDefOf.Record.BlowjobCount, totalsex);
			else pawn.records.AddTo(RsDefOf.Record.CunnilingusCount, totalsex);
		}

		private static Gender PreferredGender(Pawn pawn)
		{
			if (xxx.is_homosexual(pawn))
				return pawn.gender;

			return pawn.gender.Opposite();
		}
	}
}
