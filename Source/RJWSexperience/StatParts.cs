using RimWorld;
using Verse;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience
{
	/// <summary>
	/// Lust changes SexFrequency stat
	/// </summary>
	public class StatPart_Lust : StatPart
	{
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && (req.Thing is Pawn pawn))
			{
				return $"{Keyed.Lust.CapitalizeFirst()}: x{GetLustFactor(pawn).ToStringPercent()}";
			}
			return null;
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && (req.Thing is Pawn pawn))
				val *= GetLustFactor(pawn);
		}

		protected float GetLustFactor(Pawn pawn) => LustUtility.GetLustFactor(pawn.records.GetValue(RsDefOf.Record.Lust));
	}

	/// <summary>
	/// Make slaves more vulnurable
	/// </summary>
	public class StatPart_Slave : StatPart
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float factor;

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && ((req.Thing as Pawn)?.IsSlave == true))
			{
				return $"{Keyed.Slave.CapitalizeFirst()}: x{factor.ToStringPercent()}";
			}
			return null;
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && ((req.Thing as Pawn)?.IsSlave == true))
			{
				val *= factor;
			}
		}
	}
}
