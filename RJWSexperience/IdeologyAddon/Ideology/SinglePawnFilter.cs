using rjw;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class SinglePawnFilter
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isAnimal;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isSlave;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public bool? isPrisoner;

		public bool Applies(Pawn pawn)
		{
			if (isAnimal != null && isAnimal != pawn.IsAnimal())
				return false;

			if (isSlave != null && isSlave != pawn.IsSlave)
				return false;

			if (isPrisoner != null && isPrisoner != pawn.IsPrisoner)
				return false;

			return true;
		}
	}
}
