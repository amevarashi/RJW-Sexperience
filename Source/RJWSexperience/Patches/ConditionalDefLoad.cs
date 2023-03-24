/*using HarmonyLib;
using System.Xml;
using Verse;

namespace RJWSexperience.Patches
{
	public static class ConditionalDefLoad
	{
		public static void DoPatch()
		{
			Harmony harmony = new Harmony("RJW_SexperienceXmlLoad");
			harmony.Patch(AccessTools.Method(typeof(DirectXmlLoader), nameof(DirectXmlLoader.DefFromNode)), new HarmonyMethod(typeof(ConditionalDefLoad), nameof(ConditionalDefLoad.Prefix)));
		}

		public static bool Prefix(XmlNode node, LoadableXmlAsset loadingAsset, ref Def __result)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return true;
			}

			var settingName = node.Attributes?["RsLoadFlag"]?.Value;

			if (settingName.NullOrEmpty())
			{
				return true;
			}

			if (SexperienceMod.Settings.GetValue<bool>(settingName))
			{
				__result = null;
				return false;
			}
			return true;
		}
	}
}*/
