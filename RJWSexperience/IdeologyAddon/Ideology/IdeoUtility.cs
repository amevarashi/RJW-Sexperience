using RimWorld;
using Verse;

namespace RJWSexperience.Ideology
{
	public static class IdeoUtility
	{
		public static bool IsSubmissive(this Pawn pawn)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
				return false;

			if (ideo.HasPrecept(VariousDefOf.Submissive_Female) && pawn.gender == Gender.Female)
				return true;
			else if (ideo.HasPrecept(VariousDefOf.Submissive_Male) && pawn.gender == Gender.Male)
				return true;

			return false;
		}

		public static bool ContainAll(string str, string[] tags)
		{
			if (tags.NullOrEmpty())
				return true;

			string lstr = str.ToLower();
			for (int i = 0; i < tags.Length; i++)
			{
				if (!lstr.Contains('[' + tags[i].ToLower() + ']'))
					return false;
			}
			return true;
		}

		public static float GetPreceptsMtbMultiplier<T>(Ideo ideo) where T : Precepts.DefExtension_ModifyMtb
		{
			float finalMultiplier = 1f;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
			{
				T defExtension = ideo.PreceptsListForReading[i].def.GetModExtension<T>();

				if (defExtension == null)
					continue;

				finalMultiplier *= defExtension.multiplier;
			}
			return finalMultiplier;
		}
	}
}
