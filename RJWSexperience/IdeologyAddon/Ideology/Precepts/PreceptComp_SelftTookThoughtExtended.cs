using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class PreceptComp_SelfTookThoughtTagged : PreceptComp_SelfTookMemoryThought
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public string tag;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool exclusive = false;

		public PreceptComp_SelfTookThoughtTagged() { }

		public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			if (tag != null)
			{
				if (ev.args.TryGetArg(HistoryEventArgsNamesCustom.Tag, out string tags))
				{
					if (IdeoUtility.ContainAll(tags, tag.Replace(" ", "").Split(',')) ^ exclusive)
					{
						TookThought(ev, precept, canApplySelfTookThoughts);
					}
				}
				else if (exclusive)
				{
					TookThought(ev, precept, canApplySelfTookThoughts);
				}
			}
			else
			{
				TookThought(ev, precept, canApplySelfTookThoughts);
			}
		}

		protected virtual void TookThought(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			if (ev.def != this.eventDef || !canApplySelfTookThoughts)
			{
				return;
			}
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			Pawn partner = ev.args.GetArg<Pawn>(HistoryEventArgsNamesCustom.Partner);
			if (arg.needs?.mood != null && (!this.onlyForNonSlaves || !arg.IsSlave))
			{
				if (this.thought.minExpectationForNegativeThought != null && ExpectationsUtility.CurrentExpectationFor(arg).order < this.thought.minExpectationForNegativeThought.order)
				{
					return;
				}
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				if (thought_Memory is Thought_KilledInnocentAnimal thought_KilledInnocentAnimal && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out Pawn animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				if (thought_Memory is Thought_MemoryObservation thought_MemoryObservation && ev.args.TryGetArg<Corpse>(HistoryEventArgsNames.Subject, out Corpse target))
				{
					thought_MemoryObservation.Target = target;
				}
				arg.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, partner);
			}
		}
	}

	public class PreceptComp_KnowsMemoryThoughtTagged : PreceptComp_KnowsMemoryThought
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public string tag;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool exclusive = false;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool applyonpartner = false;

		public PreceptComp_KnowsMemoryThoughtTagged() { }

		public override void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
			if (!applyonpartner && ev.args.TryGetArg(HistoryEventArgsNamesCustom.Partner, out Pawn pawn) && pawn == member)
			{
				return;
			}
			if (tag != null)
			{
				if (ev.args.TryGetArg(HistoryEventArgsNamesCustom.Tag, out string tags))
				{
					if (IdeoUtility.ContainAll(tags, tag.Replace(" ", "").Split(',')) ^ exclusive) base.Notify_MemberWitnessedAction(ev, precept, member);
				}
				else if (exclusive)
				{
					base.Notify_MemberWitnessedAction(ev, precept, member);
				}
			}
			else
			{
				base.Notify_MemberWitnessedAction(ev, precept, member);
			}
		}
	}
}
