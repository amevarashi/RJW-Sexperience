using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public readonly struct BarInfo
	{
		public readonly string label;
		public readonly float fillPercent;
		public readonly Texture2D fillTexture;
		public readonly TipSignal tooltip;
		public readonly string labelRight;
		public readonly Texture2D border;

		public BarInfo(string label, float fillPercent, Texture2D fillTexture, TipSignal tooltip, string labelRight = "", Texture2D border = null)
		{
			this.label = label.CapitalizeFirst();
			this.fillPercent = Mathf.Clamp01(fillPercent);
			this.fillTexture = fillTexture;
			this.tooltip = tooltip;
			this.labelRight = labelRight.CapitalizeFirst();
			this.border = border;
		}

		public BarInfo(string label, float fillPercent, Texture2D fillTexture, string labelRight = "")
		{
			this.label = label.CapitalizeFirst();
			this.fillPercent = Mathf.Clamp01(fillPercent);
			this.fillTexture = fillTexture;
			this.tooltip = default;
			this.labelRight = labelRight.CapitalizeFirst();
			this.border = null;
		}
	}
}
