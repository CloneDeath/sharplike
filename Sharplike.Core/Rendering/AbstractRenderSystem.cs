using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Sharplike.Core.Rendering
{
	public abstract class AbstractRenderSystem : IDisposable
	{
		/// <summary>
		/// Primary window for the renderer.
		/// </summary>
		public abstract AbstractWindow Window
		{
			get;
		}

		public abstract AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context);

		public AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette)
		{
			return CreateWindow(displayDimensions, palette, null);
		}

		public virtual void Dispose()
		{
			if (Window != null)
				Window.Dispose();
		}

		public virtual void Process()
		{
			Window.Update();

			if (AbstractRegion.FocusControl != null) {
				foreach (Keys key in Game.InputSystem.Input.GetAllPressed()) {
					AbstractRegion.FocusControl.OnKeyPress(key);
				}
			}
		}
	}
}
