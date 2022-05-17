using RimWorld;
using Verse;
using rjw;

namespace RJWSexperience.Ideology.Ideology.Precept_Workers
{ 
    /// <summary>
    /// thought worker for a thought that is active when a certain hediff is present, and who's stage depends on the ether state of the pawn 
    /// Shamelessly taken from: https://github.com/Tachyonite/Pawnmorpher/blob/master/Source/Pawnmorphs/Esoteria/Thoughts/ThoughtWorker_EtherHediff.cs
    /// </summary>
    public class ThoughtWorker_Precept_Pregnant_Social : ThoughtWorker
    {
        /// <summary>Gets the current thought state of the given pawn.</summary>
        /// <param name="p">The pawn for whom the thoughts are generated.</param>
        /// <returns></returns>
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {

            var pregnancy = rjw.PregnancyHelper.GetPregnancy(p);

            if (pregnancy == null) //ignoring the hediff's stage, the thought's stage is dependent on only the ether state of the pawn c
            {
                return false;
            }

            return ThoughtState.ActiveAtStage(0);
        }
    }
}
