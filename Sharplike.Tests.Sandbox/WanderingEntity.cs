using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Rendering;
using Sharplike.Mapping;
using Sharplike.Mapping.Entities;
using Sharplike.UI;

namespace Sharplike.Tests.Sandbox
{
	[Serializable]
    public class WanderingEntity : AbstractEntity
    {
        public override Core.Rendering.Glyph[] Glyphs
        {
            get
            {
                return new Glyph[] { new Glyph((int)GlyphDefault.At, Color.White) };
            }
        }

        public void Wander()
        {
            Random r = new Random();
            Vector3 newloc = Location + new Vector3(r.Next(-1, 2), r.Next(-1, 2), 0);

            AbstractSquare sq = Map.GetSafeSquare(newloc);
            if (sq != null && sq.IsPassable(Direction.Here))
                Location = newloc;
        }
    }
}
