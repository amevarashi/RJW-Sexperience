using rjw;
using RJWSexperience.Logs;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RJWSexperience
{
	public static class LustUtility
	{
		/// <summary>
		///  ~0.023.
		///  No need to calculate this every call
		/// </summary>
		private static readonly float magicNum1 = Mathf.Log(10f) / 100;

		/// <summary>
		/// Transforms lust value into a stat multiplier
		/// </summary>
		/// <param name="lust"></param>
		/// <returns>Positive value</returns>
		public static float GetLustFactor(float lust)
		{
			float effectiveLust = lust * SexperienceMod.Settings.LustEffectPower;
			if (effectiveLust < 0)
			{
				effectiveLust = Mathf.Exp((effectiveLust + 200f) * magicNum1) - 100f;
			}
			else
			{
				effectiveLust = Mathf.Sqrt((effectiveLust + 25f) * 100f) - 50f;
			}

			return 1 + (effectiveLust / 100f);
		}

		public static void DrawGraph(Rect graphRect)
		{
			List<SimpleCurveDrawInfo> curves = new List<SimpleCurveDrawInfo>();
			FloatRange lustRange = new FloatRange(-300f, 300f);

			SimpleCurveDrawInfo simpleCurveDrawInfo = new SimpleCurveDrawInfo
			{
				color = Color.yellow,
				label = "Sex freq mult",
				valueFormat = "x{0}",
				curve = new SimpleCurve()
			};
			for (float lust = lustRange.min; lust <= lustRange.max; lust++)
			{
				simpleCurveDrawInfo.curve.Add(new CurvePoint(lust, GetLustFactor(lust)), false);
			}

			curves.Add(simpleCurveDrawInfo);

			SimpleCurveDrawerStyle curveDrawerStyle = new SimpleCurveDrawerStyle
			{
				UseFixedSection = true,
				FixedSection = lustRange,
				UseFixedScale = true,
				FixedScale = new Vector2(0f, GetLustFactor(lustRange.max)),
				DrawPoints = false,
				DrawBackgroundLines = true,
				DrawCurveMousePoint = true,
				DrawMeasures = true,
				MeasureLabelsXCount = 8,
				MeasureLabelsYCount = 3,
				XIntegersOnly = true,
				YIntegersOnly = false,
				LabelX = Keyed.Lust
			};
			SimpleCurveDrawer.DrawCurves(graphRect, curves, curveDrawerStyle);
		}

		public static void UpdateLust(SexProps props, float satisfaction, float baseSatisfaction)
		{
			float? lust = props.pawn.records?.GetValue(RsDefOf.Record.Lust);

			if (lust == null)
				return;

			float lustDelta;

			if (props.sexType != xxx.rjwSextype.Masturbation)
			{
				lustDelta = satisfaction - baseSatisfaction;
				if (Mathf.Sign(lustDelta) == Mathf.Sign((float)lust)) // Only if getting closer to the limit
					lustDelta *= LustIncrementFactor((float)lust);
				lustDelta = Mathf.Clamp(lustDelta, -SexperienceMod.Settings.MaxSingleLustChange, SexperienceMod.Settings.MaxSingleLustChange); // If the sex is satisfactory, lust grows up. Declines at the opposite.
			}
			else
			{
				lustDelta = Mathf.Clamp(satisfaction * satisfaction * LustIncrementFactor((float)lust), 0, SexperienceMod.Settings.MaxSingleLustChange); // Masturbation always increases lust.
			}

			if (lustDelta == 0)
				return;

			LogManager.GetLogger<DebugLogProvider>("LustUtility").Message($"{props.pawn.NameShortColored}'s lust changed by {lustDelta} (from {lust})");
			props.pawn.records.AddTo(RsDefOf.Record.Lust, lustDelta);
		}

		private static float LustIncrementFactor(float lust)
		{
			return Mathf.Exp(-Mathf.Pow(lust / SexperienceMod.Settings.LustLimit, 2));
		}
	}
}
