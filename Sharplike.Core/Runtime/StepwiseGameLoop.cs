using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Sharplike.Core.Input;
using System.Threading;
using System.Diagnostics;

namespace Sharplike.Core.Runtime
{
	public class StepwiseGameLoop : AbstractGameLoop
	{
		/// <summary>
		/// Creates a new step-based game loop, which processes game logic
		/// only in response to a keypress.
		/// </summary>
		/// <param name="callback">The game entry point.</param>
		public StepwiseGameLoop(Action<StepwiseGameLoop> callback)
		{
			GameTick = callback;
			Game.Time = 0;
		}

		/// <summary>
		/// Creates a new step-based game loop, which processes game logic
		/// only in response to a keypress.
		/// </summary>
		/// <param name="callback">The game entry point.</param>
		/// <param name="startTime">The parameter to start the system at.</param>
		public StepwiseGameLoop(Action<StepwiseGameLoop> callback, Int64 startTime)
		{
			GameTick = callback;
			Game.Time = startTime;
		}

		/// <summary>
		/// Creates a new step-based game loop, which processes game logic
		/// only in response to a keypress.
		/// </summary>
		/// <param name="stateMachine">The StateMachine used for this game's control flow.</param>
		public StepwiseGameLoop(Sharplike.Core.ControlFlow.StateMachine stateMachine)
		{
			GameTick = stateMachine.GameLoopTick;
		}

		public override void Begin()
		{
			while (Game.Terminated == false)
			{
				GameTick(this);
				
				do {
					Game.Process();
					Application.DoEvents();
				} while (Game.InputSystem.Input.GetAllPressed().Count == 0);
			}
		}

		/// <summary>
		/// Defines the entry point of an application.
		/// </summary>
		/// <param name="loop">The StepwiseGameLoop that is in charge of this game.</param>
		private Action<StepwiseGameLoop> GameTick;
		Queue<CommandData> lastcommands = new Queue<CommandData>();
	}
}
