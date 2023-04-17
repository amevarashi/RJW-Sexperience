using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience
{
	[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
	public class ThoughtDefExtension_StageFromRecord : DefModExtension
	{
		public RecordDef recordDef;
		public List<float> minimumValueforStage = new List<float>();

		public int GetStageIndex(Pawn pawn)
		{
			float value = pawn?.records?.GetValue(recordDef) ?? 0f;

			for (int i = minimumValueforStage.Count - 1; i > 0; i--)
			{
				if (minimumValueforStage[i] < value)
				{
					return i;
				}
			}

			return 0;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}

			if (recordDef == null)
			{
				yield return "<recordDef> is null";
			}

			if (minimumValueforStage.NullOrEmpty())
			{
				yield return "<minimumValueforStage> should have an entry for every stage";
			}

			for (int i = 0; i < minimumValueforStage.Count - 1; i++)
			{
				if (minimumValueforStage[i] > minimumValueforStage[i + 1])
				{
					yield return "Values in <minimumValueforStage> should be ordered from the lowest to the highest";
				}
			}
		}
	}
}
