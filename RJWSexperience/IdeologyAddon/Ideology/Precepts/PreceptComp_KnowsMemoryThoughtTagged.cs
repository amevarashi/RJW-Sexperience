using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.Precepts
{
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
