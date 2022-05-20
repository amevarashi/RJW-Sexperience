using rjw;
using System;
using Verse;
using RimWorld;
using UnityEngine;

namespace RJWSexperience
{
	public static class RecordRandomizer
	{
		private static Settings.SettingsTabHistory Settings => SexperienceMod.Settings.History;

		public static void Randomize(Pawn pawn)
		{
			int avgsex = -500;
			bool isvirgin = Rand.Chance(Settings.VirginRatio);
			int totalsex = 0;
			int totalbirth = 0;
			int deviation = (int)Settings.MaxSexCountDeviation;
			if (pawn.story != null)
			{
				float lust = RandomizeLust(pawn);

				int sexableage = 0;
				int minsexage = 0;
				if (Settings.MinSexableFromLifestage)
					minsexage = (int)pawn.RaceProps.lifeStageAges.Find(x => x.def.reproductive).minAge;
				else
					minsexage = (int)(pawn.RaceProps.lifeExpectancy * Settings.MinSexablePercent);

				if (pawn.ageTracker.AgeBiologicalYears > minsexage)
				{
					sexableage = pawn.ageTracker.AgeBiologicalYears - minsexage;
					avgsex = (int)(sexableage * Settings.SexPerYear * LustUtility.GetLustFactor(lust));
				}

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
					if (xxx.is_rapist(pawn))
					{
						if (xxx.is_zoophile(pawn))
						{
							if (pawn.Has(Quirk.ChitinLover)) totalsex += RandomizeRecord(pawn, xxx.CountOfRapedInsects, avgsex, deviation);
							else totalsex += RandomizeRecord(pawn, xxx.CountOfRapedAnimals, avgsex, deviation);
						}
						else
						{
							totalsex += RandomizeRecord(pawn, xxx.CountOfRapedHumanlikes, avgsex, deviation);
						}

						avgsex /= 8;
					}

					if (xxx.is_zoophile(pawn))
					{
						if (pawn.Has(Quirk.ChitinLover)) totalsex += RandomizeRecord(pawn, xxx.CountOfRapedInsects, avgsex, deviation);
						else totalsex += RandomizeRecord(pawn, xxx.CountOfSexWithAnimals, avgsex, deviation);
						avgsex /= 10;
					}
					else if (xxx.is_necrophiliac(pawn))
					{
						totalsex += RandomizeRecord(pawn, xxx.CountOfSexWithCorpse, avgsex, deviation);
						avgsex /= 4;
					}

					if (Settings.SlavesBeenRapedExp && pawn.IsSlave)
					{
						totalsex += RandomizeRecord(pawn, xxx.CountOfBeenRapedByAnimals, Rand.Range(-50, 10), Rand.Range(0, 10) * sexableage);
						totalsex += RandomizeRecord(pawn, xxx.CountOfBeenRapedByHumanlikes, 0, Rand.Range(0, 100) * sexableage);
					}

					totalsex += RandomizeRecord(pawn, xxx.CountOfSexWithHumanlikes, avgsex, deviation);

					if (totalsex > 0) pawn.records.AddTo(VariousDefOf.SexPartnerCount, Math.Max(1, Rand.Range(0, totalsex / 7)));
				}
			}
			pawn.records?.SetTo(xxx.CountOfSex, totalsex);
			GenerateSextypeRecords(pawn, totalsex);
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
