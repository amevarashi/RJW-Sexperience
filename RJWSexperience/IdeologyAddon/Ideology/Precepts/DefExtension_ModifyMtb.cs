using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.Precepts
{
	public abstract class DefExtension_ModifyMtb : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float multiplier = 1f;
	}
}
