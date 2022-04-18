using RimWorld;
using Verse;

namespace RJWSexperience
{
	public class Thought_IncreaseRecord : Thought_Recordbased
	{
		protected float recordIncrement;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref recordIncrement, "recordIncrement", recordIncrement, true);
		}

		public override void ThoughtInterval()
		{
			base.ThoughtInterval();
			if (recordIncrement != 0)
			{
				pawn.records.AddTo(RecordDef, recordIncrement);
				recordIncrement = 0;
			}

		}

		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= def.stackLimit)
			{
				Thought_IncreaseRecord thought_Memory = (Thought_IncreaseRecord)thoughts.memories.OldestMemoryInGroup(this);
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

		public override void Init()
		{
			base.Init();
			recordIncrement = Increment;
		}
		protected virtual void Merged()
		{
			age = 0;
			recordIncrement += Increment;
		}
	}
}
