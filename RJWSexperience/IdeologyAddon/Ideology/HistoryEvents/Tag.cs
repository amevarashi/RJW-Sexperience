using Verse;

namespace RJWSexperience.Ideology.HistoryEvents
{
	public static class Tag
	{
		public const string BeenRaped = "[BeenRaped]";
		public const string Rape = "[Rape]";
		public const string NotSpouse = "[NotSpouse]";

		public static string Gender(Pawn pawn) => "[" + pawn.gender + "]";
	}
}
