using HarmonyLib;
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
				//Log.Message("[RJWSexperience.Ideology] Found RJWSexperience, patching");
				harmony.Patch(AccessTools.Method(typeof(ExtensionMethods.PawnExtensions), nameof(ExtensionMethods.PawnExtensions.IsIncest)),
					prefix: new HarmonyMethod(typeof(Sexperience_Patch_IsIncest), nameof(Sexperience_Patch_IsIncest.Prefix)),
					postfix: null
					);
				harmony.Patch(AccessTools.Method(typeof(RJWSexperience.RJWUtility), nameof(RJWSexperience.RJWUtility.ThrowVirginHIstoryEvent)),
					prefix: null,
					postfix: new HarmonyMethod(typeof(Sexperience_Patch_ThrowVirginHIstoryEvent), nameof(Sexperience_Patch_ThrowVirginHIstoryEvent.Postfix))
					);
			}
		}
	}
}
