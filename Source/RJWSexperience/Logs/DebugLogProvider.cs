using rjw.Modules.Shared.Logs;

namespace RJWSexperience.Logs
{
	public class DebugLogProvider : ILogProvider
	{
		public bool IsActive => SexperienceMod.Settings.DevMode;
	}
}
