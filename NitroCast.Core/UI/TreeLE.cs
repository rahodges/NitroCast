using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace NitroCast.Core.UI 
{
	public class TreeLE : TreeView 
    {
        const int WM_VSCROLL = 0x0115;
        const int WM_HSCROLL = 0x0114;
        const int SB_THUMBPOSITION = 4;

        const int SB_HORZ = 0;
        const int SB_VERT = 1;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetScrollPos(int hWnd, int nBar);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar,
            int nPos, bool bRedraw);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool PostMessageA(IntPtr hWnd, int wMsg,
            int wParam, int lParam);

        public int HScrollPos
        {
            get
            {
                return GetScrollPos((int)Handle, SB_HORZ);
            }
            set
            {
                SetScrollPos(this.Handle, SB_HORZ, value, true);
                PostMessageA(this.Handle, WM_HSCROLL,
                    SB_THUMBPOSITION + 0x10000 * value, 0);
            }
        }

        public int VScrollPos
        {
            get
            {
                return GetScrollPos((int)Handle, SB_VERT);
            }
            set
            {
                SetScrollPos(this.Handle, SB_VERT, value, true);
                PostMessageA(this.Handle, WM_HSCROLL,
                    SB_THUMBPOSITION + 0x10000 * value, 0);
            }
        }

		#region Component Designer generated code

		private System.ComponentModel.Container components = null;

		private void InitializeComponent() {
			this.HideSelection = false;
			this.LabelEdit = false;
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		public TreeLE() {
			InitializeComponent();
			this.SetStyle(ControlStyles.EnableNotifyMessage, true);
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				TreeNode tn = this.GetNodeAt(e.X, e.Y);
				if(tn!=null)
					this.SelectedNode = tn;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			TreeNode tn;
			if(e.Button==MouseButtons.Left) {
				tn = this.SelectedNode;
				if(tn==this.GetNodeAt(e.X, e.Y)) {
					if(wasDoubleClick)
						wasDoubleClick = false;
					else {
						TriggerLabelEdit = true;
					}
				}
			}
			base.OnMouseUp(e);
		}

		private const int WM_TIMER = 0x0113;
		private bool TriggerLabelEdit = false;
		private string viewedLabel;
		private string editedLabel;

		protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e) {
			// put node label to initial state
			// to ensure that in case of label editing cancelled
			// the initial state of label is preserved
			this.SelectedNode.Text = viewedLabel;
			// base.OnBeforeLabelEdit is not called here
			// it is called only from StartLabelEdit
		}

		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e) 
		{
			this.LabelEdit = false;
			e.CancelEdit = true;
			if(e.Label==null)
			{
				return;
			}
			ValidateLabelEditEventArgs ea = new ValidateLabelEditEventArgs(e.Label);
			OnValidateLabelEdit(ea);
			if(ea.Cancel==true) 
			{
				e.Node.Text = editedLabel;
				this.LabelEdit = true;
				e.Node.BeginEdit(); 
			}
			else
				base.OnAfterLabelEdit(e);
		}

		public void BeginEdit()
		{
			StartLabelEdit();
		}

		protected override void OnNotifyMessage(Message m) 
		{
			if(TriggerLabelEdit)
			{
				if(m.Msg==WM_TIMER) 
				{
					TriggerLabelEdit = false;
					StartLabelEdit();
				}
			}
			base.OnNotifyMessage(m);
		}

		public void StartLabelEdit() {
			TreeNode tn = this.SelectedNode;
			viewedLabel = tn.Text;

			NodeLabelEditEventArgs e = new NodeLabelEditEventArgs(tn);
			base.OnBeforeLabelEdit(e);

			editedLabel = tn.Text;

			this.LabelEdit = true;
			tn.BeginEdit();
		}


		protected override void OnClick(EventArgs e) 
		{
			TriggerLabelEdit = false;
			base.OnClick(e);
		}

		private bool wasDoubleClick = false;
		protected override void OnDoubleClick(EventArgs e) 
		{
			wasDoubleClick = true;
			base.OnDoubleClick(e);
		}

		public event ValidateLabelEditEventHandler ValidateLabelEdit;

		protected virtual void OnValidateLabelEdit(ValidateLabelEditEventArgs e) 
		{
			if(ValidateLabelEdit != null)
			{
				ValidateLabelEdit(this, e);
			}
		}
		
		public delegate void ValidateLabelEditEventHandler(object sender, ValidateLabelEditEventArgs e);

	}

	public class ValidateLabelEditEventArgs : CancelEventArgs 
	{
			
		public ValidateLabelEditEventArgs(string label) 
		{
			this.label = label;
			this.Cancel = false;
		}

		private string label;
		public string Label 
		{
			get {return label;}
			set {label = value;}
		}

	}
}
