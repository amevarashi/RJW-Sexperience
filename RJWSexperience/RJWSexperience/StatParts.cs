using RimWorld;
using System;
using UnityEngine;
using Verse;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience
{
	/// <summary>
	/// Lust changes SexFrequency stat
	/// </summary>
	public class StatPart_Lust : StatPart
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float factor;

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && (req.Thing is Pawn pawn))
			{
				return Keyed.LustStatFactor(String.Format("{0:0.##}", GetLustFactor(pawn) * factor * 100));
			}
			return null;
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && (req.Thing is Pawn pawn))
				val *= GetLustFactor(pawn) * factor;
		}

		public static float GetLustFactor(Pawn pawn)
		{
			float lust = pawn.records.GetValue(VariousDefOf.Lust) * Configurations.LustEffectPower;
			if (lust < 0)
			{
				lust = Mathf.Exp((lust + 200f * Mathf.Log(10f)) / 100f) - 100f;
			}
			else
			{
				lust = Mathf.Sqrt(100f * (lust + 25f)) - 50f;
			}

			return 1 + lust / 100f;
		}
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
				return Keyed.SlaveStatFactor(String.Format("{0:0.##}", factor * 100));
			}
			return Keyed.SlaveStatFactorDefault;
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
