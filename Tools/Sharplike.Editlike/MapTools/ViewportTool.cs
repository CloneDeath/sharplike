using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public class ViewportTool : IMapTool
	{
		Main form = null;
		Point prevloc;

		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}
		public void SetInactive()
		{
		}

		public void Start(Point tile)
		{
			prevloc = tile;
		}

		public void End(Point tile)
		{
		}

		public void Run(Point tile)
		{
			form.Map.View = form.Map.View + (new Vector3(prevloc.X - tile.X, prevloc.Y - tile.Y, 0));
			prevloc = tile;
		}
	}
}
