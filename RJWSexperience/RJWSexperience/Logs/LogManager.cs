using rjw.Modules.Shared.Logs;
using System;
using Verse;

namespace RJWSexperience.Logs
{
	// Copy of RJW code because of hardcoded mod name prefix. Maybe TODO: update RJW's version to accept prefix from LogProvider
	public static class LogManager
	{
		private class Logger : ILog
		{
			private readonly string _loggerTypeName;
			private readonly ILogProvider _logProvider;

			public Logger(string typeName, ILogProvider logProvider = null)
			{
				_loggerTypeName = typeName;
				_logProvider = logProvider;
			}

			public void Debug(string message)
			{
				LogDebug(CreateLogMessage(message));
			}
			public void Debug(string message, Exception e)
			{
				LogDebug(CreateLogMessage(message, e));
			}

			public void Message(string message)
			{
				LogMessage(CreateLogMessage(message));
			}
			public void Message(string message, Exception e)
			{
				LogMessage(CreateLogMessage(message, e));
			}

			public void Warning(string message)
			{
				LogWarning(CreateLogMessage(message));
			}
			public void Warning(string message, Exception e)
			{
				LogWarning(CreateLogMessage(message, e));
			}

			public void Error(string message)
			{
				LogError(CreateLogMessage(message));
			}
			public void Error(string message, Exception e)
			{
				LogError(CreateLogMessage(message, e));
			}

			private string CreateLogMessage(string message)
			{
				return $"[Sexperience] [{_loggerTypeName}] {message}";
			}
			private string CreateLogMessage(string message, Exception exception)
			{
				return $"{CreateLogMessage(message)}{Environment.NewLine}{exception}";
			}

			private void LogDebug(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					Log.Message(message);
				}
			}
			private void LogMessage(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					Log.Message(message);
				}
			}
			private void LogWarning(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					Log.Warning(message);
				}
			}
			private void LogError(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					Log.Error(message);
				}
			}
		}

		public static ILog GetLogger<TType, TLogProvider>()
			where TLogProvider : ILogProvider, new()
		{
			return new Logger(typeof(TType).Name, new TLogProvider());
		}
		public static ILog GetLogger<TLogProvider>(string staticTypeName)
			where TLogProvider : ILogProvider, new()
		{
			return new Logger(staticTypeName, new TLogProvider());
		}
		public static ILog GetLogger(string staticTypeName)
		{
			return new Logger(staticTypeName);
		}
	}
}
