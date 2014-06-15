using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Mapping.Entities;
using Sharplike.Core.Rendering;
using Sharplike.UI;
using System.Drawing;
using Sharplike.Mapping;

namespace Sharplike.Tests.Mapping.State
{
	class PlayerEntity : AbstractEntity
	{
		public PlayerEntity(AbstractMap parent) : base(parent) { }

		public override Glyph[] Glyphs
		{
			get
			{
				return new Glyph[] { new Glyph((int)GlyphDefault.At, Color.White) };
			}
		}

		public void ReceiveCommand(string command)
		{
			if (command.StartsWith("move_")) {
				switch (command) {
					case "move_d":
						this.Move(Direction.South);
						break;
					case "move_l":
						this.Move(Direction.West);
						break;
					case "move_r":
						this.Move(Direction.East);
						break;
					case "move_u":
						this.Move(Direction.North);
						break;
					case "move_dl":
						this.Move(Direction.SouthWest);
						break;
					case "move_dr":
						this.Move(Direction.SouthEast);
						break;
					case "move_ul":
						this.Move(Direction.NorthWest);
						break;
					case "move_ur":
						this.Move(Direction.NorthEast);
						break;
				}
			}
		}
	}
}
