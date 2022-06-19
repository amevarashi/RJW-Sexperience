using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Verse;

namespace RJWSexperience.Ideology.Precepts
{
	public class DefExtension_PreferSextype : DefModExtension
	{
		[SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "Field value loaded from XML")]
		public List<string> sextypes = new List<string>();
		private HashSet<string> sextypesHashSet;

		public bool HasSextype(string sextype)
		{
			if (sextypesHashSet == null)
				sextypesHashSet = new HashSet<string>(sextypes);

			return sextypesHashSet.Contains(sextype);
		}
	}
}
