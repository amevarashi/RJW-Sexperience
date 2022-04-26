using HarmonyLib;
using System.Reflection;
using Verse;
using rjw.Modules.Interactions.Internals.Implementation;
using rjw.Modules.Interactions.Rules.PartKindUsageRules;
using System.Collections.Generic;
using rjw;
using RJWSexperience.Logs;

namespace RJWSexperience
{
	[StaticConstructorOnStartup]
	internal static class First
	{
		static First()
		{
			var har = new Harmony("RJW_Sexperience");
			har.PatchAll(Assembly.GetExecutingAssembly());

			InjectIntoRjwInteractionServices();
		}

		public static void InjectIntoRjwInteractionServices()
		{
			var log = LogManager.GetLogger("StaticConstructorOnStartup");

			List<IPartPreferenceRule> partKindUsageRules = Unprivater.GetProtectedValue<List<IPartPreferenceRule>>("_partKindUsageRules", typeof(PartPreferenceDetectorService));
			partKindUsageRules.Add(new Interactions.CumAddictPartKindUsageRule());
			log.Message("Added 1 rule to PartPreferenceDetectorService._partKindUsageRules");
		}
	}
}
