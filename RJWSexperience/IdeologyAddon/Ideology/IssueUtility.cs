using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RJWSexperience.Ideology
{
	public static class IssueUtility
	{
		private static readonly Dictionary<IssueDef, List<PreceptDef>> issuePrecepts = new Dictionary<IssueDef, List<PreceptDef>>();

		public static List<PreceptDef> GetAllPrecepts(this IssueDef issue)
		{
			if (issuePrecepts.TryGetValue(issue, out List<PreceptDef> precepts))
				return precepts;

			precepts = DefDatabase<PreceptDef>.AllDefsListForReading.FindAll(x => x.issue == issue);
			issuePrecepts.Add(issue, precepts);
			return precepts;
		}

		public static Precept GetPreceptOfIssue(this Ideo ideo, IssueDef issue)
		{
			foreach (PreceptDef preceptDef in issue.GetAllPrecepts())
			{
				Precept precept = ideo.GetPrecept(preceptDef);
				if (precept != null)
					return precept;
			}

			return null;
		}
	}
}
