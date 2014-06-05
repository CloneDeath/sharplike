using System;
using System.Drawing;
using Sharplike.Core.Rendering;
using Sharplike.Mapping;

namespace Sharplike.Tests.Sandbox
{


	[Serializable]
	public class NormalWallSquare : AbstractSquare
	{
		public override Boolean IsPassable(Direction d) { return false; }
		public override Color BackgroundColor { get { return Color.Black; } }

		private static Glyph[] glyphs = { new Glyph(178, Color.Yellow) };
		public override Glyph[] Glyphs
		{
			get
			{
				return NormalWallSquare.glyphs;
			}
		}
	}
}
