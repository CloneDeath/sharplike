using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sharplike.Core.Rendering;

namespace Sharplike.OpenTK.Rendering
{
	public class TKRenderSystem : AbstractRenderSystem
	{
		private TKWindow _window = null;
		public override AbstractWindow Window
		{
			get { return _window; }
		}

		/// <summary>
		/// Creates a new Window, or returns the existing one (ignoring input parameters if one already exists).
		/// </summary>
		/// <param name="displayDimensions">Size of the window.</param>
		/// <param name="palette">Glyph settings to use.</param>
		/// <param name="context">Control requesting window?</param>
		/// <returns>A new window the first time it is called, or the existing window on subsequent calls.</returns>
		public override AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context)
		{
			if (_window == null) {
				_window = new TKWindow(displayDimensions, palette, context as Control);
				_window.FocusWindow();
			}
			return _window;
		}
	}
}
