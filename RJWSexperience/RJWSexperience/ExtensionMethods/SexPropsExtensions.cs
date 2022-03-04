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

		public static Pawn GetInteractionRecipient(this SexProps props)
		{
			if (props.isReceiver)
			{
				return props.pawn;
			}
			else
			{
				return props.partner;
			}
		}
	}
}
