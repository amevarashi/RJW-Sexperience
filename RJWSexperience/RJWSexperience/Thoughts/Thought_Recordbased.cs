using RimWorld;
using System.Collections.Generic;

namespace RJWSexperience
{
	/// <summary>
	/// Thought class using record.
	/// </summary>
	public class Thought_Recordbased : Thought_Memory
	{
		private ThoughtDefExtension_StageFromRecord extension;

		protected ThoughtDefExtension_StageFromRecord Extension
		{
			get
			{
				if (extension == null)
					extension = def.GetModExtension<ThoughtDefExtension_StageFromRecord>();
				return extension;
			}
		}

		protected RecordDef RecordDef => Extension.recordDef;
		protected List<float> MinimumValueforStage => Extension.minimumValueforStage;

		public override int CurStageIndex
		{
			get
			{
				float value = pawn?.records?.GetValue(RecordDef) ?? 0f;
				for (int i = MinimumValueforStage.Count - 1; i > 0; i--)
				{
					if (MinimumValueforStage[i] < value) return i;
				}
				return 0;
			}
		}
	}
}
