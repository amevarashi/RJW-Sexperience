using RimWorld;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience
{
	public class ThoughtDefExtension_IncreaseRecord : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public RecordDef recordDef;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float increment;
	}
}
