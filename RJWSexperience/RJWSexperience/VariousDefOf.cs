using RimWorld;
using Verse;

namespace RJWSexperience
{
	[DefOf]
	public static class VariousDefOf
	{
		public static readonly RecordDef NumofEatenCum;
		public static readonly RecordDef AmountofEatenCum;
		public static readonly RecordDef Lust;
		public static readonly RecordDef VaginalSexCount;
		public static readonly RecordDef AnalSexCount;
		public static readonly RecordDef OralSexCount;
		public static readonly RecordDef BlowjobCount;
		public static readonly RecordDef CunnilingusCount;
		public static readonly RecordDef GenitalCaressCount;
		public static readonly RecordDef HandjobCount;
		public static readonly RecordDef FingeringCount;
		public static readonly RecordDef FootjobCount;
		public static readonly RecordDef MiscSexualBehaviorCount;
		public static readonly RecordDef SexPartnerCount;
		public static readonly RecordDef OrgasmCount;
		public static readonly SkillDef Sex;
		public static readonly ThingDef CumBucket;
		public static readonly ThingDef GatheredCum;
		public static readonly ThingDef FilthCum;
		public static readonly ChemicalDef Cum;
		public static readonly NeedDef Chemical_Cum;
		public static readonly TraitDef Virgin;
		public static readonly JobDef CleanSelfwithBucket;
		public static readonly KeyBindingDef OpenSexStatistics;
		public static readonly StatDef SexAbility;

		public static readonly HediffDef CumAddiction;
		public static readonly HediffDef CumTolerance;
		[MayRequire("rjw.cum")] public static readonly HediffDef Hediff_CumController;
		[MayRequire("rjw.cum")] public static readonly HediffDef Hediff_Cum; //for humans & animals
		[MayRequire("rjw.cum")] public static readonly HediffDef Hediff_InsectSpunk;
	}
}
