using RimWorld;
using rjw;
using Verse;

namespace RJWSexperience.Ideology
{
    public class ThoughtWorker_Precept_GenitalSize_Disapproved_Social : ThoughtWorker_Precept_Social
    {
            // Important Note: For the Social Worker, we measure otherPawns genitalia 
            protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
            {
                // We have 3 stages, which map directly to genitalia severity:
                // Unfavorable(<0.4), Normal(>0.4&&<0.6), Favorable(>0.6)
                if (otherPawn != null && Genital_Helper.get_AllPartsHediffList(otherPawn).Count > 0)
                {
                    float best_size = IdeoUtility.getGenitalSize(otherPawn);
                    if (best_size < 0.4f)
                        return ThoughtState.ActiveAtStage(0);
                    else if (best_size < 0.6f)
                        return ThoughtState.ActiveAtStage(1);
                    else if (best_size > 0.6f)
                        return ThoughtState.ActiveAtStage(2);
                }
                // This might can happen if the pawn has no genitalia ... maybe?
                return ThoughtState.Inactive;
            }
        }
    }
