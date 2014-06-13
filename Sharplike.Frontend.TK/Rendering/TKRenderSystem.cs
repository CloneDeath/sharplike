using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sharplike.Core.Rendering;

namespace Sharplike.Frontend.Rendering
{
	public class TKRenderSystem : AbstractRenderSystem
	{
		private TKWindow _window = null;
		public override AbstractWindow Window
		{
			get { return _window; }
		}

		public override AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context)
		{
			if (_window == null)
				_window = new TKWindow(displayDimensions, palette, context as Control);
			return _window;
		}
	}
}
