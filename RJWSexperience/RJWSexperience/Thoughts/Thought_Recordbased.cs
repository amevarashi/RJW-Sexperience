using RimWorld;
using System.Collections.Generic;

namespace RJWSexperience
{
	/// <summary>
	/// Thought class using record.
	/// </summary>
	public class Thought_Recordbased : Thought_Memory
	{
		protected ThoughtDef_Recordbased Def => (ThoughtDef_Recordbased)def;
		protected RecordDef RecordDef => Def.recordDef;
		protected List<float> MinimumValueforStage => Def.minimumValueforStage;
		protected float Increment => Def.increment;

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
