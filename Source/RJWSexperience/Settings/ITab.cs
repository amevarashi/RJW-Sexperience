using UnityEngine;

namespace RJWSexperience.Settings
{
	public interface ITab
	{
		string Label { get; }
		void DoTabContents(Rect inRect);
	}
}