using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.Precepts
{
	public class Comp_SelfTookMemoryThought_Gendered : PreceptComp_SelfTookMemoryThought
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public Gender gender;

		public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			Pawn doer = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			if (doer.gender == gender)
				TakeThought(ev, precept, canApplySelfTookThoughts, doer);
		}

		/// <summary>
		/// This is a copy of base.Notify_MemberTookAction, but with partner handling
		/// </summary>
		protected void TakeThought(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts, Pawn doer)
		{
			if (ev.def != eventDef || !canApplySelfTookThoughts)
			{
				return;
			}
			Pawn partner = ev.args.GetArg<Pawn>(HistoryEvents.ArgsNamesCustom.Partner);
			if (doer.needs?.mood != null && (!onlyForNonSlaves || !doer.IsSlave))
			{
				if (thought.minExpectationForNegativeThought != null && ExpectationsUtility.CurrentExpectationFor(doer).order < thought.minExpectationForNegativeThought.order)
				{
					return;
				}
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(thought, precept);
				if (thought_Memory is Thought_KilledInnocentAnimal thought_KilledInnocentAnimal && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out Pawn animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				if (thought_Memory is Thought_MemoryObservation thought_MemoryObservation && ev.args.TryGetArg<Corpse>(HistoryEventArgsNames.Subject, out Corpse target))
				{
					thought_MemoryObservation.Target = target;
				}
				doer.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, partner);
			}
		}
	}
}
