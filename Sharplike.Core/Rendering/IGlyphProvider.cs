using System;
using System.Drawing;

namespace Sharplike.Core.Rendering
{
	public interface IGlyphProvider
	{
		/// <summary>
		/// Background color of the glyphs.
		/// </summary>
		Color BackgroundColor
		{
			get;
		}
		
		/// <summary>
		/// Stack of glyphs to draw.
		/// </summary>
		Glyph[] Glyphs
		{
			get;
		}
		
		/// <summary>
		/// Dirty flag?
		/// </summary>
		bool Dirty
		{
			get;
		}
	}
}
