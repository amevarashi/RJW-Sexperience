using RimWorld;
using System.Collections.Generic;

namespace RJWSexperience
{
	public class Thought_Opinionbased : Thought_Memory
	{
		private ThoughtDefExtension_StageFromOpinion extension;

		protected ThoughtDefExtension_StageFromOpinion Extension
		{
			get
			{
				if (extension == null)
					extension = def.GetModExtension<ThoughtDefExtension_StageFromOpinion>();
				return extension;
			}
		}

		protected List<float> MinimumValueforStage => Extension.minimumValueforStage;

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
