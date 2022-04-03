using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience
{
	/// <summary>
	/// ThoughtDef using record
	/// </summary>
	public class ThoughtDef_Recordbased : ThoughtDef
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public RecordDef recordDef;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<float> minimumValueforStage = new List<float>();
	}

	/// <summary>
	/// Thought class using record.
	/// </summary>
	public class Thought_Recordbased : Thought_Memory
	{
		protected ThoughtDef_Recordbased Def => (ThoughtDef_Recordbased)def;
		protected RecordDef RecordDef => Def.recordDef;
		protected List<float> MinimumValueforStage => Def.minimumValueforStage;

		public override int CurStageIndex
		{
			get
			{
				float value = pawn?.records?.GetValue(RecordDef) ?? 0f;
				for (int i = MinimumValueforStage.Count - 1; i > 0; i--)
				{
					if (MinimumValueforStage[i] < value) return i + 1;
				}
				return 0;
			}
		}
	}
}
