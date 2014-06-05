using System;
using System.Drawing;

namespace Sharplike.Core.Rendering
{
	[Serializable]
	public class RawGlyphProvider : IGlyphProvider
	{
		private Glyph[] glyphs;
		private Color background = Color.Black;

		public RawGlyphProvider (Glyph g, Color c)
		{
			background = c;
			glyphs = new Glyph[1];
			glyphs[0] = g;
		}

        public Color BackgroundColor
        {
			get { return background; }
        }

        public Glyph[] Glyphs
        {
			get { return glyphs; }
        }

        public bool Dirty
        {
            get { return false; }
        }
    }
}
