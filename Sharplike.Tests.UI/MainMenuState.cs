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
		}
	}
}
