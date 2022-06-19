using Verse;

namespace RJWSexperience.Ideology
{
	public static class HETag
	{
		public const string Incestous = "[Incestuos]";
		public const string BeenRaped = "[BeenRaped]";
		public const string Rape = "[Rape]";
		public const string Spouse = "[Spouse]";
		public const string NotSpouse = "[NotSpouse]";

		public static string Gender(Pawn pawn) => "[" + pawn.gender + "]";
	}
}
