using rjw;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class HistoryEventDefExtension_AssociatedSextypes : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<xxx.rjwSextype> sextypes = new List<xxx.rjwSextype>();
	}
}
