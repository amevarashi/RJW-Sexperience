using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class RitualOutcomeComp_HediffBased : RitualOutcomeComp_QualitySingleOffset
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public HediffDef hediffDef = null;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float minSeverity = 0;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public string roleId = "";

		protected override string LabelForDesc => label;
		public override bool DataRequired => false;
		public override bool Applies(LordJob_Ritual ritual)
		{
			Pawn victim = null;
			foreach (RitualRole ritualRole in ritual.assignments.AllRolesForReading)
			{
				if (ritualRole?.id.Contains(roleId) == true)
				{
					victim = ritual.assignments.FirstAssignedPawn(ritualRole);
				}
			}
			if (victim != null && hediffDef != null)
			{
				Hediff hediff = victim.health.hediffSet.GetFirstHediffOfDef(hediffDef);
				if (hediff?.Severity >= minSeverity)
				{
					return true;
				}
			}
			return false;
		}

		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return new ExpectedOutcomeDesc
			{
				label = LabelForDesc.CapitalizeFirst(),
				present = false,
				uncertainOutcome = true,
				effect = ExpectedOffsetDesc(true, -1f),
				quality = qualityOffset,
				positive = true
			};
		}
	}

	public class RitualOutcomeComp_NeedBased : RitualOutcomeComp_QualitySingleOffset
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public NeedDef needDef = null;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float minAvgNeed = 0;

		protected override string LabelForDesc => label;
		public override bool DataRequired => false;
		public override bool Applies(LordJob_Ritual ritual)
		{
			float avgNeed = 0;
			foreach (Pawn pawn in ritual.assignments.AllPawns)
			{
				avgNeed += pawn.needs?.TryGetNeed(needDef)?.CurLevel ?? 0f;
			}
			avgNeed /= ritual.assignments.AllPawns.Count;
			return avgNeed >= minAvgNeed;
		}

		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return new ExpectedOutcomeDesc
			{
				label = LabelForDesc.CapitalizeFirst(),
				present = false,
				uncertainOutcome = true,
				effect = ExpectedOffsetDesc(true, -1f),
				quality = qualityOffset,
				positive = true
			};
		}
	}
}
