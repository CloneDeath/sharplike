using System;
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.Mapping
{
	[Serializable]
	public class EmptySquare : AbstractSquare
	{

		public new Color BackgroundColor { get { return Color.Black; } }

		private static Glyph[] glyphs = { };
		public override Glyph[] Glyphs
		{
			get
			{
				return EmptySquare.glyphs;
			}
		}
	}
}
