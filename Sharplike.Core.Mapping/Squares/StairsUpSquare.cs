using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Rendering;
using Sharplike.Mapping.Entities;

namespace Sharplike.Mapping.Squares
{
	[Serializable]
	public class StairsUpSquare : AbstractSquare
	{
		public static Glyph FloorGlyph = new Glyph(0x18, Color.Gray);

		public StairsUpSquare()
		{
		}

		public override bool Teleport(Direction enterFromDirection, AbstractEntity ent)
		{
			if (enterFromDirection != Direction.Up)
			{
				ent.Location = ent.Location + Vector3.Up;
				return true;
			}
			return false;
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
