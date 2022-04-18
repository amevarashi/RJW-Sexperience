using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience
{
	/// <summary>
	/// ThoughtDef using opinion
	/// </summary>
	public class ThoughtDef_Opinionbased : ThoughtDef
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<float> minimumValueforStage = new List<float>();
	}
}
