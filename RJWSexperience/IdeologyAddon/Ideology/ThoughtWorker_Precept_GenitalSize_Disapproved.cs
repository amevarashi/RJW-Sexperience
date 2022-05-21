using RimWorld;
using rjw;
using Verse;

namespace RJWSexperience.Ideology
{
    public class ThoughtWorker_Precept_GenitalSize_Disapproved : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            // We have 5 stages, which map directly to genitalia severity:
            // Micro(<0.2), Small(>0.2&&<0.4), Normal(>0.4&&<0.6), Big(>0.6&&<0.8), Huge(>0.8)
            if (p != null && Genital_Helper.get_AllPartsHediffList(p).Count > 0)
            {
                float best_size = IdeoUtility.getGenitalSize(p);
                if (best_size < 0.2f)
                    return ThoughtState.ActiveAtStage(0);
                else if (best_size < 0.4f)
                    return ThoughtState.ActiveAtStage(1);
                else if (best_size < 0.6f)
                    return ThoughtState.ActiveAtStage(2);
                else if (best_size < 0.8f)
                    return ThoughtState.ActiveAtStage(3);
                else if (best_size > 0.8f)
                    return ThoughtState.ActiveAtStage(4);
            }
            // This might can happen if the pawn has no genitalia ... maybe?
            return ThoughtState.Inactive;
        }

    }
}
