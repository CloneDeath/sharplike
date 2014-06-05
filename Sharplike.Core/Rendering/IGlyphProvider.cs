using System;
using System.Drawing;

namespace Sharplike.Core.Rendering
{
	public interface IGlyphProvider
	{
		Color BackgroundColor
		{
			get;
		}
		
		Glyph[] Glyphs
		{
			get;
		}
		
		bool Dirty
		{
			get;
		}
	}
}
