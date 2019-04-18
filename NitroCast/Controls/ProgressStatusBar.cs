using System;
using System.Windows.Forms;

namespace NitroCast.Controls
{
	/// <summary>
	/// Summary description for ProgressStatusBar.
	/// </summary>
	public class ProgressStatusBar : StatusBar
	{
		public ProgressBar ProgressBar;

        private int progressPanelIndex;

		public int ProgressPanelIndex
		{
			get 
			{
				return this.progressPanelIndex; 
			}
			set 
			{ 
				progressPanelIndex = value;
				this.Panels[progressPanelIndex].Style = StatusBarPanelStyle.OwnerDraw;
			}
		}

		public ProgressStatusBar()
		{
            ProgressBar = new ProgressBar();
            progressPanelIndex = -1;
			ProgressBar.Hide();
			this.Controls.Add(ProgressBar);
			this.SizingGrip = false;
			this.DrawItem += new System.Windows.Forms.StatusBarDrawItemEventHandler(this.ProgressStatus_DrawItem);
		}

		private void ProgressStatus_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent)
		{
			ProgressBar.Location = new System.Drawing.Point(sbdevent.Bounds.X, sbdevent.Bounds.Y);
			ProgressBar.Size = new System.Drawing.Size(sbdevent.Bounds.Width, sbdevent.Bounds.Height);
			ProgressBar.Show();
		}
	}
}
