using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sharplike.Mapping;
using Sharplike.Mapping.Squares;

namespace Sharplike.Editlike.MapTools
{
	public class PenTool : IMapTool
	{
		Main form = null;

		Point lastloc;

		SquareChange change;

		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}
		public void SetInactive()
		{
		}

		public void Start(Point tile)
		{
			change = new SquareChange(form.Map);
			Run(tile);
			lastloc = tile;
		}

		public void End(Point tile)
		{
			form.UndoRedo.AddChange(change);
			change = null;
			lastloc = new Point(-1, -1);
		}

		public void Run(Point tile)
		{
			if (tile != lastloc)
			{
				Vector3 squareloc = new Vector3(tile.X + form.Map.View.X,
								tile.Y + form.Map.View.Y, form.Map.View.Z);
				EditorExtensionNode node = form.SelectedSquareType();
				if (node != null)
				{
					AbstractSquare sq = (AbstractSquare)node.CreateInstance();
					change.AddOperation(form.Map.GetSafeSquare(squareloc), sq, squareloc);
					form.Map.SetSquare(squareloc, sq);
				}
				form.Map.InvalidateTiles(new Rectangle(tile, new Size(1, 1)));
				form.Map.ViewFrom(form.Map.View, true);

				lastloc = tile;
			}
		}
	}
}
