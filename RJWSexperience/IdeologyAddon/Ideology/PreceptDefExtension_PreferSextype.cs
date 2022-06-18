using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology
{
	public class PreceptDefExtension_PreferSextype : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public HashSet<string> sextypes = new HashSet<string>();
	}
}
