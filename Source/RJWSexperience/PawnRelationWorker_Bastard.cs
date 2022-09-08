using RimWorld;
using Verse;

namespace RJWSexperience
{
	public class PawnRelationWorker_Bastard : PawnRelationWorker_Child
	{
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (!SexperienceMod.Settings.EnableBastardRelation)
				return false;

			Pawn mother = other.GetMother();
			Pawn father = other.GetFather();
			if (me != other && (mother == me || father == me))
			{
				if (mother == null || father == null) return true;
				else if (mother.relations != null) return !(mother.relations.DirectRelationExists(PawnRelationDefOf.Spouse, father) || mother.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, father));
			}

			return false;
		}
	}
}
