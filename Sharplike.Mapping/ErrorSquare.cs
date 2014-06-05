using System;
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.Mapping
{
	public class ErrorSquare : AbstractSquare
	{

		public override Boolean IsPassable(Direction d) { return false; }
		public override Color BackgroundColor { get { return Color.Black; } }

		private Glyph[] glyphs;
		public override Glyph[] Glyphs
		{
			get
			{
				return glyphs;
			}
		}
		
		public ErrorSquare (Int32 g)
		{
			glyphs = new Glyph[1];
			glyphs[0] = new Glyph(g, Color.Red);
		}
	}
}
