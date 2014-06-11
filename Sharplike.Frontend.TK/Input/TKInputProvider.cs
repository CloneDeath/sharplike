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

			win.Control.MouseMove += this.OnMouseMove;
			win.Control.KeyDown += this.OnKeyPressed;
			win.Control.KeyUp += this.OnKeyReleased;
			win.Control.MouseDown += this.OnMousePressed;
			win.Control.MouseUp += this.OnMouseReleased;
			win.Control.MouseWheel += this.OnMouseWheel;
		}

		public override void Dispose()
		{

		}
	}
}
