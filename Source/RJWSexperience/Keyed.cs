using RimWorld;
using rjw;
using RJWSexperience.SexHistory.UI;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	public static class Keyed
	{
		public static string RS_LostVirgin(string pawn, string partner) => "RS_LostVirgin".Translate(pawn.Colorize(Color.yellow), partner.Colorize(Color.yellow));
		public static string RS_SexInfo(string sextype, int sexcount) => string.Format(RS_Sex_Info, sextype, sexcount);
		public static string RS_SatAVG(float avgsat) => string.Format(RS_SAT_AVG, avgsat.ToStringPercent());
		public static string RS_HadBestSexDaysAgo(string days) => "RS_HadBestSexDaysAgo".Translate(days);

		public static readonly string Mod_Title = "RS_Mod_Title".Translate();
		public static readonly string RSTotalGatheredCum = "RSTotalGatheredCum".Translate();
		public static readonly string RS_Sex_Info = "RS_Sex_Info".Translate();
		public static readonly string RS_SAT_AVG = "RS_SAT_AVG".Translate();
		public static readonly string RS_Best_Sextype = "RS_Best_Sextype".Translate();
		public static readonly string RS_Recent_Sextype = "RS_Recent_Sextype".Translate();
		public static readonly string RS_Sex_Partners = "RS_Sex_Partners".Translate();
		public static readonly string RS_Cum_Swallowed = "RS_Cum_Swallowed".Translate();
		public static readonly string RS_Selected_Partner = "RS_Selected_Partner".Translate();
		public static readonly string RS_Sex_Count = "RS_Sex_Count".Translate();
		public static readonly string RS_Orgasms = "RS_Orgasms".Translate();
		public static readonly string RS_Recent_Sex_Partner = "RS_Recent_Sex_Partner".Translate();
		public static readonly string RS_First_Sex_Partner = "RS_First_Sex_Partner".Translate();
		public static readonly string RS_Most_Sex_Partner = "RS_Most_Sex_Partner".Translate();
		public static readonly string RS_Best_Sex_Partner = "RS_Best_Sex_Partner".Translate();
		public static readonly string RS_VirginsTaken = "RS_VirginsTaken".Translate();
		public static readonly string RS_TotalSexHad = "RS_TotalSexHad".Translate();
		public static readonly string RS_Recent_Sex_Partner_ToolTip = "RS_Recent_Sex_Partner_ToolTip".Translate();
		public static readonly string RS_First_Sex_Partner_ToolTip = "RS_First_Sex_Partner_ToolTip".Translate();
		public static readonly string RS_Most_Sex_Partner_ToolTip = "RS_Most_Sex_Partner_ToolTip".Translate();
		public static readonly string RS_Best_Sex_Partner_ToolTip = "RS_Best_Sex_Partner_ToolTip".Translate();
		public static readonly string RS_Raped = "RS_Raped".Translate();
		public static readonly string RS_RapedMe = "RS_RapedMe".Translate();
		public static readonly string RS_Sex_History = "RS_Sex_History".Translate();
		public static readonly string RS_Statistics = "RS_Statistics".Translate();
		public static readonly string RS_PartnerList = "RS_PartnerList".Translate();
		public static readonly string RS_Sexuality = "RS_Sexuality".Translate();
		public static readonly string RS_BeenRaped = "RS_BeenRaped".Translate();
		public static readonly string RS_RapedSomeone = "RS_RapedSomeone".Translate();
		public static readonly string RS_PreferRace = "RS_PreferRace".Translate();
		public static readonly string Lust = "Lust".Translate();
		public static readonly string Unknown = "Unknown".Translate();
		public static readonly string Incest = "Incest".Translate();
		public static readonly string None = "None".Translate();
		public static readonly string RS_Bestiality = "RS_Bestiality".Translate();
		public static readonly string RS_Interspecies = "RS_Interspecies".Translate();
		public static readonly string RS_Necrophile = "RS_Necrophile".Translate();
		public static readonly string RS_SexSkill = "RS_SexSkill".Translate();
		public static readonly string RS_NumofTimes = "RS_NumofTimes".Translate();
		public static readonly string RS_Ago = "RS_Ago".Translate();
		public static readonly string RS_LastSex = "RS_LastSex".Translate().CapitalizeFirst();
		public static readonly string RS_PawnLockDesc = "RS_PawnLockDesc".Translate();
		[MayRequireRoyalty] public static readonly string Slave = "Slave".Translate();

		public static readonly string TabLabelMain = "RSTabLabelMain".Translate();
		public static readonly string TabLabelHistory = "RSTabLabelHistory".Translate();
		public static readonly string TabLabelDebug = "RSTabLabelDebug".Translate();

		public static readonly string Option_1_Label = "RSOption_1_Label".Translate();
		public static readonly string Option_1_Desc = "RSOption_1_Desc".Translate();
		public static readonly string Option_2_Label = "RSOption_2_Label".Translate();
		public static readonly string Option_2_Desc = "RSOption_2_Desc".Translate();
		public static readonly string Option_3_Label = "RSOption_3_Label".Translate();
		public static readonly string Option_3_Desc = "RSOption_3_Desc".Translate();
		public static readonly string Option_4_Label = "RSOption_4_Label".Translate();
		public static readonly string Option_4_Desc = "RSOption_4_Desc".Translate();
		public static readonly string Option_5_Label = "RSOption_5_Label".Translate();
		public static readonly string Option_5_Desc = "RSOption_5_Desc".Translate();
		public static readonly string Option_6_Label = "RSOption_6_Label".Translate();
		public static readonly string Option_6_Desc = "RSOption_6_Desc".Translate();
		public static readonly string Option_7_Label = "RSOption_7_Label".Translate();
		public static readonly string Option_7_Desc = "RSOption_7_Desc".Translate();
		public static readonly string Option_8_Label = "RSOption_8_Label".Translate();
		public static readonly string Option_8_Desc = "RSOption_8_Desc".Translate();
		public static readonly string Option_9_Label = "RSOption_9_Label".Translate();
		public static readonly string Option_9_Desc = "RSOption_9_Desc".Translate();
		public static readonly string Option_10_Label = "RSOption_10_Label".Translate();
		public static readonly string Option_10_Desc = "RSOption_10_Desc".Translate();
		public static readonly string Option_MinSexableFromLifestage_Label = "RSOption_MinSexableFromLifestage_Label".Translate();
		public static readonly string Option_MinSexableFromLifestage_Desc = "RSOption_MinSexableFromLifestage_Desc".Translate();
		public static readonly string Option_MaxSingleLustChange_Label = "RSOption_MaxSingleLustChange_Label".Translate();
		public static readonly string Option_MaxSingleLustChange_Desc = "RSOption_MaxSingleLustChange_Desc".Translate();
		public static readonly string Option_EnableBastardRelation_Label = "RSOption_EnableBastardRelation_Label".Translate();
		public static readonly string Option_EnableBastardRelation_Desc = "RSOption_EnableBastardRelation_Desc".Translate();
		public static readonly string Option_SexCanFillBuckets_Label = "RSOption_SexCanFillBuckets_Label".Translate();
		public static readonly string Option_SexCanFillBuckets_Desc = "RSOption_SexCanFillBuckets_Desc".Translate();
		public static readonly string Option_Debug_Label = "RSOption_Debug_Label".Translate();
		public static readonly string Option_Debug_Desc = "RSOption_Debug_Desc".Translate();
		public static readonly string Option_EnableSexHistory_Label = "RSOption_EnableSexHistory_Label".Translate();
		public static readonly string Option_EnableSexHistory_Desc = "RSOption_EnableSexHistory_Desc".Translate();
		public static readonly string Option_HideGizmoWhenDrafted_Label = "RSOption_HideGizmoWhenDrafted_Label".Translate();
		public static readonly string Option_HideGizmoWhenDrafted_Desc = "RSOption_HideGizmoWhenDrafted_Desc".Translate();
		public static readonly string Option_HideGizmoWithRJW_Label = "RSOption_HideGizmoWithRJW_Label".Translate();
		public static readonly string Option_HideGizmoWithRJW_Desc = "RSOption_HideGizmoWithRJW_Desc".Translate();
		public static readonly string Button_ResetToDefault = "Button_ResetToDefault".Translate();
		public static readonly string Option_VirginityCheck_M2M_Label = "RSOption_VirginityCheck_M2M_Label".Translate();
		public static readonly string Option_VirginityCheck_M2M_Desc = "RSOption_VirginityCheck_M2M_Desc".Translate();
		public static readonly string Option_VirginityCheck_F2F_Label = "RSOption_VirginityCheck_F2F_Label".Translate();
		public static readonly string Option_VirginityCheck_F2F_Desc = "RSOption_VirginityCheck_F2F_Desc".Translate();

		public static string Translate(this PartnerOrderMode mode)
		{
			switch (mode)
			{
				case PartnerOrderMode.Normal:
				default:
					return "RS_PONormal".Translate();
				case PartnerOrderMode.Recent:
					return "RS_PoRecent".Translate();
				case PartnerOrderMode.Most:
					return "RS_PoMost".Translate();
				case PartnerOrderMode.Name:
					return "RS_PoName".Translate();
			}
		}

		public static readonly string[] Sextype =
		{
			((xxx.rjwSextype)0).ToString().Translate(),
			((xxx.rjwSextype)1).ToString().Translate(),
			((xxx.rjwSextype)2).ToString().Translate(),
			((xxx.rjwSextype)3).ToString().Translate(),
			((xxx.rjwSextype)4).ToString().Translate(),
			((xxx.rjwSextype)5).ToString().Translate(),
			((xxx.rjwSextype)6).ToString().Translate(),
			((xxx.rjwSextype)7).ToString().Translate(),
			((xxx.rjwSextype)8).ToString().Translate(),
			((xxx.rjwSextype)9).ToString().Translate(),
			((xxx.rjwSextype)10).ToString().Translate(),
			((xxx.rjwSextype)11).ToString().Translate(),
			((xxx.rjwSextype)12).ToString().Translate(),
			((xxx.rjwSextype)13).ToString().Translate(),
			((xxx.rjwSextype)14).ToString().Translate(),
			((xxx.rjwSextype)15).ToString().Translate(),
			((xxx.rjwSextype)16).ToString().Translate(),
			((xxx.rjwSextype)17).ToString().Translate(),
			((xxx.rjwSextype)18).ToString().Translate(),
			((xxx.rjwSextype)19).ToString().Translate(),
			((xxx.rjwSextype)20).ToString().Translate()
		};

		public static readonly string[] Sexuality =
		{
			((Orientation)0).ToString().Translate(),
			((Orientation)1).ToString().Translate(),
			((Orientation)2).ToString().Translate(),
			((Orientation)3).ToString().Translate(),
			((Orientation)4).ToString().Translate(),
			((Orientation)5).ToString().Translate(),
			((Orientation)6).ToString().Translate(),
			((Orientation)7).ToString().Translate(),
			((Orientation)8).ToString().Translate(),
			((Orientation)9).ToString().Translate(),
			((Orientation)10).ToString().Translate()
		};
	}
}
