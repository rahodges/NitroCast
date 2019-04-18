using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Summary description for ProgressBarConfig.
	/// </summary>
	public class ProgressBarConfig
	{
		int __minimum;
		int __maximum;
		int __value;
		int __step;

		public ProgressBarConfig(int minimum, int maximum, int value, int step)
		{
			__minimum = minimum;
			__maximum = maximum;
			__value = value;
			__step = step;
		}

		public void SetProgressBar(System.Windows.Forms.ProgressBar progressBar)
		{
			progressBar.Minimum = __minimum;
			progressBar.Maximum = __maximum;
			progressBar.Value = __value;
			progressBar.Step = __step;
		}
	}
}
