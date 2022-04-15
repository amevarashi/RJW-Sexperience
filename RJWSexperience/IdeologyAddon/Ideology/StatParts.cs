using RimWorld;
using System;
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

			Ideo ideo = pawn.Ideo;
			float fact = 1f;
			if (ideo?.memes.NullOrEmpty() == false)
			{
				for (int i = 0; i < ideo.memes.Count; i++)
				{
					if (ideo.memes[i] == MemeDefOf.MaleSupremacy)
					{
						if (pawn.gender == Gender.Male) fact = modifier;
						else if (pawn.gender == Gender.Female) fact = 1 / modifier;
						break;
					}
					else if (ideo.memes[i] == MemeDefOf.FemaleSupremacy)
					{
						if (pawn.gender == Gender.Male) fact = 1 / modifier;
						else if (pawn.gender == Gender.Female) fact = modifier;
						break;
					}
				}
			}

			return Keyed.MemeStatFactor(String.Format("{0:0.##}", fact * 100));
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing || !(req.Thing is Pawn pawn))
				return;

			Ideo ideo = pawn.Ideo;

			if (ideo?.memes.NullOrEmpty() == false)
			{
				for (int i = 0; i < ideo.memes.Count; i++)
				{
					if (ideo.memes[i] == MemeDefOf.MaleSupremacy)
					{
						if (pawn.gender == Gender.Male) val *= modifier;
						else if (pawn.gender == Gender.Female) val /= modifier;
						break;
					}
					else if (ideo.memes[i] == MemeDefOf.FemaleSupremacy)
					{
						if (pawn.gender == Gender.Male) val /= modifier;
						else if (pawn.gender == Gender.Female) val *= modifier;
						break;
					}
				}
			}
		}
	}
}
