using rjw;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class TwoPawnFilter
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public SinglePawnFilter doer;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public SinglePawnFilter partner;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public RelationFilter relations;

		public bool Applies(Pawn pawn, Pawn partner)
		{
			if (doer?.Applies(pawn) == false)
				return false;

			if (this.partner?.Applies(pawn) == false)
				return false;

			if (relations?.Applies(pawn, partner) == false)
				return false;

			return true;
		}
	}
}
