using Verse;
using rjw;

namespace RJWSexperience.ExtensionMethods
{
	public static class SexPropsExtensions
	{
		public static Pawn GetInteractionInitiator(this SexProps props)
		{
			if (props.isReceiver)
			{
				return props.partner;
			}
			else
			{
				return props.pawn;
			}
		}

		public static bool IsBestiality(this SexProps props)
		{
			if (props.hasPartner())
			{
				return props.pawn.IsAnimal() ^ props.partner.IsAnimal();
			}
			return false;
		}

		/// <summary>
		/// Get a not-so-unique ID. Same interaction between the same partners will return same ID
		/// </summary>
		public static int GetTempId(this SexProps props)
		{
			return props.pawn.GetHashCode() ^
				(props.partner?.GetHashCode() ?? 0) ^
				props.dictionaryKey.GetHashCode() ^
				(props.isReceiver ? 0 : 1);
		}
	}
}
