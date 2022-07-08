using HarmonyLib;
using RJWSexperience.Ideology.Patches;
using System.Reflection;
using Verse;

namespace RJWSexperience.Ideology
{
	[StaticConstructorOnStartup]
	internal static class First
	{
		static First()
		{
			var harmony = new Harmony("RJW_Sexperience.Ideology");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			if (ModLister.HasActiveModWithName("RJW Sexperience"))
			{
				harmony.Patch(AccessTools.Method("RJWSexperience.RJWUtility:ThrowVirginHistoryEvent"),
					prefix: null,
					postfix: new HarmonyMethod(typeof(Sexperience_Patch_ThrowVirginHistoryEvent), nameof(Sexperience_Patch_ThrowVirginHistoryEvent.Postfix))
					);
			}
		}
	}
}
