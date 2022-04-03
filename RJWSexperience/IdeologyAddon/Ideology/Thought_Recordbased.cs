using RimWorld;
using System.Collections.Generic;

namespace RJWSexperience.Ideology
{
	/// <summary>
	/// ThoughtDef using record
	/// </summary>
	public class ThoughtDef_Recordbased : ThoughtDef
	{
		public RecordDef recordDef;
		public List<float> minimumValueforStage = new List<float>();
		public float increment;
	}

	/// <summary>
	/// Thought class using record.
	/// </summary>
	public class Thought_Recordbased : Thought_Memory
	{
		protected ThoughtDef_Recordbased Def => (ThoughtDef_Recordbased)def;
		protected RecordDef recordDef => Def.recordDef;
		protected List<float> minimumValueforStage => Def.minimumValueforStage;

		public override int CurStageIndex
		{
			get
			{
				float value = pawn?.records?.GetValue(recordDef) ?? 0f;
				for (int i = minimumValueforStage.Count - 1; i > 0; i--)
				{
					if (minimumValueforStage[i] < value) return i + 1;
				}
				return 0;
			}
		}
	}
}
