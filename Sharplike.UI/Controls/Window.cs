﻿///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 2010 Ed Ropple.          ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

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
