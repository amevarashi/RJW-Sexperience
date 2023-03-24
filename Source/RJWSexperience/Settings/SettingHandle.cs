using Verse;

namespace RJWSexperience.Settings
{
	public class SettingHandle<T> : ISettingHandle
	{
		public T Value { get; set; }
		public readonly string XmlLabel;
		public readonly T DefaultValue;

		public SettingHandle(string xmlLabel, T defaultValue)
		{
			XmlLabel = xmlLabel;
			DefaultValue = defaultValue;
			Value = defaultValue;
		}

		public void Reset()
		{
			Value = DefaultValue;
		}

		public void Scribe()
		{
			T value = Value;
			Scribe_Values.Look(ref value, XmlLabel, DefaultValue);
			Value = value;
		}

		public static implicit operator T(SettingHandle<T> settingHandle)
		{
			return settingHandle.Value;
		}
	}
}
