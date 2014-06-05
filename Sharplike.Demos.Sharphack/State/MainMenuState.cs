using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Sharplike.Core;
using Sharplike.Core.ControlFlow;
using Sharplike.Core.Rendering;
using Sharplike.Core.Input;
using Sharplike.UI;
using Sharplike.UI.Controls;


namespace Sharplike.Demos.Sharphack.State
{
	public class MainMenuState : AbstractState
	{
		AbstractWindow Window;

		Window dialogBox;

		public MainMenuState() : base()
		{
			Window = Game.RenderSystem.Window;

			dialogBox = new Window(Window);
			dialogBox.Size = Window.Size;
			dialogBox.Title = "New Character";
			dialogBox.Style = BorderStyle.Double;
			dialogBox.ForegroundColor = Color.DarkGray;

			Label lbl = new Label("[N]ew Game", dialogBox);
			lbl.Location = new Point(1, 1);
			lbl.Text = "[N]ew Game";
			lbl.Color = Color.Yellow;
		}

		protected override void StateStarted()
		{
			Game.InputSystem.CommandSetKey = "MainMenu";

			base.StateStarted();
		}

		protected override void GameLoopTick(Core.Runtime.AbstractGameLoop loop)
		{
			Window.InvalidateTiles();
		}

		protected override void CommandTriggered(InputSystem.CommandEventArgs e)
		{
			base.CommandTriggered(e);
		}
	}
}
