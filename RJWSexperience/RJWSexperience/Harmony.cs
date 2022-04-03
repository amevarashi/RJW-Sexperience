using HarmonyLib;
using System.Reflection;
using Verse;

namespace RJWSexperience
{
	[StaticConstructorOnStartup]
	internal static class First
	{
		static First()
		{
			var har = new Harmony("RJW_Sexperience");
			har.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
