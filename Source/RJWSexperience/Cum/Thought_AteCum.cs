namespace RJWSexperience // Change in namespace will lead to save incompatibility
{
	public class Thought_AteCum : Thought_Recordbased
	{
		protected override void UpdateCurStage()
		{
			if (pawn?.health?.hediffSet?.HasHediff(VariousDefOf.CumAddiction) ?? false)
			{
				SetForcedStage(def.stages.Count - 1);
			}
			else
			{
				base.UpdateCurStage();
			}
		}
	}
}
