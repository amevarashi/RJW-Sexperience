using UnityEngine;
using Verse;

namespace RJWSexperience.SexHistory.UI
{
	public class BarInfo
	{
		private string _label;
		private float _fillPercent;
		private string _labelRight;

		public string Label
		{
			get => _label;
			set => _label = value.CapitalizeFirst();
		}
		public float FillPercent
		{
			get => _fillPercent;
			set => _fillPercent = Mathf.Clamp01(value);
		}
		public Texture2D FillTexture { get; set; }
		public TipSignal Tooltip { get; set; }
		public string LabelRight
		{
			get => _labelRight;
			set => _labelRight = value.CapitalizeFirst();
		}
		public Texture2D Border { get; set; }

		public BarInfo()
		{
			_label = "";
			_fillPercent = 0f;
			FillTexture = Texture2D.grayTexture;
			Tooltip = default;
			_labelRight = "";
			Border = null;
		}

		public BarInfo(Texture2D fillTexture)
		{
			_label = "";
			_fillPercent = 0f;
			FillTexture = fillTexture;
			Tooltip = default;
			_labelRight = "";
			Border = null;
		}

		public BarInfo(string label, float fillPercent, Texture2D fillTexture, TipSignal tooltip, string labelRight = "", Texture2D border = null)
		{
			_label = label.CapitalizeFirst();
			_fillPercent = Mathf.Clamp01(fillPercent);
			FillTexture = fillTexture;
			Tooltip = tooltip;
			_labelRight = labelRight.CapitalizeFirst();
			Border = border;
		}

		public BarInfo(string label, float fillPercent, Texture2D fillTexture, string labelRight = "")
		{
			_label = label.CapitalizeFirst();
			_fillPercent = Mathf.Clamp01(fillPercent);
			FillTexture = fillTexture;
			Tooltip = default;
			_labelRight = labelRight.CapitalizeFirst();
			Border = null;
		}
	}
}
