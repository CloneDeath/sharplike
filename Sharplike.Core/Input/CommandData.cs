using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharplike.Core.Input
{
	/// <summary>
	/// A command, as is given to the game loop for processing.
	/// </summary>
	public class CommandData
	{
		/// <summary>
		/// The invoked command.
		/// </summary>
		public readonly String Command;

		/// <summary>
		/// The point on screen where the mouse command occurred.
		/// </summary>
        public Point ScreenCoordinates
        {
            get
            {
                return Game.InputSystem.Input.MousePosition;
            }
        }

		/// <summary>
		/// The screen tile on which the mouse command occurred.
		/// </summary>
        public Point TileCoordinates
        {
            get
            {
                Point sc = ScreenCoordinates;
                int x = sc.X / Game.RenderSystem.Window.GlyphPalette.GlyphDimensions.Width;
				int y = sc.Y / Game.RenderSystem.Window.GlyphPalette.GlyphDimensions.Width;
                return new Point(x,y);
            }
        }

		public CommandData(String command)
		{
			this.Command = command;
		}

		public override string ToString()
		{
			String str = "CommandData: { " + this.Command;
			//{
			//    str += " (mouse) ";
			//    str += "TileXY: " + this.TileCoordinates.ToString() + " ";
			//    str += "ScreenXY: " + this.ScreenCoordinates.ToString();
			//}
			str += " }";
			return str;
		}
	}
}
