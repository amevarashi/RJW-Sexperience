namespace RJWSexperience.SexHistory.UI
{
	public enum PartnerOrderMode
	{
		Normal = 0,
		Recent = 1,
		Most = 2,
		Name = 3
	};

	public static class PartnerOrderModeExtension
	{
		public static PartnerOrderMode Next(this PartnerOrderMode mode)
		{
			return (PartnerOrderMode)(((int)mode + 1) % 4);
		}
	}
}
