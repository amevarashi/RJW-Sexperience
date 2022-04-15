using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience.Ideology
{
	/// <summary>
	/// ThoughtDef using opinion
	/// </summary>
	public class ThoughtDef_Opinionbased : ThoughtDef
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<float> minimumValueforStage = new List<float>();
	}

	/// <summary>
	/// Thought class using record.
	/// </summary>
	public class Thought_Opinionbased : Thought_Memory
	{
		protected ThoughtDef_Opinionbased Def => (ThoughtDef_Opinionbased)def;
		protected List<float> MinimumValueforStage => Def.minimumValueforStage;

		public override int CurStageIndex
		{
			get
			{
				float value = 0f;
				if (otherPawn != null) value = pawn.relations?.OpinionOf(otherPawn) ?? 0f;
				for (int i = MinimumValueforStage.Count - 1; i > 0; i--)
				{
					if (MinimumValueforStage[i] < value) return i;
				}
				return 0;
			}
		}
	}
}
