using rjw;
using System;
using Verse;
using RimWorld;
using UnityEngine;
using RJWSexperience.Logs;

namespace RJWSexperience.SexHistory
{
	public static class RecordRandomizer
	{
		private static readonly rjw.Modules.Shared.Logs.ILog log = LogManager.GetLogger<DebugLogProvider>("RecordRandomizer");

		private static Settings.SettingsTabHistory Settings => SexperienceMod.Settings.History;

		public static bool Randomize(Pawn pawn)
		{
			log.Message($"Randomize request for {pawn.NameShortColored}");

			int avgsex = -500;
			bool isvirgin = Rand.Chance(Settings.VirginRatio);
			int totalsex = 0;
			int totalbirth = 0;

			if (isvirgin)
				log.Message("Rand.Chance rolled virgin");

			if (pawn.story != null)
			{
				int sexableage = 0;
				int minsexage = 0;
				if (Settings.MinSexableFromLifestage)
				{
					LifeStageAge lifeStageAges = pawn.RaceProps.lifeStageAges.Find(x => x.def.reproductive);
					if (lifeStageAges == null)
					{
						log.Message($"No reproductive life stage! {pawn.NameShortColored}'s randomization cancelled");
						return false;
					}
					minsexage = (int)lifeStageAges.minAge;
				}
				else
				{
					minsexage = (int)(pawn.RaceProps.lifeExpectancy * Settings.MinSexablePercent);
				}

				log.Message($"Min sex age is {minsexage}");

				float lust = RandomizeLust(pawn);
				log.Message($"Lust set to {lust}");

				if (pawn.ageTracker.AgeBiologicalYears > minsexage)
				{
					sexableage = pawn.ageTracker.AgeBiologicalYears - minsexage;
					avgsex = (int)(sexableage * Settings.SexPerYear * LustUtility.GetLustFactor(lust));
				}

				log.Message($"Generating {sexableage} years of sex life");
				log.Message($"Average sex/year: {avgsex}");

				if (pawn.relations != null && pawn.gender == Gender.Female)
				{
					totalbirth += pawn.relations.ChildrenCount;
					totalsex += totalbirth;
					pawn.records?.AddTo(xxx.CountOfSexWithHumanlikes, totalbirth);
					pawn.records?.SetTo(xxx.CountOfBirthHuman, totalbirth);
					if (totalbirth > 0) isvirgin = false;
				}
				if (!isvirgin)
				{
					totalsex += GeneratePartnerTypeRecords(pawn, avgsex);

					if (Settings.SlavesBeenRapedExp && pawn.IsSlave)
					{
						totalsex += RandomizeRecord(pawn, xxx.CountOfBeenRapedByAnimals, Rand.Range(-50, 10), Rand.Range(0, 10) * sexableage);
						totalsex += RandomizeRecord(pawn, xxx.CountOfBeenRapedByHumanlikes, 0, Rand.Range(0, 100) * sexableage);
					}
				}
			}
			pawn.records?.SetTo(xxx.CountOfSex, totalsex);
			log.Message($"Splitting {totalsex} sex acts between sex types");
			GenerateSextypeRecords(pawn, totalsex);
			log.Message($"{pawn.NameShortColored} randomized");
			return true;
		}

		public static float RandomizeLust(Pawn pawn)
		{
			float value = Utility.RandGaussianLike(Settings.AvgLust - Settings.MaxLustDeviation, Settings.AvgLust + Settings.MaxLustDeviation);
			float minValue;

			if (xxx.is_nympho(pawn))
				minValue = 0;
			else
				minValue = float.MinValue;

			value = Mathf.Clamp(value, minValue, float.MaxValue);
			float recordvalue = pawn.records.GetValue(VariousDefOf.Lust);
			pawn.records.AddTo(VariousDefOf.Lust, value - recordvalue);

			return value;
		}

