using System;
using System.Drawing;
using System.Collections.Generic;

namespace Sharplike.Core.Rendering
{
	/// <summary>
	/// Bare-bones glyph provider. Offers easy access to adding and removing glyphs.
	/// </summary>
	[Serializable]
	public class RawGlyphProvider : IGlyphProvider
	{
		private List<Glyph> glyphs = new List<Glyph>();
		private Color background = Color.Black;

		/// <summary>
		/// Creates a new glyph provider, with no glyphs and a black background.
		/// </summary>
		public RawGlyphProvider() { }

		/// <summary>
		/// Creates a new glyph provider that serves the specified data.
		/// </summary>
		/// <param name="glyph">The Glyph to serve.</param>
		/// <param name="background_color">The background color.</param>
		public RawGlyphProvider(Glyph glyph, Color background_color)
		{
			background = background_color;
			glyphs.Add(glyph);
		}

		/// <summary>
		/// Background Color.
		/// </summary>
		public Color BackgroundColor
		{
			get { return background; }
			set { background = value; }
		}

		/// <summary>
		/// Glyphs.
		/// </summary>
		public Glyph[] Glyphs
		{
			get { return glyphs.ToArray(); }
			set { this.glyphs = new List<Glyph>(value); }
		}

		/// <summary>
		/// Dirty.
		/// </summary>
		public bool Dirty
		{
			get;
			set;
		}

		/// <summary>
		/// Adds a glyph to this provider.
		/// </summary>
		/// <param name="glyph"></param>
		public void AddGlyph(Glyph glyph)
		{
			this.glyphs.Add(glyph);
		}

		/// <summary>
		/// Clears all glyphs from this provider.
		/// </summary>
		public void ClearGlyphs()
		{
			this.glyphs.Clear();
		}
	}
}
