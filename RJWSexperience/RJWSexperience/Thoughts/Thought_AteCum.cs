using RimWorld;

namespace RJWSexperience
{
	public class Thought_AteCum : Thought_Recordbased
	{
		public override int CurStageIndex
		{
			get
			{
				if (pawn?.health?.hediffSet?.HasHediff(VariousDefOf.CumAddiction) ?? false)
					return def.stages.Count - 1;
				return base.CurStageIndex;
			}
		}

		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= def.stackLimit)
			{
				Thought_AteCum thought_Memory = (Thought_AteCum)thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.def.DurationTicks / 2);
					thought_Memory.Merged();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		protected virtual void Merged()
		{
			age = 0;
		}
	}
}