		private static int RandomizeRecord(Pawn pawn, RecordDef record, int avg, int dist, int min = 0, int max = int.MaxValue)
		{
			int value = (int)Mathf.Clamp(Utility.RandGaussianLike(avg - dist, avg + dist), min, max);
			int recordvalue = pawn.records.GetAsInt(record);
			pawn.records.AddTo(record, value - recordvalue);

			return value;
		}

		private static int GeneratePartnerTypeRecords(Pawn pawn, int avgsex)
		{
			int deviation = (int)Settings.MaxSexCountDeviation;
			int totalSexCount = 0;

			if (xxx.is_rapist(pawn))
			{
				if (xxx.is_zoophile(pawn))
				{
					if (pawn.Has(Quirk.ChitinLover)) totalSexCount += RandomizeRecord(pawn, xxx.CountOfRapedInsects, avgsex, deviation);
					else totalSexCount += RandomizeRecord(pawn, xxx.CountOfRapedAnimals, avgsex, deviation);
				}
				else
				{
					totalSexCount += RandomizeRecord(pawn, xxx.CountOfRapedHumanlikes, avgsex, deviation);
				}

				avgsex /= 8;
			}

			if (xxx.is_zoophile(pawn))
			{
				if (pawn.Has(Quirk.ChitinLover)) totalSexCount += RandomizeRecord(pawn, xxx.CountOfRapedInsects, avgsex, deviation);
				else totalSexCount += RandomizeRecord(pawn, xxx.CountOfSexWithAnimals, avgsex, deviation);
				avgsex /= 10;
			}
			else if (xxx.is_necrophiliac(pawn))
			{
				totalSexCount += RandomizeRecord(pawn, xxx.CountOfSexWithCorpse, avgsex, deviation);
				avgsex /= 4;
			}

			totalSexCount += RandomizeRecord(pawn, xxx.CountOfSexWithHumanlikes, avgsex, deviation);

			if (totalSexCount > 0)
				pawn.records.AddTo(VariousDefOf.SexPartnerCount, Math.Max(1, Rand.Range(0, totalSexCount / 7)));

			return totalSexCount;
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
			Gender prefer = pawn.PreferGender();
			int sex = (int)(totalsex * RJWPreferenceSettings.vaginal / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.VaginalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.anal / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.AnalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fellatio / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(VariousDefOf.BlowjobCount, sex);
			else pawn.records.AddTo(VariousDefOf.OralSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.cunnilingus / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(VariousDefOf.OralSexCount, sex);
			else pawn.records.AddTo(VariousDefOf.CunnilingusCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.rimming / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.double_penetration / totalweight) / 2;
			totalsex -= sex;
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.VaginalSexCount, sex);
			pawn.records.AddTo(VariousDefOf.AnalSexCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.breastjob / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.handjob / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(VariousDefOf.HandjobCount, sex);
			else pawn.records.AddTo(VariousDefOf.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fingering / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Female) pawn.records.AddTo(VariousDefOf.FingeringCount, sex);
			else pawn.records.AddTo(VariousDefOf.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.mutual_masturbation / totalweight);
			totalsex -= sex;
			if (prefer == Gender.Male) pawn.records.AddTo(VariousDefOf.HandjobCount, sex);
			else pawn.records.AddTo(VariousDefOf.FingeringCount, sex);
			pawn.records.AddTo(VariousDefOf.GenitalCaressCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.footjob / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.FootjobCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.scissoring / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.MiscSexualBehaviorCount, sex);

			sex = (int)(totalsex * RJWPreferenceSettings.fisting / totalweight);
			totalsex -= sex;
			pawn.records.AddTo(VariousDefOf.MiscSexualBehaviorCount, sex);

			pawn.records.AddTo(VariousDefOf.OralSexCount, totalsex);
			if (prefer == Gender.Male) pawn.records.AddTo(VariousDefOf.BlowjobCount, totalsex);
			else pawn.records.AddTo(VariousDefOf.CunnilingusCount, totalsex);
		}
	}
}
