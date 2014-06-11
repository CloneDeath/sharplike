using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Sharplike.Core.Rendering
{
	public abstract class AbstractRenderSystem : IDisposable
	{
		public abstract AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context);

		public AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette)
		{
			return CreateWindow(displayDimensions, palette, null);
		}

		/// <summary>
		/// Primary window for the renderer.
		/// </summary>
		public abstract AbstractWindow Window
		{
			get;
		}
		public abstract void Process();

		public abstract void Dispose();
	}
}
