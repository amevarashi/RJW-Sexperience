using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
