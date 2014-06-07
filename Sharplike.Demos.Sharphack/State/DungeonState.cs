using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.ControlFlow;
using Sharplike.Core;
using Sharplike.Mapping;
using System.Drawing;
using Sharplike.UI.Controls;

namespace Sharplike.Demos.Sharphack.State
{
	class DungeonState : AbstractState
	{
		protected override void StateStarted()
		{
			Game.RenderSystem.Window.Clear();

			Window win = new Window(Game.RenderSystem.Window);
			win.Title = "Dungeon1";

			MapStack ms = new MapStack(new Size(30, 30), 100, 100, "Map", Game.RenderSystem.Window);
		}
	}
}
