using RimWorld;
using rjw;
using Verse;

namespace RJWSexperience.Ideology
{
    // This Thoughtworker Checks for Bukkake-Hediff and adds a approving thought if the gender and ideology are fullfilled.
    // The thought gets removed when the Hediff is removed.
    public class ThoughtWorker_Precept_GenitalSize_Approved : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            // We have 5 stages, which map directly to genitalia severity:
            // Micro(<0.2), Small(>0.2&&<0.4), Normal(>0.4&&<0.6), Big(>0.6&&<0.8), Huge(>0.8)
            if (p != null && Genital_Helper.get_AllPartsHediffList(p).Count > 0)
            {
                float best_size = getGenitalSize(p);
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

        private float getGenitalSize(Pawn p)
        {
            if (p == null)
                return 0f;

            // Iff the pawn has multiple genitalia, the "best" is picked (the biggest penis or tightest vagina)
            float best_seen_size = 0f;
            foreach (Hediff part in rjw.Genital_Helper.get_AllPartsHediffList(p))
            {
                // Only check for Vaginas and Penises, not for Anus or for things not categorized as primary sexual parts
                if (Genital_Helper.is_penis(part) || Genital_Helper.is_vagina(part))
                {
                    best_seen_size = part.Severity > best_seen_size ? part.Severity : best_seen_size;
                }
            }


            // For Women, the scale is inversed.
            if (p.gender == Gender.Female)
                return 1 - best_seen_size;

            return best_seen_size;
        }
    }
}
