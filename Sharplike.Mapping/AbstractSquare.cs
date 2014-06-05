using System;
using System.Collections.Generic;
using System.Drawing;
using Sharplike.Mapping.Entities;

using Sharplike.Core.Rendering;

namespace Sharplike.Mapping
{
	[Serializable]
	public abstract class AbstractSquare : IGlyphProvider
	{
		public virtual Boolean IsPassable(Direction fromDirection) { return false; }
		public virtual Boolean Teleport(Direction fromDirection, AbstractEntity newLocation) { return false; }
		public virtual Color BackgroundColor { get { return Color.Black; } }

		private static Glyph[] glyphs = { };
		public virtual Glyph[] Glyphs
		{
			get
			{
				return AbstractSquare.glyphs;
			}
		}

        public virtual bool Dirty
        {
			get { return false; }
        }

		public virtual AbstractMap Map
		{
			get { return null; }
			set { }
		}

		public virtual Vector3 Location
		{
			get { return Vector3.Zero; }
			set { }
		}
    }
}
