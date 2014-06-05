using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Sharplike.Frontend.Rendering;

namespace Sharplike.Frontend.Rendering
{
	public delegate void TKDisplayFunc();
	public delegate void TKResizeFunc(int Width, int Height);

	/// <summary>
	/// The form on which TKWindow shall render.
	/// </summary>
	public class TKForm : Form
	{
		public TKForm()
		{
			// W01_First_Window
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.KeyPreview = true;
			this.Name = "W01_First_Window";
			this.ResumeLayout(false);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.ResumeLayout(false);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TKForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "TKForm";
			this.ResumeLayout(false);

		}


	}
}
