using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.ControlFlow;
using Sharplike.UI.Controls;
using Sharplike.Core;
using Sharplike.Core.Rendering;

namespace Sharplike.Tests.UI
{
	class MainMenuState : AbstractState
	{
		protected override void StateStarted()
		{
			AbstractWindow pwin = Game.RenderSystem.Window;
			pwin.RemoveAllRegions();
			pwin.Clear();

			Window mainwin = new Window(pwin);
			mainwin.Size = pwin.Size;
			mainwin.Title = "Sharplike.Tests.UI";
			mainwin.Style = BorderStyle.Double;

			ListBox ols = new ListBox(mainwin);
			ols.Location = new System.Drawing.Point(1, 1);
			ols.Size = new System.Drawing.Size(11, 5);
			ListBoxItem def = ols.AddItem("New Game");
			ols.AddItem("Load Game");
			ols.AddItem("Quit");

			ols.SelectedItem = def;

			ols.Focus();

			Game.InputSystem.Command.CommandSet = "MainMenu";
		}

		protected override void CommandTriggered(Core.Input.CommandEventArgs e)
		{
			switch (e.CommandData.Command) {
				case "quit":
					Game.Terminate();
					break;
			}
		}
	}
}
