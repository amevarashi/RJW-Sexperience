using RimWorld;
using Verse;

namespace RJWSexperience
{
	/// <summary>
	/// Thought class that uses record to select active stage
	/// </summary>
	public class Thought_Recordbased : Thought_Memory
	{
		private ThoughtDefExtension_StageFromRecord extension;
		protected ThoughtDefExtension_StageFromRecord Extension => extension ?? (extension = def.GetModExtension<ThoughtDefExtension_StageFromRecord>());

		/// <summary>
		/// This method is called for every thought right after the pawn is assigned
		/// </summary>
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			UpdateCurStage();
			return base.TryMergeWithExistingMemory(out showBubble);
		}

		protected virtual void UpdateCurStage()
		{
			SetForcedStage(Extension.GetStageIndex(pawn));
		}
	}
}
