using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class StatPart_GenderPrimacy : StatPart
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float modifier;

		public override string ExplanationPart(StatRequest req)
		{
			if (!req.HasThing || !(req.Thing is Pawn pawn))
				return null;

			return $"{Keyed.MemeStatFactor}: x{GetModifier(pawn).ToStringPercent()}";
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing || !(req.Thing is Pawn pawn))
				return;

			val *= GetModifier(pawn);
		}

		private float GetModifier(Pawn pawn)
		{
			if (pawn.Ideo == null)
				return 1f;

			Gender supremeGender = pawn.Ideo.SupremeGender;

			if (pawn.gender == supremeGender)
				return modifier;
			else if (pawn.gender == supremeGender.Opposite())
				return 1f / modifier;

			return 1f;
		}
	}
}
