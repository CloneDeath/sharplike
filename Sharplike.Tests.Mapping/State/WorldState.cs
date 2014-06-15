using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharplike.Core.ControlFlow;
using Sharplike.Core;
using Sharplike.Mapping;

namespace Sharplike.Tests.Mapping.State
{
	class WorldState : AbstractState
	{
		PlayerEntity hacker;
		MapStack map;

		protected override void StateStarted()
		{
			Game.RenderSystem.Window.Focus();

			map = new MapStack(Game.RenderSystem.Window.Size, 20, 15, "SandboxMap", Game.RenderSystem.Window);
			map.AddPage(new BuildingPage(map.PageSize), new Vector3(0, 0, 0));

			hacker = new PlayerEntity(map);
			hacker.Location = new Vector3(10, 8, 0);

			map.ViewFrom(hacker);

			Game.InputSystem.Command.CommandSet = "World";
		}

		protected override void CommandTriggered(Sharplike.Core.Input.CommandEventArgs e)
		{
			map.BroadcastMessage(hacker.Location, new Vector3(5, 5, 1), "Ping");
			hacker.ReceiveCommand(e.CommandData.Command);

		}

		protected override void KeyPressed(System.Windows.Forms.KeyEventArgs Key)
		{
			if (Key.KeyCode == System.Windows.Forms.Keys.Space) {
				map.ViewFrom(hacker);
			}
		}

		protected override void GameProcessing()
		{
		}
	}
}
