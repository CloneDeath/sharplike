using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.UI.Controls;
using Sharplike.Core.Rendering;
using Lidgren.Network;
using System.Drawing;

namespace Sharplike.Multiplayer
{
	class RemoteTerminal : Window
	{
		public const string Handle = "RemoteTerminal";

		public RemoteTerminal(AbstractRegion parent) : base(parent) {
			this.Style = BorderStyle.None;
		}

		internal void Receive(NetIncomingMessage msg)
		{
			int x_offset = msg.ReadInt32();
			int y_offset = msg.ReadInt32();
			int x_size = msg.ReadInt32();
			int y_size = msg.ReadInt32();

			for (int x = x_offset; x < x_offset + x_size; x++) {
				for (int y = y_offset; y < y_offset + y_size; y++) {
					int provider_count = msg.ReadInt32();
					this.RegionTiles[x, y].ClearGlyphs();

					for (int i = 0; i < provider_count; i++){
						RawGlyphProvider provider = new RawGlyphProvider();
						provider.Glyphs

						int glyph_index = msg.ReadInt32();
						byte color_red = msg.ReadByte();
						byte color_green = msg.ReadByte();
						byte color_blue = msg.ReadByte();
						byte color_alpha = msg.ReadByte();

					
						this.RegionTiles[x, y].AddGlyph(glyph_index, 
							Color.FromArgb(color_alpha, color_red, color_green, color_blue),
						);
					}
					
				}
			}
		}
	}
}
