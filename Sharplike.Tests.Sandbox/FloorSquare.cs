using System;
using System.Drawing;
using Sharplike.Core.Rendering;
using Sharplike.Mapping;

namespace Sharplike.Tests.Sandbox
{


	[Serializable]
    public class FloorSquare : AbstractSquare
    {
        public override Color BackgroundColor { get { return Color.Black; } }

        private static Glyph[] glyphs = { new Glyph((int)'.', Color.Gray) };
        public override Glyph[] Glyphs
        {
            get
            {
                return FloorSquare.glyphs;
            }
        }
    }
}
