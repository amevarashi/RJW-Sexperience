using RimWorld;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RJWSexperience
{
	/// <summary>
	/// ThoughtDef using record
	/// </summary>
	public class ThoughtDef_Recordbased : ThoughtDef
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public RecordDef recordDef;
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<float> minimumValueforStage = new List<float>();
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public float increment;
	}
}
