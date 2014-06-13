using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using Sharplike.Core;
using Sharplike.Core.Input;
using Sharplike.Frontend;
using Sharplike.Frontend.Rendering;

namespace Sharplike.Frontend.Input
{
	public class TKInputProvider : AbstractInputProvider
	{
		TKWindow win;

		public TKInputProvider()
		{
			TKRenderSystem rsys = (TKRenderSystem)Game.RenderSystem;
			win = (TKWindow)rsys.Window;

			

			win.Control.KeyDown += new KeyEventHandler(Control_KeyDown);
			win.Control.KeyUp += new KeyEventHandler(Control_KeyUp);
			win.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
			win.Control.MouseUp += new MouseEventHandler(Control_MouseUp);
			win.Control.MouseWheel += new MouseEventHandler(Control_MouseWheel);
			win.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
		}

		void Control_KeyDown(object sender, KeyEventArgs e)
		{
			this.KeyPressed(e);
		}

		void Control_KeyUp(object sender, KeyEventArgs e)
		{
			this.KeyReleased(e);
		}

		void Control_MouseDown(object sender, MouseEventArgs e)
		{
			this.MousePressed(e);
		}

		void Control_MouseUp(object sender, MouseEventArgs e)
		{
			this.MouseReleased(e);
		}

		void Control_MouseWheel(object sender, MouseEventArgs e)
		{
			this.MouseWheel(e);
		}

		void Control_MouseMove(object sender, MouseEventArgs e)
		{
			this.MouseMove(e);
		}
	}
}
