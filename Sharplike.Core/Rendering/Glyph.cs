using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Sharplike.Core.Rendering
{
	/// <summary>
	/// Representation of a single glyph. (A tile may have an arbitrary number of Glyphs.)
	/// </summary>
	[Serializable]
	public struct Glyph
	{
		/// <summary>
		/// Set by GlyphPalette (yes, this means only one GlyphPalette per application, for now.)
		/// </summary>
		internal static Int32 GlyphCount = 0;

		/// <summary>
		/// The color of the glyph to render. Uses color tinting by default (OpenGL implementation),
		/// so grayscale will be multiplied and properly tinted (darker versions of specified color).
		/// </summary>
		public readonly Color Color;
		/// <summary>
		/// The GlyphPalette index of the glyph to display. Users are recommended to build their own
		/// glyph index mappings.
		/// </summary>
		public readonly Int32 Index;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="glyphIndex">The glyph index in the application's glyph palette.</param>
		/// <param name="glyphColor">The glyph color (32-bit RGBA).</param>
		public Glyph(Int32 glyphIndex, Color glyphColor)
		{
			if (glyphIndex >= GlyphCount)
				throw new ArgumentOutOfRangeException(glyphIndex.ToString() + " does not exist; " +
					"glyph palette's last index is " + (GlyphCount - 1).ToString() + ".");

			Color = glyphColor;
			Index = glyphIndex;
		}
	}
}
