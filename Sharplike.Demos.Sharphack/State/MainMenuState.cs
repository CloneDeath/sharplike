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

			Label welcome = new Label(dialogBox);
			welcome.Text = "Welcome to Sharphack, where is ale is sharp, and the swords are strong.";
			welcome.Location = new Point(2, 2);
			welcome.Size = new Size(dialogBox.Size.Width - 4, 5);

			Label lblNewGame = new Label("[N]ew Game", dialogBox);
			lblNewGame.Location = new Point(2, 8);
			lblNewGame.Color = Color.Yellow;

			Label lblQuit = new Label("[Q]uit Game", dialogBox);
			lblQuit.Location = new Point(2, 9);
			lblQuit.Color = Color.Yellow;
		}

		protected override void StateStarted()
		{
			Game.InputSystem.Command.CommandSet = "MainMenu";

			base.StateStarted();
		}

		protected override void GameLoopTick(Core.Runtime.AbstractGameLoop loop)
		{
			Window.Invalidate();
		}

		protected override void CommandTriggered(CommandEventArgs e)
		{
			Console.WriteLine("Got a command!");
			switch (e.CommandData.Command) {
				case "new_game":
					this.StateMachine.PushState(new DungeonState());
					break;
				case "quit":
					this.StateMachine.PopState();
					break;
			}
		}
	}
}
