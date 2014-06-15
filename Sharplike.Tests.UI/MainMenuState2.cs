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
	class MainMenuState2 : AbstractState
	{
		protected override void StateStarted()
		{
			AbstractWindow pwin = Game.RenderSystem.Window;
			pwin.RemoveAllRegions();
			pwin.Clear();

			Window mainwin = new Window(pwin);
			mainwin.Size = pwin.Size;
			mainwin.Title = "(Press Escape)";
			mainwin.Style = BorderStyle.Double;


			Game.InputSystem.Command.CommandSet = "MainMenu";
		}

		protected override void CommandTriggered(Core.Input.CommandEventArgs e)
		{
			switch (e.CommandData.Command) {
				case "quit":
					StateMachine.PopState();
					break;
			}
		}
	}
}
