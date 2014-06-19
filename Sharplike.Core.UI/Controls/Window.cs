using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Rendering;

namespace Sharplike.UI.Controls
{
	public class Window : Border
	{
		public Window(AbstractRegion parent) : base(parent)
		{
			titletext = new Label(this);
			titletext.Size = new Size(this.Size.Width - 3, 1);
			titletext.Location = new Point(2, 0);
			Title = "Unnamed Window";
		}

		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
				if (titletext != null) {
					titletext.Size = new Size(this.Size.Width - 3, 1);
				}
			}
		}

		public String Title
		{
			get { return titletext.Text; }
			set { 
				titletext.Text = value;
				titletext.Size = new Size(this.Size.Width - 3, 1);
			}
		}

		public Color TitleColor
		{
			get { return titletext.Color; }
			set { titletext.Color = value; }
		}

		public override void Dispose()
		{
			base.Dispose();
			titletext.Dispose();
		}

		private Label titletext;
	}
}
