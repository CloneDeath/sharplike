using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Rendering;

namespace Sharplike.Mapping.Squares
{
	[Serializable]
	public class FloorSquare : AbstractSquare
	{
		public static Glyph FloorGlyph = new Glyph(0xFA, Color.Gray);

		public FloorSquare()
		{
		}

		public override bool IsPassable(Direction fromDirection)
		{
			return true;
		}

		public override Glyph[] Glyphs
		{
			get
			{
				return new Glyph[] { FloorGlyph };
			}
		}

		public override bool Dirty
		{
			get
			{
				return false;
			}
		}
	}
}
