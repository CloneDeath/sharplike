using System;
using System.Collections.Generic;
using System.Text;
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.UI.Controls
{
    public class Label : AbstractRegion
    {
        public Label(AbstractRegion parent): base(parent)
        {
			this.AutoSizeToContents = true;
        }

		public Label(string text, AbstractRegion parent) : base(parent)
		{
			this.AutoSizeToContents = true;
			this.Text = text;
		}

        public override void Render()
        {
            this.Clear();
            int y = 0;
			if (this.AutoSizeToContents) {
				this.Size = new Size(0, 0);
				this.AutoSizeToContents = true;
			}
            foreach (String line in Wrap(text))
            {
				if (y > this.Size.Height - 1) {
					if (this.AutoSizeToContents == false) {
						break;
					} else {
						this.Size = new Size(this.Size.Width, this.Size.Height + 1);
						this.AutoSizeToContents = true;
					}
				}

                int x = 0;
                foreach (char c in line)
                {
					if (x > this.Size.Width - 1) {
						if (this.AutoSizeToContents == false) {
							break;
						} else {
							this.Size = new Size(this.Size.Width + 1, this.Size.Height);
							this.AutoSizeToContents = true;
						}
					}
                    RegionTile tile = this.RegionTiles[x, y];
                    tile.ClearGlyphs();
                    tile.AddGlyph((int)c, Color, Background);
                    ++x;
                }
                ++y;
            }
        }

        private String[] Wrap(String text)
        {
            return text.Split(new String[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        public Color Background
        {
            get
            {
                return bg;
            }
            set
            {
                bg = value;
				this.Invalidate();
            }
        }
        private Color bg = Color.Black;

        public Color Color
        {
            get 
            { 
                return fg; 
            }
            set 
            { 
                fg = value;
				this.Invalidate();
            }
        }
        private Color fg = Color.White;

        public String Text
        {
            get
            {
                return text;
            }

            set 
            {
                text = value;
				this.Invalidate();
            }
        }
        private String text = "";
    }
}
