using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sharplike.Mapping;
using Sharplike.Mapping.Entities;

namespace Sharplike.Editlike.MapTools
{
	public class CursorTool : IMapTool
	{
		Main form;
		AbstractEntity ent;
		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}

		public void End(Point tile)
		{
			form.EntityProperties.SelectedObject = ent;
			ent = null;
		}

		public void SetInactive()
		{
			form = null;
		}

		public void Start(Point tile)
		{
			Vector3 location = new Vector3(tile.X + form.Map.View.X,
				tile.Y + form.Map.View.Y, form.Map.View.Z);
			Vector3 extents = new Vector3(1, 1, 1);
			AbstractEntity[] ents = form.Map.EntitiesInRectangularRange(location, extents);

			if (ents.Length > 0)
				ent = ents[0];


			form.EntityProperties.SelectedObject = ent;
		}

		public void Run(Point tile)
		{
			if (ent != null)
			{
				Vector3 location = new Vector3(tile.X + form.Map.View.X,
					tile.Y + form.Map.View.Y, form.Map.View.Z);
				ent.Location = location;
			}
		}
	}
}
