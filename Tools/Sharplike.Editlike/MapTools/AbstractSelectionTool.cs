﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sharplike.UI;
using Sharplike.UI.Controls;
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public abstract class AbstractSelectionTool : IMapTool
	{
		public static Color DefaultForegroundColor = Color.Green;
		public static Color DefaultBackgroundColor = Color.FromArgb(64, 0, 255, 0);

		Timer borderupdate = new Timer();

		protected Border border;
		protected bool fill;
		protected Main form = null;

		protected Color ForegroundColor = DefaultForegroundColor;
		protected Color BackgroundColor = DefaultBackgroundColor;

		private Point initialCoordinate;


		public AbstractSelectionTool()
		{
			borderupdate.Tick += new EventHandler(borderupdate_Tick);
			borderupdate.Interval = 16;
		}

		void borderupdate_Tick(object sender, EventArgs e)
		{
		}

		public virtual void SetActive(Main screen, string tag)
		{
			form = screen;

			border = new Border(form.Map);
			border.Size = new Size(1, 1);
			border.Location = new Point(0, 0);
			border.BackgroundColor = Color.Transparent;
			border.ForegroundColor = Color.Transparent;
		}

		public virtual void SetInactive()
		{
			border.Dispose();
			border = null;
		}

		public virtual void Start(Point tile)
		{
			border.Location = tile;
			initialCoordinate = tile;

			border.ForegroundColor = ForegroundColor;
			if (fill)
				border.BackgroundColor = BackgroundColor;
			else
				border.BackgroundColor = Color.Transparent;

			Run(tile);

			borderupdate.Enabled = true;
		}

		public virtual void End(Point tile)
		{
			if (border == null)
				return;

			borderupdate.Enabled = false;

			border.ForegroundColor = Color.Transparent;
			border.BackgroundColor = Color.Transparent;
		}

		public virtual void Run(Point tile)
		{
			int top = Math.Min(tile.Y, initialCoordinate.Y);
			int left = Math.Min(tile.X, initialCoordinate.X);
			int height = Math.Abs(tile.Y - initialCoordinate.Y);
			int width = Math.Abs(tile.X - initialCoordinate.X);

			border.Location = new Point(left, top);
			border.Size = new Size(width+1, height+1);
		}
	}
}
