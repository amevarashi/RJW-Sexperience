using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience.Ideology
{
	public class PreceptDef_RequirementExtended : PreceptDef
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<MemeDef> requiredAllMemes = new List<MemeDef>();
	}
}
