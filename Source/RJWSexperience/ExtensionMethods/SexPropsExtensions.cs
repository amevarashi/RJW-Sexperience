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
			if (props.partner != null)
			{
				return props.pawn.IsAnimal() ^ props.partner.IsAnimal();
			}
			return false;
		}
	}
}
