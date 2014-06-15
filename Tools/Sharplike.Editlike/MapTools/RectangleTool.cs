using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sharplike.UI;
using Sharplike.UI.Controls;
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public class RectangleTool : AbstractSelectionTool
	{
		SquareChange change;
		public override void SetActive(Main screen, String tag)
		{
			base.SetActive(screen, tag);
			fill = (tag == "Fill");
		}

		public override void Start(Point tile)
		{
			base.Start(tile);
			change = new SquareChange(form.Map);
		}

		public override void End(Point tile)
		{
			base.End(tile);

			EditorExtensionNode node = form.SelectedSquareType();
			if (node != null)
			{
				for (int y = border.Location.Y; y < border.Location.Y + border.Size.Height; ++y)
				{
					for (int x = border.Location.X; x < border.Location.X + border.Size.Width; ++x)
					{
						if (fill || 
							(x == border.Location.X || x == border.Location.X + border.Size.Width - 1 ||
							y == border.Location.Y || y == border.Location.Y + border.Size.Height - 1))
						{
							AbstractSquare sq = (AbstractSquare)node.CreateInstance();
							Vector3 loc = new Vector3(x + form.Map.View.X,
								y + form.Map.View.Y, form.Map.View.Z);

							change.AddOperation(form.Map.GetSafeSquare(loc), sq, loc);
							form.Map.SetSquare(loc, sq);
						}
					}
				}
			}
			if (change.Count > 0)
				form.UndoRedo.AddChange(change);

			change = null;
			form.Map.ViewFrom(form.Map.View, true);
		}
	}
}
